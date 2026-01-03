using ECommerceProjesi.Entities;
namespace ECommerceProjesi.Business.Abstract
{
    public interface ISepetService
    {
        Sepet? GetirSepetByKullaniciEmail(string kullaniciEmail);
        void SepeteEkle(string kullaniciEmail, int urunVaryantiId, int adet);
        void SepettenSil(string kullaniciEmail, int sepetKalemiId);
        void SepetAdetGuncelle(string kullaniciEmail, int sepetKalemiId, int yeniAdet);
        void SepetiTemizle(int sepetId);
    }
}