using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.DataAccess;
using ECommerceProjesi.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceProjesi.Business.Concrete
{
    public class KategoriManager : IKategoriService
    {
        private readonly ECommerceContext _context;
        public KategoriManager(ECommerceContext context)
        {
            _context = context;
        }
        public List<Kategori> AltKategorileriGetir(int parentKategoriId)
        {
            return _context.Kategoriler.Where(k => k.ParentKategoriId == parentKategoriId).ToList();
        }
        public List<Kategori> AnaKategorileriGetir()
        {
            return _context.Kategoriler.Where(k => k.ParentKategoriId == null).ToList();
        }
        public void KategoriEkle(Kategori kategori)
        {
            if (kategori != null)
            {
                _context.Kategoriler.Add(kategori);
                _context.SaveChanges();
            }
        }
        public void KategoriGuncelle(Kategori kategori)
        {
            if (kategori != null)
            {
                _context.Kategoriler.Update(kategori);
                _context.SaveChanges();
            }
        }
        public Kategori? KategoriGetirById(int kategoriId)
        {
            return _context.Kategoriler.Find(kategoriId);
        }

        public void KategoriSil(int kategoriId)
        {
            var kategori = _context.Kategoriler.Find(kategoriId);
            if (kategori != null)
            {
                _context.Kategoriler.Remove(kategori);
                _context.SaveChanges();
            }
        }
        public Kategori GetirKategoriById(int id)
        {
            return _context.Kategoriler.Find(id);
        }
        public List<Kategori> TumKategorileriGetir()
        {
            return _context.Kategoriler.ToList();
        }
    }
}