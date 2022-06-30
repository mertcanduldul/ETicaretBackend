namespace Business.ServiceModel.Product;

public class OfferRequest
{
    public int idKullanici { get; set; }
    public int idUrun { get; set; }
    public int fiyat { get; set; }
}