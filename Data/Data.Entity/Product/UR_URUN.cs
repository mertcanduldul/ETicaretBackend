namespace Data.Entity.Product;

public class UR_URUN
{
    public int ID_URUN { get; set; }
    public string URUN_ADI { get; set; }
    public string URUN_ACIKLAMA { get; set; }
    public int ID_KATEGORI { get; set; }
    public int ID_RENK { get; set; }
    public int ID_MARKA { get; set; }
    public int ID_KULLANIM_DURUMU { get; set; }
    public int FIYAT { get; set; }
    public bool IS_OFFERABLE { get; set; }
    public bool IS_SOLD { get; set; }
    public DateTime CREDATE { get; set; }
    public bool DELETED { get; set; }
    public int ID_KULLANICI { get; set; }
}