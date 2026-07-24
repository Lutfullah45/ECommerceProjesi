using ECommerceProjesi.Entities;
using System.Collections.Generic;

namespace ECommerceProjesi.Business.Abstract
{
    public interface IUrunService
    {
        List<Urun> TumUrunleriGetir();
        List<Urun> UrunleriAra(string aramaMetni);
        Urun? UrunuGetirById(int urunId);
        void UrunEkle(Urun urun);
        void UrunGuncelle(Urun urun);
        void UrunSil(int urunId);
        List<Urun> KategoriyeGoreUrunleriGetir(int kategoriId);
        List<UrunVaryanti> UrunVaryantlariniGetir(int urunId);
        List<UrunResmi> UrunResimleriniGetir(int urunId);
    }
}