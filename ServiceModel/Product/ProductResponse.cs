namespace Data.Entity.Product;

public class ProductResponse
{
    public int ID_URUN { get; set; }
    public string URUN_ADI { get; set; }
    public string URUN_ACIKLAMA { get; set; }
    public int FIYAT { get; set; }
    public bool IS_SOLD { get; set; }
    public bool IS_OFFERABLE { get; set; }
    public string KATEGORI_ADI { get; set; }
    public string RENK_ADI { get; set; }
    public string MARKA_ADI { get; set; }
    public string KULLANIM_DURUM_ADI { get; set; }
    public int ID_URUN_SAHIBI { get; set; }
}