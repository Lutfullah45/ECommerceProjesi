using ECommerceProjesi.Entities;
using System.Collections.Generic;

namespace ECommerceProjesi.Business.Abstract
{
    public interface IUrunResmiService
    {
        List<UrunResmi> GetirResimlerByUrunId(int urunId);
        UrunResmi? GetirResimById(int resimId);
        void ResimEkle(UrunResmi resim);
        void ResimSil(UrunResmi resim);
        void AnaResimYap(int urunId, int resimId);
    }
}