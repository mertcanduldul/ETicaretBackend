namespace Data.Entity.Product;

public class SP_TEKLIF
{
    public int ID_TEKLIF { get; set; }
    public int ID_URUN { get; set; }
    public decimal TEKLIF_FIYAT { get; set; }
    public int ID_TEKLIF_DURUM { get; set; }
    public int ID_URUN_SAHIBI { get; set; }
    public int ID_TEKLIF_VEREN { get; set; }
    public DateTime CREDATE { get; set; }
    public bool DELETED { get; set; }
}