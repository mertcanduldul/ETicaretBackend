namespace Business.ServiceModel.Product;

public class AddProductRequest
{
    public string URUN_ADI { get; set; }
    public string URUN_ACIKLAMA { get; set; }
    public int ID_KATEGORI { get; set; }
    public int ID_RENK { get; set; }
    public int ID_MARKA { get; set; }
    public int ID_KULLANIM_DURUMU { get; set; }
    public int FIYAT { get; set; }
    public bool IS_OFFERABLE { get; set; }
}