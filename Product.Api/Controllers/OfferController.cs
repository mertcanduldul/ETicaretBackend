using Business.ServiceModel.Product;
using Data.Dapper.Repository.Product;
using Data.Entity.Product;
using Microsoft.AspNetCore.Mvc;
using ServiceModel;

namespace Product.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfferController : ControllerBase
{
    private readonly ILogger<OfferController> _logger;

    public OfferController(ILogger<OfferController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        return "Successfully reached - ETicaretBackend.Product.Api - OfferController";
    }

    [HttpGet]
    [Route("GetProductOffer")]
    public IActionResult GetProductOffer(int idUrun)
    {
        OfferRepository offerRepository = new OfferRepository();
        SP_TEKLIF offer = offerRepository.GetLastOfferByIdProduct(idUrun).FirstOrDefault();
        return Ok(offer != null ? offer : "No offer found");
    }

    [HttpGet]
    [Route("PostOfferOfUser")]
    [Tags("Kullanıcının Verdiği Teklifleri Listeler !")]
    public IActionResult PostOfferOfUser(int idUser)
    {
        OfferRepository offerRepository = new OfferRepository();
        var offer = offerRepository.PostOfferOfUser(idUser).ToList();
        return Ok(offer != null ? offer : "No offer found");
    }

    [HttpGet]
    [Route("GetOfferOfUser")]
    [Tags("Kullanıcıya Gelen Teklifleri Listeler !")]
    public IActionResult GetOfferOfUser(int idUser)
    {
        OfferRepository offerRepository = new OfferRepository();
        var offer = offerRepository.GetOfferOfUser(idUser).ToList();
        return Ok(offer != null ? offer : "No offer found");
    }

    [HttpGet]
    [Route("GetOfferStatus")]
    public IActionResult GetOfferStatus()
    {
        OfferRepository offerRepository = new OfferRepository();
        var offer = offerRepository.GetOfferStatus();
        return Ok(offer);
    }

    [HttpPost]
    [Route("AddOffer")]
    public ServicesResponse AddOffer(OfferRequest request)
    {
        OfferRepository offerRepository = new OfferRepository();
        offerRepository.AddOffer(request.idKullanici, request.idUrun, request.fiyat, request.idUrunSahibi);
        return new ServicesResponse
        {
            Message = "Teklif Verildi !",
            IsSuccess = true
        };
    }

    [HttpPost]
    [Route("DeleteOffer")]
    public ServicesResponse DeleteOffer(SP_TEKLIF offer)
    {
        OfferRepository offerRepository = new OfferRepository();
        offerRepository.Delete(offer);
        return new ServicesResponse
        {
            IsSuccess = true,
            Message = "Teklif Silindi !"
        };
    }

    [HttpPost]
    [Route("AcceptOffer")]
    public ServicesResponse AcceptOffer(int idOffer, int idKullanici)
    {
        OfferRepository offerRepository = new OfferRepository();
        offerRepository.AcceptOffer(idOffer, idKullanici);
        return new ServicesResponse
        {
            IsSuccess = true,
            Message = "Teklif Kabul Edildi !"
        };
    }

    [HttpPost]
    [Route("RejectOffer")]
    public ServicesResponse RejectOffer(int idOffer)
    {
        OfferRepository offerRepository = new OfferRepository();
        offerRepository.RejectOffer(idOffer);
        return new ServicesResponse
        {
            IsSuccess = true,
            Message = "Teklif Reddedildi !"
        };
    }

    [HttpPost]
    [Route("BuyProduct")]
    public ServicesResponse BuyProduct(BuyProductRequest request)
    {
        OfferRepository offerRepository = new OfferRepository();
        offerRepository.BuyProduct(request.idKullanici, request.idUrun);
        return new ServicesResponse
        {
            IsSuccess = true,
            Message = "Ürün Satıldı !"
        };
    }
}