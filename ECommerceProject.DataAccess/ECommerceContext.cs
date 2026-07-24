using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ECommerceProjesi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProjesi.DataAccess
{
    public class ECommerceContext : IdentityDbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options)
            : base(options)
        {
        }

        // E-Ticaret Tabloları
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Adres> Adresler { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<KullaniciRol> KullaniciRolleri { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Marka> Markalar { get; set; }
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<UrunVaryanti> UrunVaryantilari { get; set; }
        public DbSet<UrunResmi> UrunResimleri { get; set; }
        public DbSet<Yorum> Yorumlar { get; set; }
        public DbSet<Sepet> Sepetler { get; set; }
        public DbSet<Favori> Favoriler { get; set; }
        public DbSet<SepetKalemi> SepetKalemleri { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisKalemi> SiparisKalemleri { get; set; }
        public DbSet<SiparisDurumu> SiparisDurumlari { get; set; }
        public DbSet<Odeme> Odemeler { get; set; }
        public DbSet<Fatura> Faturalar { get; set; }
        public DbSet<KargoSirketi> KargoSirketleri { get; set; }
        public DbSet<Teslimat> Teslimatlar { get; set; }
        public DbSet<IndirimKuponu> IndirimKuponlari { get; set; }
        public DbSet<IstekListesi> IstekListeleri { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Siparis>()
                .HasOne(s => s.SiparisDurumu)
                .WithMany(sd => sd.Siparisler)
                .HasForeignKey(s => s.SiparisDurumId);

            modelBuilder.Entity<UrunVaryanti>().Property(p => p.Fiyat).HasPrecision(18, 2);
            modelBuilder.Entity<Siparis>().Property(p => p.ToplamTutar).HasPrecision(18, 2);
            modelBuilder.Entity<SiparisKalemi>().Property(p => p.BirimFiyat).HasPrecision(18, 2);
            modelBuilder.Entity<Odeme>().Property(p => p.Tutar).HasPrecision(18, 2);
            modelBuilder.Entity<Fatura>().Property(p => p.ToplamKDV).HasPrecision(18, 2);
            modelBuilder.Entity<Fatura>().Property(p => p.GenelToplam).HasPrecision(18, 2);
            modelBuilder.Entity<IndirimKuponu>().Property(p => p.IndirimTutari).HasPrecision(18, 2);
            modelBuilder.Entity<IndirimKuponu>().Property(p => p.IndirimOrani).HasPrecision(18, 2);

            modelBuilder.Entity<KullaniciRol>()
                .HasKey(kr => new { kr.MusteriId, kr.RolId });

            modelBuilder.Entity<IstekListesi>()
                .HasKey(il => new { il.MusteriId, il.UrunVaryantiId });

            modelBuilder.Entity<Musteri>()
                .HasOne(m => m.Sepet)
                .WithOne(s => s.Musteri)
                .HasForeignKey<Sepet>(s => s.MusteriId);

            modelBuilder.Entity<Siparis>()
                .HasOne(s => s.Fatura)
                .WithOne(f => f.Siparis)
                .HasForeignKey<Fatura>(f => f.SiparisId);

            modelBuilder.Entity<Siparis>()
                .HasOne(s => s.Teslimat)
                .WithOne(t => t.Siparis)
                .HasForeignKey<Teslimat>(t => t.SiparisId);

            modelBuilder.Entity<Siparis>()
                .HasOne(s => s.TeslimatAdresi)
                .WithMany()
                .HasForeignKey(s => s.TeslimatAdresId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Siparis>()
                .HasOne(s => s.FaturaAdresi)
                .WithMany()
                .HasForeignKey(s => s.FaturaAdresId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}