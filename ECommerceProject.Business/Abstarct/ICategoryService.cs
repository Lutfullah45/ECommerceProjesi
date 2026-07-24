using ECommerceProjesi.Entities;
using System.Collections.Generic;

namespace ECommerceProjesi.Business.Abstract
{
    public interface IKategoriService
    {
        List<Kategori> TumKategorileriGetir();
        Kategori? KategoriGetirById(int kategoriId);
        void KategoriEkle(Kategori kategori);
        void KategoriGuncelle(Kategori kategori);
        void KategoriSil(int kategoriId);
        Kategori GetirKategoriById(int id);
        List<Kategori> AnaKategorileriGetir(); 
        List<Kategori> AltKategorileriGetir(int parentKategoriId);
    }
}