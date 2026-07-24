using ECommerceProjesi.Entities;
using System.Collections.Generic;

namespace ECommerceProjesi.Business.Abstract
{
    public interface IAdresService
    {
        void AdresEkle(Adres adres);
        Adres GetirAdresById(int adresId);
        List<Adres> GetirKullaniciAdresleri(string userId);
        void AdresSil(Adres adres);
        void AdresGuncelle(Adres adres);
    }
}