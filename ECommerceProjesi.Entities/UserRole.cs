namespace ECommerceProjesi.Entities
{
    public class KullaniciRol
    {
        public int MusteriId { get; set; }
        public virtual Musteri Musteri { get; set; }
        public int RolId { get; set; }
        public virtual Rol Rol { get; set; }
    }
}