using ECommerceProjesi.Entities;

namespace ECommerceProjesi.Business.Abstract
{
    public interface IMusteriService
    {
        Musteri? GetirMusteriByEmail(string email);
        void MusteriEkle(Musteri musteri);
        void MusteriGuncelle(Musteri musteri);
    }
}