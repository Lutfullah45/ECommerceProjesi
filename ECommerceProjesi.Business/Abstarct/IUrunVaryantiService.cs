using ECommerceProjesi.Entities;
using System.Collections.Generic;

namespace ECommerceProjesi.Business.Abstract
{
    public interface IUrunVaryantiService
    {
        List<UrunVaryanti> GetirVaryantlarByUrunId(int urunId);
        UrunVaryanti? GetirVaryantById(int varyantId);
        void VaryantEkle(UrunVaryanti varyant);
        void VaryantGuncelle(UrunVaryanti varyant);
        void VaryantSil(int varyantId);
    }
}