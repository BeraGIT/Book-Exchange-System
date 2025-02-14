using API.LoginCtrl;
using API.Models.Books.RequestModels;
using API.Models.Books.ResponseModels;
using BLL.ManagerServices.Interfaces;
using ENTITIES.Enums;
using ENTITIES.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IManager<Book> _bookManager;
        private readonly LoginStatus _loginStatus;

        public BookController(IManager<Book> bookManager, LoginStatus loginStatus)
        {
            _bookManager = bookManager;
            _loginStatus = loginStatus;
        }

        [HttpPost("CreateBook")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequestModel request)
        {
            // ModelState validasyonu
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys
                    .SelectMany(key => ModelState[key].Errors.Select(error => new
                    {
                        field = key,
                        message = error.ErrorMessage
                    })).ToList();

                return BadRequest(new { success = false, errors });
            }

            // Kullanıcı oturum kontrolü
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

            // Yeni kitap oluşturma
            var newBook = new Book
            {
                Title = request.Title,
                Description = request.Description,
                Author = request.Author,
                UserID = _loginStatus.LoggedInUserId,
            };

            // Kitap ekleme işlemi
            await _bookManager.AddAsync(newBook);

            return Ok(new
            {
                success = true,
                message = "Kitap başarıyla oluşturuldu!",
                data = new
                {
                    BookId = newBook.ID,
                    Title = newBook.Title,
                    Author = newBook.Author,
                    UserID = newBook.UserID,
                    CreatedDate = newBook.CreatedDate, // BaseEntity'den otomatik olarak atanır
                    BookStatus = newBook.BStatus,
                    Status = newBook.Status
                }
            });
        }

        [HttpDelete("DeleteBook/{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            try
            {
                // Kullanıcının giriş yapıp yapmadığını kontrol et
                if (!_loginStatus.IsLoggedIn)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Kullanıcı giriş yapmamış!"
                    });
                }

                var userId = _loginStatus.LoggedInUserId;
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Kullanıcı bilgisi bulunamadı!"
                    });
                }

                // Kitabı bul
                var book = await _bookManager.FirstOrDefaultAsync(b => b.ID == id && b.UserID == userId);
                if (book == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Kitap bulunamadı veya bu kullanıcıya ait değil!"
                    });
                }

                // Kitabı sil
                await _bookManager.DeleteAsync(book);
             


                return Ok(new
                {
                    success = true,
                    message = "Kitap başarıyla silindi!",
                    data = new
                    {
                        BookId = book.ID,
                        Title = book.Title,
                        DeletedDate = book.DeletedDate
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Kitap silinirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }


        [HttpGet("GetUserBooks")]
        public async Task<IActionResult> GetUserBooks()
        {
            try
            {
                // Kullanıcının giriş yapıp yapmadığını kontrol et
                if (!_loginStatus.IsLoggedIn)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Kullanıcı giriş yapmamış!"
                    });
                }

                var userId = _loginStatus.LoggedInUserId;
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Kullanıcı bilgisi bulunamadı!"
                    });
                }

                // Kullanıcının kitaplarını getir
                var userBooks =  _bookManager.Where(b => b.UserID == userId && b.Status != ENTITIES.Enums.DataStatus.Deleted);
                if (userBooks == null || !userBooks.Any())
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Bu kullanıcıya ait kitap bulunamadı!"
                    });
                }

                // Kitapları liste olarak döndür
                return Ok(new
                {
                    success = true,
                    message = "Kullanıcının kitapları başarıyla getirildi!",
                    data = userBooks.Select(b => new
                    {
                        BookId = b.ID,
                        Title = b.Title,
                        Description = b.Description,
                        Author = b.Author,
                        BookStatus = b.BStatus.ToString(),
                        CreatedDate = b.CreatedDate
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Kullanıcının kitapları getirilirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }
        [HttpGet("GetBook/{id}")]
        public async Task<IActionResult> GetBook(string id)
        {
            try
            {
                // Kitabı bul
                var book = await _bookManager.FirstOrDefaultAsync(b => b.ID == id && b.Status != ENTITIES.Enums.DataStatus.Deleted);
                if (book == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Kitap bulunamadı!"
                    });
                }

                // Kitap detaylarını döndür
                return Ok(new
                {
                    success = true,
                    message = "Kitap başarıyla getirildi!",
                    data = new
                    {
                        BookId = book.ID,
                        Title = book.Title,
                        Description = book.Description,
                        Author = book.Author,
                        Status = book.BStatus.ToString(),
                        CreatedDate = book.CreatedDate
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Kitap getirilirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }




    }
}

