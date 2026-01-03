using ECommerceProjesi.Entities;
using System.Collections.Generic;
namespace ECommerceProjesi.Business.Abstract
{
    public interface ISiparisService
    {
        Siparis SiparisOlustur(Sepet sepet, int teslimatAdresId, int faturaAdresId);
        List<Siparis> GetirSiparislerByMusteriId(int musteriId);
        Siparis? GetirSiparisDetay(int siparisId);
    }
}