using ECommerceProjesi.Business.Abstract;
using ECommerceProjesi.DataAccess;
using ECommerceProjesi.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceProjesi.Business.Concrete
{
    public class MarkaManager : IMarkaService
    {
        private readonly ECommerceContext _context;
        public MarkaManager(ECommerceContext context)
        {
            _context = context;
        }
        public List<Marka> TumMarkalariGetir()
        {
            return _context.Markalar.ToList();
        }
        public Marka? MarkaGetirById(int markaId)
        {
            return _context.Markalar.Find(markaId);
        }
        public void MarkaEkle(Marka marka)
        {
            if (marka != null)
            {
                _context.Markalar.Add(marka);
                _context.SaveChanges();
            }
        }
        public void MarkaGuncelle(Marka marka)
        {
            if (marka != null)
            {
                _context.Markalar.Update(marka);
                _context.SaveChanges();
            }
        }
        public void MarkaSil(int markaId)
        {
            var marka = _context.Markalar.Find(markaId);
            if (marka != null)
            {
                // Dikkat: Bu markaya bağlı ürünler varsa silme işlemi hata verebilir.
                _context.Markalar.Remove(marka);
                _context.SaveChanges();
            }
        }
    }
}