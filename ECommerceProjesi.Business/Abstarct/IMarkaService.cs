using ECommerceProjesi.Entities;
using System.Collections.Generic;

namespace ECommerceProjesi.Business.Abstract
{
    public interface IMarkaService
    {
        List<Marka> TumMarkalariGetir();
        Marka? MarkaGetirById(int markaId);
        void MarkaEkle(Marka marka);
        void MarkaGuncelle(Marka marka);
        void MarkaSil(int markaId);
    }
}