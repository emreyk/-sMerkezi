using MYARCH.CORE;
using MYARCH.CORE.Entities;
using System;
using System.Data.Entity;

namespace MYARCH.DATA.Context
{
    public partial class MyArchContext : DbContext
    {
        private readonly MyArchContext _context;
        public MyArchContext()
            : base("name=MyArchEntities")
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<kullanici> kullanici { get; set; }
        public virtual DbSet<blok> blok { get; set; }
        public virtual DbSet<tipler> tipler { get; set; }
        public virtual DbSet<personel> personel { get; set; }
        public virtual DbSet<anasayac> anasayac { get; set; }
        public virtual DbSet<sayac_tipleri> sayac_tipleri { get; set; }
        public virtual DbSet<anasayac_ortak_dagitim> anasayac_ortak_dagitim { get; set; }
        public virtual DbSet<bagimsiz_bolum_sayaclari> bagimsiz_bolum_sayaclari { get; set; }
        public virtual DbSet<bagimsiz_bolumler> bagimsiz_bolumler { get; set; }
        public virtual DbSet<kisiler> kisiler { get; set; }
        public virtual DbSet<bagimsiz_bolum_kisiler> bagimsiz_bolum_kisiler { get; set; }
        public virtual DbSet<hesap_hareket> hesap_hareket { get; set; }
        public virtual DbSet<kasa> kasa { get; set; }
        public virtual DbSet<banka> banka { get; set; }
        public virtual DbSet<banka_hareket> banka_hareket { get; set; }
        public virtual DbSet<kasa_hareket> kasa_hareket { get; set; }
        public virtual DbSet<firmalar> firmalar { get; set; }
        public virtual DbSet<toplu_borclandir> toplu_borclandir { get; set; }
        public virtual DbSet<aidat_gun> aidat_gun { get; set; }
        public virtual DbSet<vade_gun> vade_gun { get; set; }
        public virtual DbSet<yakit> yakit { get; set; }
        public virtual DbSet<borc_tipleri> borc_tipleri { get; set; }
        public virtual DbSet<sms> sms { get; set; }
        public virtual DbSet<muhtelif_baslikler> muhtelif_baslikler { get; set; }
        public virtual DbSet<muhtelif_islemler> muhtelif_islemler { get; set; }
        public virtual DbSet<dosyalar> dosyalar { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<kullanici>().ToTable("kullanici", "dbo");
            modelBuilder.Entity<blok>().ToTable("blok", "dbo");
            modelBuilder.Entity<tipler>().ToTable("tipler", "dbo");
            modelBuilder.Entity<personel>().ToTable("personel", "dbo");
            modelBuilder.Entity<anasayac>().ToTable("ana_sayac", "dbo");
            modelBuilder.Entity<sayac_tipleri>().ToTable("sayac_tipleri", "dbo");
            modelBuilder.Entity<anasayac_ortak_dagitim>().ToTable("anasayac_ortak_dagitim", "dbo");
            modelBuilder.Entity<bagimsiz_bolum_sayaclari>().ToTable("bagimsiz_bolum_sayaclari", "dbo");
            modelBuilder.Entity<bagimsiz_bolumler>().ToTable("bagimsiz_bolumler", "dbo");
            modelBuilder.Entity<kisiler>().ToTable("kisiler", "dbo");
            modelBuilder.Entity<bagimsiz_bolum_kisiler>().ToTable("bagimsiz_bolum_kisiler", "dbo");
            modelBuilder.Entity<hesap_hareket>().ToTable("hesap_hareket", "dbo");
            modelBuilder.Entity<kasa>().ToTable("kasa", "dbo");
            modelBuilder.Entity<banka>().ToTable("banka", "dbo");
            modelBuilder.Entity<banka_hareket>().ToTable("banka_hareket", "dbo");
            modelBuilder.Entity<kasa_hareket>().ToTable("kasa_hareket", "dbo");
            modelBuilder.Entity<firmalar>().ToTable("firmalar", "dbo");
            modelBuilder.Entity<toplu_borclandir>().ToTable("toplu_borclandir", "dbo");
            modelBuilder.Entity<aidat_gun>().ToTable("aidat_gun", "dbo");
            modelBuilder.Entity<vade_gun>().ToTable("vade_gun", "dbo");
            modelBuilder.Entity<yakit>().ToTable("yakit", "dbo");
            modelBuilder.Entity<borc_tipleri>().ToTable("borc_tipleri", "dbo");
            modelBuilder.Entity<sms>().ToTable("sms", "dbo");
            modelBuilder.Entity<muhtelif_baslikler>().ToTable("muhtelif_baslikler", "dbo");
            modelBuilder.Entity<muhtelif_islemler>().ToTable("muhtelif_islemler", "dbo");
            modelBuilder.Entity<dosyalar>().ToTable("dosyalar", "dbo");
            base.OnModelCreating(modelBuilder);
        }
    }
}
