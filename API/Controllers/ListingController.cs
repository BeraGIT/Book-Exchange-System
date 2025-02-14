using API.LoginCtrl;
using API.Models.Listings.RequestModels;
using BLL.ManagerServices.Interfaces;
using ENTITIES.Enums;
using ENTITIES.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListingController : ControllerBase
    {
        private readonly IListingManager _listingManager;
        private readonly IOfferManager _offerManager;
        private readonly IBookManager _bookManager;
        private readonly LoginStatus _loginStatus;
        private readonly IFacultyManager _facultyManager;

        public ListingController(IListingManager listingManager, IOfferManager offerManager, IBookManager bookManager, LoginStatus loginStatus, IFacultyManager facultyManager)
        {
            _listingManager = listingManager;
            _offerManager = offerManager;
            _bookManager = bookManager;
            _loginStatus = loginStatus;
            _facultyManager = facultyManager;
        }

        private IActionResult CheckLoginStatus()
        {
            if (!_loginStatus.IsLoggedIn)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Kullanıcı giriş yapmamış!"
                });
            }

            if (string.IsNullOrWhiteSpace(_loginStatus.LoggedInUserId))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Kullanıcı bilgisi bulunamadı!"
                });
            }

            return null;
        }
        [HttpPost("CreateListing")]
        public async Task<IActionResult> CreateListing([FromBody] ListingCreateModel request)
        {
            // ModelState doğrulaması
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys
                    .SelectMany(key => ModelState[key].Errors.Select(error => new
                    {
                        field = key,
                        message = error.ErrorMessage
                    }));
                return BadRequest(new
                {
                    success = false,
                    errors
                });
            }

            // Kullanıcı giriş kontrolü
            var loginStatusCheck = CheckLoginStatus();
            if (loginStatusCheck != null) return loginStatusCheck;

            try
            {
                var userId = _loginStatus.LoggedInUserId;

                var book = await _bookManager.FirstOrDefaultAsync(b => b.ID == request.BookID && b.UserID == userId);
                if (book == null || book.BStatus != BookStatus.Usable)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = book == null ? "Kitap bulunamadı veya bu kullanıcıya ait değil!" : "Kitap kullanılabilir durumda değil!"
                    });
                }

                var faculty = await _facultyManager.FirstOrDefaultAsync(f => f.ID == request.FacultyID);
                if (faculty == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Fakülte bilgisi bulunamadı!"
                    });
                }

                var newListing = new Listing
                {
                    BookID = book.ID,
                    UserID = userId,
                    FacultyID = request.FacultyID,
                    ListingDescription = request.ListingDescription,
                    LStatus = ListingStatus.Open,
                    Status = DataStatus.Created,
                    CreatedDate = DateTime.UtcNow
                };

                await _listingManager.AddAsync(newListing);

                // Kitap durumu güncelleniyor
                book.BStatus = BookStatus.Used;
                await _bookManager.UpdateAsync(book);

                return Ok(new
                {
                    success = true,
                    message = "İlan başarıyla oluşturuldu!",
                    data = new
                    {
                        ListingId = newListing.ID,
                        BookTitle = book.Title,
                        FacultyName = faculty.FacultyName,
                        Description = newListing.ListingDescription,
                        Status = newListing.LStatus.ToString()
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "İlan oluşturulurken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("DeleteListing/{id}")]
        public async Task<IActionResult> DeleteListing(string id)
        {
            // Parametre doğrulama
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "İlan ID boş olamaz."
                });
            }

            // Kullanıcı giriş kontrolü
            var loginStatusCheck = CheckLoginStatus();
            if (loginStatusCheck != null) return loginStatusCheck;

            try
            {
                var userId = _loginStatus.LoggedInUserId;

                // İlanın doğruluğunu kontrol et
                var listing = await _listingManager.FirstOrDefaultAsync(l => l.ID == id && l.UserID == userId);
                if (listing == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "İlan bulunamadı veya bu kullanıcıya ait değil!"
                    });
                }

                // Kitap durumunu eski haline getirme
                var book = await _bookManager.FirstOrDefaultAsync(b => b.ID == listing.BookID);
                if (book != null && book.BStatus == BookStatus.Used)
                {
                    book.BStatus = BookStatus.Usable;
                    await _bookManager.UpdateAsync(book);
                }

                // İlanı sil
                _listingManager.Destroy(listing);

                return Ok(new
                {
                    success = true,
                    message = "İlan başarıyla silindi ve kitabın durumu eski haline getirildi!",
                    data = new { ListingId = listing.ID }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "İlan silinirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }


        [HttpGet("GetUserListings")]
        public IActionResult GetUserListings()
        {
            // Kullanıcı giriş kontrolü
            var loginStatusCheck = CheckLoginStatus();
            if (loginStatusCheck != null) return loginStatusCheck;

            try
            {
                var userId = _loginStatus.LoggedInUserId;

                // Kullanıcının ilanlarını getir
                var userListings = _listingManager.Where(l => l.UserID == userId && l.Status != DataStatus.Deleted).ToList();
                if (!userListings.Any())
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Kullanıcıya ait ilan bulunamadı!"
                    });
                }

                // Başarılı yanıt
                return Ok(new
                {
                    success = true,
                    message = "Kullanıcının ilanları başarıyla getirildi!",
                    data = userListings.Select(l => new
                    {
                        ListingId = l.ID,
                        BookTitle = l.Book?.Title,
                        FacultyName = l.Faculty?.FacultyName,
                        Description = l.ListingDescription,
                        Status = l.LStatus.ToString(),
                        CreatedDate = l.CreatedDate
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Kullanıcının ilanları getirilirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("GetListingDetails/{id}")]
        public async Task<IActionResult> GetListingDetails(string id)
        {
            // Parametre doğrulama
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "İlan ID boş olamaz."
                });
            }

            try
            {
                // İlanı getir
                var listing = await _listingManager.FirstOrDefaultAsync(l => l.ID == id && l.Status != DataStatus.Deleted);
                if (listing == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "İlan bulunamadı veya silinmiş!"
                    });
                }

                // İlanla ilişkili kitabı kontrol et
                var book = await _bookManager.FirstOrDefaultAsync(b => b.ID == listing.BookID);
                if (book == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "İlan ile ilişkili kitap bulunamadı!"
                    });
                }

                // Fakülte bilgisini kontrol et
                var faculty = await _facultyManager.FirstOrDefaultAsync(f => f.ID == listing.FacultyID);
                if (faculty == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Fakülte bilgisi bulunamadı!"
                    });
                }

                // Başarılı yanıt
                return Ok(new
                {
                    success = true,
                    message = "İlan detayları başarıyla getirildi!",
                    data = new
                    {
                        ListingId = listing.ID,
                        Book = new
                        {
                            BookId = book.ID,
                            Title = book.Title,
                            Author = book.Author,
                            Description = book.Description
                        },
                        Faculty = new
                        {
                            FacultyId = faculty.ID,
                            Name = faculty.FacultyName,
                            Address = faculty.FacultyAddress
                        },
                        ListingDescription = listing.ListingDescription,
                        Status = listing.LStatus.ToString(),
                        CreatedDate = listing.CreatedDate
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "İlan detayları getirilirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("GetAllListings")]
        public IActionResult GetListings()
        {
            try
            {
                // Tüm aktif ilanları getir
                var listings = _listingManager.Where(l => l.Status != DataStatus.Deleted && l.LStatus == ListingStatus.Open).ToList();
                if (!listings.Any())
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Aktif ilan bulunamadı!"
                    });
                }

                // Başarılı yanıt
                return Ok(new
                {
                    success = true,
                    message = "İlanlar başarıyla getirildi!",
                    data = listings.Select(l => new
                    {
                        ListingId = l.ID,
                        BookTitle = l.Book?.Title,
                        FacultyName = l.Faculty?.FacultyName,
                        Description = l.ListingDescription,
                        Status = l.LStatus.ToString(),
                        CreatedDate = l.CreatedDate
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "İlanlar getirilirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }

    }
}


