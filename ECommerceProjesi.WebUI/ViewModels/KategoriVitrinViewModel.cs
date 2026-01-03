using ECommerceProjesi.Entities;
public class KategoriVitrinViewModel
{
    public int KategoriId { get; set; }
    public string KategoriAdi { get; set; }
    public string KategoriUrl { get; set; } 
    public List<Urun> Urunler { get; set; } 
}