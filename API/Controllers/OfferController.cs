using API.LoginCtrl;
using API.Models.Offers.RequestModels;
using BLL.ManagerServices.Interfaces;
using ENTITIES.Enums;
using ENTITIES.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferController : ControllerBase
    {
        private readonly IOfferManager _offerManager;
        private readonly IBookManager _bookManager;
        private readonly IListingManager _listingManager;
        private readonly LoginStatus _loginStatus;

        public OfferController(
            IOfferManager offerManager,
            IBookManager bookManager,
            IListingManager listingManager,
            LoginStatus loginStatus)
        {
            _offerManager = offerManager;
            _bookManager = bookManager;
            _listingManager = listingManager;
            _loginStatus = loginStatus;
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

        [HttpPost("CreateOffer")]
        public async Task<IActionResult> CreateOffer([FromBody] OfferCreateModel request)
        {
            var loginStatusCheck = CheckLoginStatus();
            if (loginStatusCheck != null) return loginStatusCheck;

            try
            {
                var userId = _loginStatus.LoggedInUserId;

                var listing = await _listingManager.FirstOrDefaultAsync(l => l.ID == request.ListingID && l.Status != DataStatus.Deleted);
                if (listing == null || listing.UserID == userId)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = listing == null ? "İlan bulunamadı veya silinmiş!" : "Kendi ilanınıza teklif yapamazsınız!"
                    });
                }

                var offeredBook = await _bookManager.FirstOrDefaultAsync(b => b.ID == request.OfferedBookID && b.UserID == userId && b.Status != DataStatus.Deleted);
                if (offeredBook == null || offeredBook.BStatus != BookStatus.Usable)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = offeredBook == null ? "Kitap bulunamadı veya geçerli değil!" : "Kitap kullanılabilir durumda değil!"
                    });
                }

                var newOffer = new Offer
                {
                    ListingID = listing.ID,
                    UserID = userId,
                    OfferedBookID = offeredBook.ID,
                    OStatus = OfferStatus.Pending,
                    Status = DataStatus.Created,
                    CreatedDate = DateTime.UtcNow
                };

                await _offerManager.AddAsync(newOffer);

                offeredBook.BStatus = BookStatus.Used;
                await _bookManager.UpdateAsync(offeredBook);

                return Ok(new
                {
                    success = true,
                    message = "Teklif başarıyla oluşturuldu!",
                    data = new
                    {
                        OfferId = newOffer.ID,
                        ListingId = listing.ID,
                        OfferedBookTitle = offeredBook.Title,
                        OfferStatus = newOffer.OStatus.ToString()
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Teklif oluşturulurken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("DeleteOffer/{id}")]
        public async Task<IActionResult> DeleteOffer(string id)
        {
            var loginStatusCheck = CheckLoginStatus();
            if (loginStatusCheck != null) return loginStatusCheck;

            try
            {
                var userId = _loginStatus.LoggedInUserId;

                var offer = await _offerManager.FirstOrDefaultAsync(o => o.ID == id && o.UserID == userId);
                if (offer == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Teklif bulunamadı!"
                    });
                }

                var offeredBook = await _bookManager.FirstOrDefaultAsync(b => b.ID == offer.OfferedBookID);
                if (offeredBook != null && offeredBook.BStatus == BookStatus.Used)
                {
                    offeredBook.BStatus = BookStatus.Usable;
                    await _bookManager.UpdateAsync(offeredBook);
                }

               
                //Silme
                await _offerManager.DeleteAsync(offer);

                return Ok(new
                {
                    success = true,
                    message = "Teklif başarıyla silindi!",
                    data = new { OfferId = offer.ID }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Teklif silinirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("GetUserOffers")]
        public IActionResult GetUserOffers()
        {
            var loginStatusCheck = CheckLoginStatus();
            if (loginStatusCheck != null) return loginStatusCheck;

            try
            {
                var userId = _loginStatus.LoggedInUserId;

                var offers = _offerManager.Where(o => o.UserID == userId && o.Status != DataStatus.Deleted).ToList();
                if (!offers.Any())
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Kullanıcının yaptığı teklif bulunamadı!"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Teklifler başarıyla getirildi!",
                    data = offers.Select(o => new
                    {
                        OfferId = o.ID,
                        ListingId = o.ListingID,
                        OfferedBookTitle = o.OfferedBook.Title,
                        OfferStatus = o.OStatus.ToString()
                    })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Teklifler getirilirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("AcceptOrRejectOffer")]
        public async Task<IActionResult> AcceptOrRejectOffer([FromBody] OfferDecisionModel request)
        {
            var loginStatusCheck = CheckLoginStatus();
            if (loginStatusCheck != null) return loginStatusCheck;

            try
            {
                var userId = _loginStatus.LoggedInUserId;

                // Teklif bulunup bulunmadığını kontrol et
                var offer = await _offerManager.FirstOrDefaultAsync(o => o.ID == request.OfferID && o.Status != DataStatus.Deleted);
                if (offer == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Teklif bulunamadı!"
                    });
                }

                // Kullanıcının kendi ilanı için teklifi kabul/reddetmeye yetkisi olup olmadığını kontrol et
                var listing = await _listingManager.FirstOrDefaultAsync(l => l.ID == offer.ListingID && l.UserID == userId);
                if (listing == null)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Bu teklif üzerinde işlem yapma yetkiniz yok!"
                    });
                }

                // Durum değişikliği
                if (request.Accept)
                {
                    offer.OStatus = OfferStatus.Accepted;

                    // Diğer teklifleri kapat
                    var otherOffers = _offerManager.Where(o => o.ListingID == offer.ListingID && o.ID != offer.ID && o.Status != DataStatus.Deleted).ToList();
                    foreach (var otherOffer in otherOffers)
                    {
                        otherOffer.OStatus = OfferStatus.Closed;
                        await _offerManager.UpdateAsync(otherOffer);
                    }

                    // İlgili ilanın durumunu kapat
                    listing.LStatus = ListingStatus.Completed;
                    await _listingManager.UpdateAsync(listing);
                }
                else
                {
                    offer.OStatus = OfferStatus.Rejected;
                }

                await _offerManager.UpdateAsync(offer);

                return Ok(new
                {
                    success = true,
                    message = request.Accept ? "Teklif kabul edildi!" : "Teklif reddedildi!",
                    data = new
                    {
                        OfferId = offer.ID,
                        Status = offer.OStatus.ToString()
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Teklif işlemi sırasında bir hata oluştu.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("GetOffersForUserListings")]
        public async Task<IActionResult> GetOffersForUserListings()
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

                // Kullanıcının ilanlarına gelen teklifleri getir
                var listingIds = userListings.Select(l => l.ID).ToList();
                var offers = _offerManager.Where(o => listingIds.Contains(o.ListingID) && o.Status != DataStatus.Deleted).ToList();
                if (!offers.Any())
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "İlanlarınıza gelen teklif bulunamadı!"
                    });
                }

                // Tekliflerin detaylarını hazırlayın
                var response = offers.Select(o => new
                {
                    OfferId = o.ID,
                    ListingId = o.ListingID,
                    OfferedBookTitle = o.OfferedBook?.Title,
                    OfferStatus = o.OStatus.ToString(),
                    CreatedDate = o.CreatedDate,
                    ListingDescription = userListings.FirstOrDefault(l => l.ID == o.ListingID)?.ListingDescription
                });

                return Ok(new
                {
                    success = true,
                    message = "İlanlarınıza gelen teklifler başarıyla getirildi!",
                    data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "İlanlarınıza gelen teklifler getirilirken bir hata oluştu.",
                    error = ex.Message
                });
            }
        }


    }
}

