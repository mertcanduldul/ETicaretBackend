namespace Business.ServiceModel.Product;

public class GetOfferResponse
{
    public int ID_OFFER { get; set; }
    public string URUN_ADI { get; set; }
    public string TEKLIF_DURUM_ADI { get; set; }
    public string TEKLIF_VEREN { get; set; }
    public string URUN_SAHIBI { get; set; }
    public int TEKLIF_FIYATI { get; set; }
}