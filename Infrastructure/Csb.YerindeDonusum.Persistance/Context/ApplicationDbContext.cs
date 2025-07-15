using System;
using System.Collections.Generic;
using Csb.YerindeDonusum.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Persistance.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AfadBasvuru> AfadBasvurus { get; set; }

    public virtual DbSet<AfadBasvuruTekil> AfadBasvuruTekils { get; set; }

    public virtual DbSet<AfadGirisBilgi> AfadGirisBilgis { get; set; }

    public virtual DbSet<Ayar> Ayars { get; set; }

    public virtual DbSet<AydinlatmaMetni> AydinlatmaMetnis { get; set; }

    public virtual DbSet<Basvuru> Basvurus { get; set; }

    public virtual DbSet<BasvuruAfadDurum> BasvuruAfadDurums { get; set; }

    public virtual DbSet<BasvuruDestekTur> BasvuruDestekTurs { get; set; }

    public virtual DbSet<BasvuruDosya> BasvuruDosyas { get; set; }

    public virtual DbSet<BasvuruDosyaTur> BasvuruDosyaTurs { get; set; }

    public virtual DbSet<BasvuruDurum> BasvuruDurums { get; set; }

    public virtual DbSet<BasvuruImzaVeren> BasvuruImzaVerens { get; set; }

    public virtual DbSet<BasvuruImzaVerenDosya> BasvuruImzaVerenDosyas { get; set; }

    public virtual DbSet<BasvuruImzaVerenDosyaTur> BasvuruImzaVerenDosyaTurs { get; set; }

    public virtual DbSet<BasvuruIptalTur> BasvuruIptalTurs { get; set; }

    public virtual DbSet<BasvuruKamuUstlenecek> BasvuruKamuUstleneceks { get; set; }

    public virtual DbSet<BasvuruKanal> BasvuruKanals { get; set; }

    public virtual DbSet<BasvuruTapuBilgi> BasvuruTapuBilgis { get; set; }

    public virtual DbSet<BasvuruTur> BasvuruTurs { get; set; }

    public virtual DbSet<BilgilendirmeMesaj> BilgilendirmeMesajs { get; set; }

    public virtual DbSet<BinaAdinaYapilanYardim> BinaAdinaYapilanYardims { get; set; }

    public virtual DbSet<BinaAdinaYapilanYardimTipi> BinaAdinaYapilanYardimTipis { get; set; }

    public virtual DbSet<BinaDegerlendirme> BinaDegerlendirmes { get; set; }

    public virtual DbSet<BinaDegerlendirmeDosya> BinaDegerlendirmeDosyas { get; set; }

    public virtual DbSet<BinaDegerlendirmeDosyaTur> BinaDegerlendirmeDosyaTurs { get; set; }

    public virtual DbSet<BinaDegerlendirmeDurum> BinaDegerlendirmeDurums { get; set; }

    public virtual DbSet<BinaKotUstuSayi> BinaKotUstuSayis { get; set; }

    public virtual DbSet<BinaMuteahhit> BinaMuteahhits { get; set; }

    public virtual DbSet<BinaMuteahhitTapuTur> BinaMuteahhitTapuTurs { get; set; }

    public virtual DbSet<BinaNakdiYardimTaksit> BinaNakdiYardimTaksits { get; set; }

    public virtual DbSet<BinaNakdiYardimTaksitDosya> BinaNakdiYardimTaksitDosyas { get; set; }

    public virtual DbSet<BinaOdeme> BinaOdemes { get; set; }

    public virtual DbSet<BinaOdemeDetay> BinaOdemeDetays { get; set; }

    public virtual DbSet<BinaOdemeDurum> BinaOdemeDurums { get; set; }

    public virtual DbSet<BinaYapiDenetimSeviyeTespit> BinaYapiDenetimSeviyeTespits { get; set; }

    public virtual DbSet<BinaYapiDenetimSeviyeTespitDosya> BinaYapiDenetimSeviyeTespitDosyas { get; set; }

    public virtual DbSet<BinaYapiRuhsatIzinDosya> BinaYapiRuhsatIzinDosyas { get; set; }

    public virtual DbSet<Birim> Birims { get; set; }

    public virtual DbSet<HizmetAciklama> HizmetAciklamas { get; set; }

    public virtual DbSet<IstisnaAskiKodu> IstisnaAskiKodus { get; set; }

    public virtual DbSet<Kullanici> Kullanicis { get; set; }

    public virtual DbSet<KullaniciGirisBasarili> KullaniciGirisBasarilis { get; set; }

    public virtual DbSet<KullaniciGirisHatum> KullaniciGirisHata { get; set; }

    public virtual DbSet<KullaniciGirisKod> KullaniciGirisKods { get; set; }

    public virtual DbSet<KullaniciGirisKodDeneme> KullaniciGirisKodDenemes { get; set; }

    public virtual DbSet<KullaniciHesapTip> KullaniciHesapTips { get; set; }

    public virtual DbSet<KullaniciRol> KullaniciRols { get; set; }

    public virtual DbSet<Kurum> Kurums { get; set; }

    public virtual DbSet<OfisKonum> OfisKonums { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<SikcaSorulanSoru> SikcaSorulanSorus { get; set; }

    public virtual DbSet<SmsLog> SmsLogs { get; set; }

    public virtual DbSet<TakbisIl> TakbisIls { get; set; }

    public virtual DbSet<TebligatGonderim> TebligatGonderims { get; set; }

    public virtual DbSet<TebligatGonderimDetay> TebligatGonderimDetays { get; set; }

    public virtual DbSet<TebligatGonderimDetayDosya> TebligatGonderimDetayDosyas { get; set; }

    public DbSet<IstisnaAskiKodu> IstisnaAskiKodular { get; set; }
    public DbSet<IstisnaAskiKoduDosya> IstisnaAskiKoduDosyalar { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:YerindeDonusum", x => x.UseNetTopologySuite());


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is IstisnaAskiKodu kod)
            {
                if (entry.State == EntityState.Added)
                    kod.OlusturmaTarihi = DateTime.UtcNow;
                else if (entry.State == EntityState.Modified)
                    kod.GuncellemeTarihi = DateTime.UtcNow;
            }
            else if (entry.Entity is IstisnaAskiKoduDosya dosya)
            {
                if (entry.State == EntityState.Added)
                    dosya.OlusturmaTarihi = DateTime.UtcNow;
                else if (entry.State == EntityState.Modified)
                    dosya.GuncellemeTarihi = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("uuid-ossp");
        
        modelBuilder.Entity<IstisnaAskiKodu>(entity =>
        {
            entity.ToTable("istisna_aski_kodu");
            entity.HasKey(e => e.IstisnaAskiKoduId);
            entity.HasMany(e => e.Dosyalar)
                  .WithOne(d => d.IstisnaAskiKodu)
                  .HasForeignKey(d => d.IstisnaAskiKoduId);
        });

        modelBuilder.Entity<IstisnaAskiKoduDosya>(entity =>
        {
            entity.ToTable("istisna_aski_kodu_dosya");
            entity.HasKey(e => e.IstisnaAskiKoduDosyaId);
        });

        modelBuilder.Entity<AfadBasvuru>(entity =>
        {
            entity.HasKey(e => e.CsbId).HasName("afad_basvuru_pkey");

            entity.ToTable("afad_basvuru");

            entity.HasIndex(e => e.HedefTarih, "ix_afad_basvuru_hedef_tarih");

            entity.HasIndex(e => new { e.HedefTarih, e.CsbAktifMi, e.CsbSilindiMi }, "ix_afad_basvuru_hedef_tarih_aktif_mi_silindi_mi");

            entity.HasIndex(e => new { e.CsbOlusturmaTarihi, e.CsbAktifMi, e.CsbSilindiMi, e.HedefTarih }, "ix_afad_basvuru_hedef_tarih_csb_olusturma_tarihi_aktif_silindi");

            entity.HasIndex(e => e.Huid, "ix_afad_basvuru_huid");

            entity.HasIndex(e => new { e.Tckn, e.BasvuruNo, e.CsbAktifMi, e.CsbSilindiMi }, "ix_afad_basvuru_tckn_basvuru_no_aktif_mi_silindi_mi");

            entity.Property(e => e.CsbId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("csb_id");
            entity.Property(e => e.Aciklama).HasColumnName("aciklama");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.Ada)
                .HasMaxLength(255)
                .HasColumnName("ada");
            entity.Property(e => e.AltTasinmazId).HasColumnName("alt_tasinmaz_id");
            entity.Property(e => e.AskiKodu)
                .HasMaxLength(255)
                .HasColumnName("aski_kodu");
            entity.Property(e => e.BabaAd)
                .HasMaxLength(255)
                .HasColumnName("baba_ad");
            entity.Property(e => e.BasvuruNo).HasColumnName("basvuru_no");
            entity.Property(e => e.BasvuruTipi)
                .HasMaxLength(255)
                .HasColumnName("basvuru_tipi");
            entity.Property(e => e.CsbAktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("csb_aktif_mi");
            entity.Property(e => e.CsbGuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("csb_guncelleme_tarihi");
            entity.Property(e => e.CsbOlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("csb_olusturma_tarihi");
            entity.Property(e => e.CsbRezervAlanId).HasColumnName("csb_rezerv_alan_id");
            entity.Property(e => e.CsbSilindiMi).HasColumnName("csb_silindi_mi");
            entity.Property(e => e.DegerlendirmeDurumu)
                .HasMaxLength(255)
                .HasColumnName("degerlendirme_durumu");
            entity.Property(e => e.DegerlendirmeIptalDurumu)
                .HasMaxLength(255)
                .HasColumnName("degerlendirme_iptal_durumu");
            entity.Property(e => e.EbeveynTckn).HasColumnName("ebeveyn_tckn");
            entity.Property(e => e.HasarDurumu)
                .HasMaxLength(255)
                .HasColumnName("hasar_durumu");
            entity.Property(e => e.HedefTarih).HasColumnName("hedef_tarih");
            entity.Property(e => e.Huid)
                .HasMaxLength(255)
                .HasColumnName("huid");
            entity.Property(e => e.Il)
                .HasMaxLength(255)
                .HasColumnName("il");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.Ilce)
                .HasMaxLength(255)
                .HasColumnName("ilce");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.ItirazDegerlendirmeId).HasColumnName("itiraz_degerlendirme_id");
            entity.Property(e => e.ItirazDegerlendirmeSonucu)
                .HasMaxLength(255)
                .HasColumnName("itiraz_degerlendirme_sonucu");
            entity.Property(e => e.ItirazDegerlendirmeSonucuId).HasColumnName("itiraz_degerlendirme_sonucu_id");
            entity.Property(e => e.ItirazId).HasColumnName("itiraz_id");
            entity.Property(e => e.ItirazOlusmus).HasColumnName("itiraz_olusmus");
            entity.Property(e => e.ItirazTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("itiraz_tarihi");
            entity.Property(e => e.KomisyonKararNo).HasColumnName("komisyon_karar_no");
            entity.Property(e => e.KullanimAmaci)
                .HasMaxLength(255)
                .HasColumnName("kullanim_amaci");
            entity.Property(e => e.KuraIsabetEttiMi).HasColumnName("kura_isabet_etti_mi");
            entity.Property(e => e.Mahalle)
                .HasMaxLength(255)
                .HasColumnName("mahalle");
            entity.Property(e => e.MahalleId).HasColumnName("mahalle_id");
            entity.Property(e => e.MaksYapiKimlikNo).HasColumnName("maks_yapi_kimlik_no");
            entity.Property(e => e.OlayId).HasColumnName("olay_id");
            entity.Property(e => e.OlusturulmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturulma_tarihi");
            entity.Property(e => e.Parsel)
                .HasMaxLength(255)
                .HasColumnName("parsel");
            entity.Property(e => e.Soyad)
                .HasMaxLength(255)
                .HasColumnName("soyad");
            entity.Property(e => e.TapuIl)
                .HasMaxLength(255)
                .HasColumnName("tapu_il");
            entity.Property(e => e.TapuIlce)
                .HasMaxLength(255)
                .HasColumnName("tapu_ilce");
            entity.Property(e => e.TapuMahalle)
                .HasMaxLength(255)
                .HasColumnName("tapu_mahalle");
            entity.Property(e => e.TasinmazCinsi)
                .HasMaxLength(255)
                .HasColumnName("tasinmaz_cinsi");
            entity.Property(e => e.TasinmazId).HasColumnName("tasinmaz_id");
            entity.Property(e => e.Tckn).HasColumnName("tckn");
            entity.Property(e => e.Telefon)
                .HasMaxLength(255)
                .HasColumnName("telefon");
            entity.Property(e => e.Wkt)
                .HasMaxLength(255)
                .HasColumnName("wkt");
        });

        modelBuilder.Entity<AfadBasvuruTekil>(entity =>
        {
            entity.HasKey(e => e.CsbId).HasName("afad_basvuru_tekil_pkey");

            entity.ToTable("afad_basvuru_tekil");

            entity.HasIndex(e => e.HedefTarih, "ix_afad_basvuru_tekil_hedef_tarih");

            entity.HasIndex(e => new { e.HedefTarih, e.CsbAktifMi, e.CsbSilindiMi }, "ix_afad_basvuru_tekil_hedef_tarih_aktif_mi_silindi_mi");

            entity.HasIndex(e => new { e.CsbOlusturmaTarihi, e.CsbAktifMi, e.CsbSilindiMi, e.HedefTarih }, "ix_afad_basvuru_tekil_hedef_tarih_csb_olusturma_tarihi_aktif_mi");

            entity.HasIndex(e => e.Huid, "ix_afad_basvuru_tekil_huid");

            entity.HasIndex(e => new { e.Tckn, e.BasvuruNo, e.CsbAktifMi, e.CsbSilindiMi }, "ix_afad_basvuru_tekil_tckn_basvuru_no_aktif_mi_silindi_mi");

            entity.Property(e => e.CsbId)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("csb_id");
            entity.Property(e => e.Aciklama).HasColumnName("aciklama");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.Ada)
                .HasMaxLength(255)
                .HasColumnName("ada");
            entity.Property(e => e.AltTasinmazId).HasColumnName("alt_tasinmaz_id");
            entity.Property(e => e.AskiKodu)
                .HasMaxLength(255)
                .HasColumnName("aski_kodu");
            entity.Property(e => e.BabaAd)
                .HasMaxLength(255)
                .HasColumnName("baba_ad");
            entity.Property(e => e.BasvuruNo).HasColumnName("basvuru_no");
            entity.Property(e => e.BasvuruTipi)
                .HasMaxLength(255)
                .HasColumnName("basvuru_tipi");
            entity.Property(e => e.CsbAktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("csb_aktif_mi");
            entity.Property(e => e.CsbGuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("csb_guncelleme_tarihi");
            entity.Property(e => e.CsbOlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("csb_olusturma_tarihi");
            entity.Property(e => e.CsbRezervAlanId).HasColumnName("csb_rezerv_alan_id");
            entity.Property(e => e.CsbSilindiMi).HasColumnName("csb_silindi_mi");
            entity.Property(e => e.DegerlendirmeDurumu)
                .HasMaxLength(255)
                .HasColumnName("degerlendirme_durumu");
            entity.Property(e => e.DegerlendirmeIptalDurumu)
                .HasMaxLength(255)
                .HasColumnName("degerlendirme_iptal_durumu");
            entity.Property(e => e.EbeveynTckn).HasColumnName("ebeveyn_tckn");
            entity.Property(e => e.HasarDurumu)
                .HasMaxLength(255)
                .HasColumnName("hasar_durumu");
            entity.Property(e => e.HedefTarih).HasColumnName("hedef_tarih");
            entity.Property(e => e.Huid)
                .HasMaxLength(255)
                .HasColumnName("huid");
            entity.Property(e => e.Il)
                .HasMaxLength(255)
                .HasColumnName("il");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.Ilce)
                .HasMaxLength(255)
                .HasColumnName("ilce");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.ItirazDegerlendirmeId).HasColumnName("itiraz_degerlendirme_id");
            entity.Property(e => e.ItirazDegerlendirmeSonucu)
                .HasMaxLength(255)
                .HasColumnName("itiraz_degerlendirme_sonucu");
            entity.Property(e => e.ItirazDegerlendirmeSonucuId).HasColumnName("itiraz_degerlendirme_sonucu_id");
            entity.Property(e => e.ItirazId).HasColumnName("itiraz_id");
            entity.Property(e => e.ItirazOlusmus).HasColumnName("itiraz_olusmus");
            entity.Property(e => e.ItirazTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("itiraz_tarihi");
            entity.Property(e => e.KomisyonKararNo).HasColumnName("komisyon_karar_no");
            entity.Property(e => e.KullanimAmaci)
                .HasMaxLength(255)
                .HasColumnName("kullanim_amaci");
            entity.Property(e => e.KuraIsabetEttiMi).HasColumnName("kura_isabet_etti_mi");
            entity.Property(e => e.Mahalle)
                .HasMaxLength(255)
                .HasColumnName("mahalle");
            entity.Property(e => e.MahalleId).HasColumnName("mahalle_id");
            entity.Property(e => e.MaksYapiKimlikNo).HasColumnName("maks_yapi_kimlik_no");
            entity.Property(e => e.OlayId).HasColumnName("olay_id");
            entity.Property(e => e.OlusturulmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturulma_tarihi");
            entity.Property(e => e.Parsel)
                .HasMaxLength(255)
                .HasColumnName("parsel");
            entity.Property(e => e.Soyad)
                .HasMaxLength(255)
                .HasColumnName("soyad");
            entity.Property(e => e.TapuIl)
                .HasMaxLength(255)
                .HasColumnName("tapu_il");
            entity.Property(e => e.TapuIlce)
                .HasMaxLength(255)
                .HasColumnName("tapu_ilce");
            entity.Property(e => e.TapuMahalle)
                .HasMaxLength(255)
                .HasColumnName("tapu_mahalle");
            entity.Property(e => e.TasinmazCinsi)
                .HasMaxLength(255)
                .HasColumnName("tasinmaz_cinsi");
            entity.Property(e => e.TasinmazId).HasColumnName("tasinmaz_id");
            entity.Property(e => e.Tckn).HasColumnName("tckn");
            entity.Property(e => e.Telefon)
                .HasMaxLength(255)
                .HasColumnName("telefon");
            entity.Property(e => e.Wkt)
                .HasMaxLength(255)
                .HasColumnName("wkt");
        });

        modelBuilder.Entity<AfadGirisBilgi>(entity =>
        {
            entity.HasKey(e => e.AfadGirisBilgiId).HasName("afad_giris_bilgi_pkey");

            entity.ToTable("afad_giris_bilgi");

            entity.Property(e => e.AfadGirisBilgiId).HasColumnName("afad_giris_bilgi_id");
            entity.Property(e => e.AccessToken)
                .HasMaxLength(255)
                .HasColumnName("access_token");
            entity.Property(e => e.GecerlilikSuresi)
                .HasComment("saniye")
                .HasColumnName("gecerlilik_suresi");
            entity.Property(e => e.GirisTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("giris_tarihi");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255)
                .HasColumnName("refresh_token");
            entity.Property(e => e.TokenTuru)
                .HasMaxLength(50)
                .HasColumnName("token_turu");
        });

        modelBuilder.Entity<Ayar>(entity =>
        {
            entity.HasKey(e => e.AyarId).HasName("ayar_pkey");

            entity.ToTable("ayar");

            entity.Property(e => e.AyarId).HasColumnName("ayar_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.Deger).HasColumnName("deger");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<AydinlatmaMetni>(entity =>
        {
            entity.HasKey(e => e.AydinlatmaMetniId).HasName("aydinlatma_metni_pkey");

            entity.ToTable("aydinlatma_metni");

            entity.Property(e => e.AydinlatmaMetniId).HasColumnName("aydinlatma_metni_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.AydinlatmaMetniGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("aydinlatma_metni_guid");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.Icerik).HasColumnName("icerik");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<Basvuru>(entity =>
        {
            entity.HasKey(e => e.BasvuruId).HasName("basvuru_pkey");

            entity.ToTable("basvuru");

            entity.Property(e => e.BasvuruId).HasColumnName("basvuru_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.AydinlatmaMetniId).HasColumnName("aydinlatma_metni_id");
            entity.Property(e => e.BasvuruAfadDurumGuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("basvuru_afad_durum_guncelleme_tarihi");
            entity.Property(e => e.BasvuruAfadDurumId).HasColumnName("basvuru_afad_durum_id");
            entity.Property(e => e.BasvuruDestekTurId).HasColumnName("basvuru_destek_tur_id");
            entity.Property(e => e.BasvuruDurumId).HasColumnName("basvuru_durum_id");
            entity.Property(e => e.BasvuruGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("basvuru_guid");
            entity.Property(e => e.BasvuruIptalAciklamasi)
                .HasMaxLength(2000)
                .HasColumnName("basvuru_iptal_aciklamasi");
            entity.Property(e => e.BasvuruIptalTurId).HasColumnName("basvuru_iptal_tur_id");
            entity.Property(e => e.BasvuruKamuUstlenecekGuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("basvuru_kamu_ustlenecek_guncelleme_tarihi");
            entity.Property(e => e.BasvuruKanalId).HasColumnName("basvuru_kanal_id");
            entity.Property(e => e.BasvuruKodu)
                .HasMaxLength(13)
                .HasColumnName("basvuru_kodu");
            entity.Property(e => e.BasvuruTurId).HasColumnName("basvuru_tur_id");
            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.CepTelefonu)
                .HasMaxLength(20)
                .HasColumnName("cep_telefonu");
            entity.Property(e => e.Eposta)
                .HasMaxLength(255)
                .HasColumnName("eposta");
            entity.Property(e => e.EskiTapuAda)
                .HasMaxLength(10)
                .HasColumnName("eski_tapu_ada");
            entity.Property(e => e.EskiTapuGuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("eski_tapu_guncelleme_tarihi");
            entity.Property(e => e.EskiTapuGuncelleyenKullaniciId).HasColumnName("eski_tapu_guncelleyen_kullanici_id");
            entity.Property(e => e.EskiTapuIlceAdi)
                .HasMaxLength(75)
                .HasColumnName("eski_tapu_ilce_adi");
            entity.Property(e => e.EskiTapuIlceId).HasColumnName("eski_tapu_ilce_id");
            entity.Property(e => e.EskiTapuMahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("eski_tapu_mahalle_adi");
            entity.Property(e => e.EskiTapuMahalleId).HasColumnName("eski_tapu_mahalle_id");
            entity.Property(e => e.EskiTapuParsel)
                .HasMaxLength(10)
                .HasColumnName("eski_tapu_parsel");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.HasarTespitAda)
                .HasMaxLength(10)
                .HasColumnName("hasar_tespit_ada");
            entity.Property(e => e.HasarTespitAskiKodu)
                .HasMaxLength(100)
                .HasColumnName("hasar_tespit_aski_kodu");
            entity.Property(e => e.HasarTespitDisKapiNo).HasColumnName("hasar_tespit_dis_kapi_no");
            entity.Property(e => e.HasarTespitGuclendirmeMahkemeSonucu)
                .HasMaxLength(255)
                .HasColumnName("hasar_tespit_guclendirme_mahkeme_sonucu");
            entity.Property(e => e.HasarTespitHasarDurumu)
                .HasMaxLength(100)
                .HasColumnName("hasar_tespit_hasar_durumu");
            entity.Property(e => e.HasarTespitIlAdi)
                .HasMaxLength(75)
                .HasColumnName("hasar_tespit_il_adi");
            entity.Property(e => e.HasarTespitIlceAdi)
                .HasMaxLength(100)
                .HasColumnName("hasar_tespit_ilce_adi");
            entity.Property(e => e.HasarTespitItirazSonucu)
                .HasMaxLength(100)
                .HasColumnName("hasar_tespit_itiraz_sonucu");
            entity.Property(e => e.HasarTespitMahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("hasar_tespit_mahalle_adi");
            entity.Property(e => e.HasarTespitParsel)
                .HasMaxLength(10)
                .HasColumnName("hasar_tespit_parsel");
            entity.Property(e => e.HasarTespitUid)
                .HasMaxLength(64)
                .HasColumnName("hasar_tespit_uid");
            entity.Property(e => e.Huid)
                .HasMaxLength(64)
                .HasColumnName("huid");
            entity.Property(e => e.HuidKontrolTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("huid_kontrol_tarihi");
            entity.Property(e => e.KacakYapiMi).HasColumnName("kacak_yapi_mi");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.SonuclandirmaAciklamasi)
                .HasMaxLength(1000)
                .HasColumnName("sonuclandirma_aciklamasi");
            entity.Property(e => e.Soyad)
                .HasMaxLength(255)
                .HasColumnName("soyad");
            entity.Property(e => e.TapuAda)
                .HasMaxLength(10)
                .HasColumnName("tapu_ada");
            entity.Property(e => e.TapuAnaTasinmazId).HasColumnName("tapu_ana_tasinmaz_id");
            entity.Property(e => e.TapuArsaPay).HasColumnName("tapu_arsa_pay");
            entity.Property(e => e.TapuArsaPayda).HasColumnName("tapu_arsa_payda");
            entity.Property(e => e.TapuBagimsizBolumNo)
                .HasMaxLength(20)
                .HasColumnName("tapu_bagimsiz_bolum_no");
            entity.Property(e => e.TapuBeyanAciklama).HasColumnName("tapu_beyan_aciklama");
            entity.Property(e => e.TapuBlok)
                .HasMaxLength(15)
                .HasColumnName("tapu_blok");
            entity.Property(e => e.TapuGirisBilgisi)
                .HasMaxLength(20)
                .HasColumnName("tapu_giris_bilgisi");
            entity.Property(e => e.TapuHazineArazisiMi).HasColumnName("tapu_hazine_arazisi_mi");
            entity.Property(e => e.TapuIlAdi)
                .HasMaxLength(75)
                .HasColumnName("tapu_il_adi");
            entity.Property(e => e.TapuIlId).HasColumnName("tapu_il_id");
            entity.Property(e => e.TapuIlceAdi)
                .HasMaxLength(75)
                .HasColumnName("tapu_ilce_adi");
            entity.Property(e => e.TapuIlceId).HasColumnName("tapu_ilce_id");
            entity.Property(e => e.TapuIstirakNo).HasColumnName("tapu_istirak_no");
            entity.Property(e => e.TapuKat).HasColumnName("tapu_kat");
            entity.Property(e => e.TapuMahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("tapu_mahalle_adi");
            entity.Property(e => e.TapuMahalleId).HasColumnName("tapu_mahalle_id");
            entity.Property(e => e.TapuNitelik).HasColumnName("tapu_nitelik");
            entity.Property(e => e.TapuParsel)
                .HasMaxLength(10)
                .HasColumnName("tapu_parsel");
            entity.Property(e => e.TapuRehinDurumu)
                .HasMaxLength(500)
                .HasColumnName("tapu_rehin_durumu");
            entity.Property(e => e.TapuTasinmazId).HasColumnName("tapu_tasinmaz_id");
            entity.Property(e => e.TapuTasinmazTipi)
                .HasMaxLength(100)
                .HasColumnName("tapu_tasinmaz_tipi");
            entity.Property(e => e.TapuToplamBagimsizBolumSayisi).HasColumnName("tapu_toplam_bagimsiz_bolum_sayisi");
            entity.Property(e => e.TapuToplamKisiBagimsizBolumSayisi).HasColumnName("tapu_toplam_kisi_bagimsiz_bolum_sayisi");
            entity.Property(e => e.TapuToplamKisiHisseOrani)
                .HasPrecision(18, 15)
                .HasColumnName("tapu_toplam_kisi_hisse_orani");
            entity.Property(e => e.TcKimlikNo)
                .HasMaxLength(11)
                .HasColumnName("tc_kimlik_no");
            entity.Property(e => e.TuzelKisiAdi)
                .HasMaxLength(500)
                .HasColumnName("tuzel_kisi_adi");
            entity.Property(e => e.TuzelKisiAdres)
                .HasMaxLength(1000)
                .HasColumnName("tuzel_kisi_adres");
            entity.Property(e => e.TuzelKisiMersisNo)
                .HasMaxLength(50)
                .HasColumnName("tuzel_kisi_mersis_no");
            entity.Property(e => e.TuzelKisiTipId).HasColumnName("tuzel_kisi_tip_id");
            entity.Property(e => e.TuzelKisiVergiNo)
                .HasMaxLength(15)
                .HasColumnName("tuzel_kisi_vergi_no");
            entity.Property(e => e.TuzelKisiYetkiTuru)
                .HasMaxLength(100)
                .HasColumnName("tuzel_kisi_yetki_turu");
            entity.Property(e => e.UavtAcikAdres)
                .HasMaxLength(1000)
                .HasColumnName("uavt_acik_adres");
            entity.Property(e => e.UavtAdresNo)
                .HasMaxLength(255)
                .HasColumnName("uavt_adres_no");
            entity.Property(e => e.UavtBeyanMi).HasColumnName("uavt_beyan_mi");
            entity.Property(e => e.UavtBinaAda)
                .HasMaxLength(255)
                .HasColumnName("uavt_bina_ada");
            entity.Property(e => e.UavtBinaBlokAdi)
                .HasMaxLength(255)
                .HasColumnName("uavt_bina_blok_adi");
            entity.Property(e => e.UavtBinaKodu)
                .HasMaxLength(255)
                .HasColumnName("uavt_bina_kodu");
            entity.Property(e => e.UavtBinaNo)
                .HasMaxLength(255)
                .HasColumnName("uavt_bina_no");
            entity.Property(e => e.UavtBinaPafta)
                .HasMaxLength(255)
                .HasColumnName("uavt_bina_pafta");
            entity.Property(e => e.UavtBinaParsel)
                .HasMaxLength(255)
                .HasColumnName("uavt_bina_parsel");
            entity.Property(e => e.UavtBinaSiteAdi)
                .HasMaxLength(255)
                .HasColumnName("uavt_bina_site_adi");
            entity.Property(e => e.UavtCaddeNo).HasColumnName("uavt_cadde_no");
            entity.Property(e => e.UavtCsbm)
                .HasMaxLength(255)
                .HasColumnName("uavt_csbm");
            entity.Property(e => e.UavtCsbmKodu)
                .HasMaxLength(255)
                .HasColumnName("uavt_csbm_kodu");
            entity.Property(e => e.UavtDisKapiNo)
                .HasMaxLength(255)
                .HasColumnName("uavt_dis_kapi_no");
            entity.Property(e => e.UavtIcKapiNo)
                .HasMaxLength(255)
                .HasColumnName("uavt_ic_kapi_no");
            entity.Property(e => e.UavtIlAdi)
                .HasMaxLength(75)
                .HasColumnName("uavt_il_adi");
            entity.Property(e => e.UavtIlKodu)
                .HasMaxLength(75)
                .HasColumnName("uavt_il_kodu");
            entity.Property(e => e.UavtIlNo).HasColumnName("uavt_il_no");
            entity.Property(e => e.UavtIlceAdi)
                .HasMaxLength(100)
                .HasColumnName("uavt_ilce_adi");
            entity.Property(e => e.UavtIlceKodu)
                .HasMaxLength(75)
                .HasColumnName("uavt_ilce_kodu");
            entity.Property(e => e.UavtIlceNo).HasColumnName("uavt_ilce_no");
            entity.Property(e => e.UavtMahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("uavt_mahalle_adi");
            entity.Property(e => e.UavtMahalleKodu)
                .HasMaxLength(75)
                .HasColumnName("uavt_mahalle_kodu");
            entity.Property(e => e.UavtMahalleNo).HasColumnName("uavt_mahalle_no");
            entity.Property(e => e.UavtMeskenBinaNo).HasColumnName("uavt_mesken_bina_no");
            entity.Property(e => e.UavtNitelik)
                .HasMaxLength(255)
                .HasColumnName("uavt_nitelik");
            entity.Property(e => e.VatandaslikDurumu)
                .HasComment("1: tc vatandaşı, 2: yabancı")
                .HasColumnName("vatandaslik_durumu");

            entity.HasOne(d => d.AydinlatmaMetni).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.AydinlatmaMetniId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_aydinlatma_metni_id");

            entity.HasOne(d => d.BasvuruAfadDurum).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.BasvuruAfadDurumId)
                .HasConstraintName("fk_basvuru_basvuru_afad_durum");

            entity.HasOne(d => d.BasvuruDestekTur).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.BasvuruDestekTurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_basvuru_destek_tur_id");

            entity.HasOne(d => d.BasvuruDurum).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.BasvuruDurumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_basvuru_durum_id");

            entity.HasOne(d => d.BasvuruIptalTur).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.BasvuruIptalTurId)
                .HasConstraintName("fk_basvuru_basvuru_iptal_tur_id");

            entity.HasOne(d => d.BasvuruKanal).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.BasvuruKanalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_basvuru_kanal_id");

            entity.HasOne(d => d.BasvuruTur).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.BasvuruTurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_basvuru_tur_id");

            entity.HasOne(d => d.BinaDegerlendirme).WithMany(p => p.Basvurus)
                .HasForeignKey(d => d.BinaDegerlendirmeId)
                .HasConstraintName("fk_basvuru_bina_degerlendirme_id");
        });

        modelBuilder.Entity<BasvuruAfadDurum>(entity =>
        {
            entity.HasKey(e => e.BasvuruAfadDurumId).HasName("basvuru_afad_durum_pkey");

            entity.ToTable("basvuru_afad_durum");

            entity.Property(e => e.BasvuruAfadDurumId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("basvuru_afad_durum_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.SilindiMi)
                .HasDefaultValueSql("false")
                .HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BasvuruDestekTur>(entity =>
        {
            entity.HasKey(e => e.BasvuruDestekTurId).HasName("basvuru_destek_tur_pkey");

            entity.ToTable("basvuru_destek_tur");

            entity.Property(e => e.BasvuruDestekTurId).HasColumnName("basvuru_destek_tur_id");
            entity.Property(e => e.Aciklama).HasColumnName("aciklama");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BasvuruDestekTurGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("basvuru_destek_tur_guid");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BasvuruDosya>(entity =>
        {
            entity.HasKey(e => e.BasvuruDosyaId).HasName("basvuru_dosya_pkey");

            entity.ToTable("basvuru_dosya");

            entity.Property(e => e.BasvuruDosyaId).HasColumnName("basvuru_dosya_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BasvuruDosyaGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("basvuru_dosya_guid");
            entity.Property(e => e.BasvuruDosyaTurId).HasColumnName("basvuru_dosya_tur_id");
            entity.Property(e => e.BasvuruId).HasColumnName("basvuru_id");
            entity.Property(e => e.DosyaAdi)
                .HasMaxLength(255)
                .HasColumnName("dosya_adi");
            entity.Property(e => e.DosyaTuru)
                .HasMaxLength(255)
                .HasColumnName("dosya_turu");
            entity.Property(e => e.DosyaYolu)
                .HasMaxLength(500)
                .HasColumnName("dosya_yolu");
            entity.Property(e => e.OlusturanIp)
                .HasColumnType("character varying")
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.BasvuruDosyaTur).WithMany(p => p.BasvuruDosyas)
                .HasForeignKey(d => d.BasvuruDosyaTurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_dosya_basvuru_dosya_tur_id");

            entity.HasOne(d => d.Basvuru).WithMany(p => p.BasvuruDosyas)
                .HasForeignKey(d => d.BasvuruId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_dosya_basvuru_id");
        });

        modelBuilder.Entity<BasvuruDosyaTur>(entity =>
        {
            entity.HasKey(e => e.BasvuruDosyaTurId).HasName("basvuru_dosya_tur_pkey");

            entity.ToTable("basvuru_dosya_tur");

            entity.Property(e => e.BasvuruDosyaTurId).HasColumnName("basvuru_dosya_tur_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(100)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BasvuruDosyaTurGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("basvuru_dosya_tur_guid");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BasvuruDurum>(entity =>
        {
            entity.HasKey(e => e.BasvuruDurumId).HasName("basvuru_durum_pkey");

            entity.ToTable("basvuru_durum");

            entity.Property(e => e.BasvuruDurumId).HasColumnName("basvuru_durum_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BasvuruDurumGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("basvuru_durum_guid");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BasvuruImzaVeren>(entity =>
        {
            entity.HasKey(e => e.BasvuruImzaVerenId).HasName("basvuru_imza_veren_pkey");

            entity.ToTable("basvuru_imza_veren");

            entity.HasIndex(e => new { e.BasvuruId, e.AktifMi, e.SilindiMi }, "ix_basvuru_imza_veren_basvuruid");

            entity.Property(e => e.BasvuruImzaVerenId).HasColumnName("basvuru_imza_veren_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BagimsizBolumAlani)
                .HasComment("metre kare")
                .HasColumnName("bagimsiz_bolum_alani");
            entity.Property(e => e.BagimsizBolumNo)
                .HasMaxLength(255)
                .HasColumnName("bagimsiz_bolum_no");
            entity.Property(e => e.BasvuruDestekTurId).HasColumnName("basvuru_destek_tur_id");
            entity.Property(e => e.BasvuruId).HasColumnName("basvuru_id");
            entity.Property(e => e.BasvuruKamuUstlenecekId).HasColumnName("basvuru_kamu_ustlenecek_id");
            entity.Property(e => e.BasvuruTurId).HasColumnName("basvuru_tur_id");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.HibeOdemeTutar).HasColumnName("hibe_odeme_tutar");
            entity.Property(e => e.HissePay).HasColumnName("hisse_pay");
            entity.Property(e => e.HissePayda).HasColumnName("hisse_payda");
            entity.Property(e => e.IbanGirildiMi).HasColumnName("iban_girildi_mi");
            entity.Property(e => e.IbanNo)
                .HasMaxLength(50)
                .HasColumnName("iban_no");
            entity.Property(e => e.KrediOdemeTutar).HasColumnName("kredi_odeme_tutar");
            entity.Property(e => e.NotBilgi)
                .HasMaxLength(1000)
                .HasColumnName("not_bilgi");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.SozlesmeTarihi).HasColumnName("sozlesme_tarihi");

            entity.HasOne(d => d.BasvuruDestekTur).WithMany(p => p.BasvuruImzaVerens)
                .HasForeignKey(d => d.BasvuruDestekTurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_imza_veren_basvuru_destek_tur_id");

            entity.HasOne(d => d.Basvuru).WithMany(p => p.BasvuruImzaVerens)
                .HasForeignKey(d => d.BasvuruId)
                .HasConstraintName("fk_basvuru_imza_veren_basvuru_id");

            entity.HasOne(d => d.BasvuruKamuUstlenecek).WithMany(p => p.BasvuruImzaVerens)
                .HasForeignKey(d => d.BasvuruKamuUstlenecekId)
                .HasConstraintName("fk_basvuru_imza_veren_basvuru_kamu_ustlenecek_id");

            entity.HasOne(d => d.BasvuruTur).WithMany(p => p.BasvuruImzaVerens)
                .HasForeignKey(d => d.BasvuruTurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_imza_veren_basvuru_tur_id");
        });

        modelBuilder.Entity<BasvuruImzaVerenDosya>(entity =>
        {
            entity.HasKey(e => e.BasvuruImzaVerenDosyaId).HasName("basvuru_imza_veren_dosya_pkey");

            entity.ToTable("basvuru_imza_veren_dosya");

            entity.Property(e => e.BasvuruImzaVerenDosyaId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("basvuru_imza_veren_dosya_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BasvuruImzaVerenDosyaGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("basvuru_imza_veren_dosya_guid");
            entity.Property(e => e.BasvuruImzaVerenDosyaTurId).HasColumnName("basvuru_imza_veren_dosya_tur_id");
            entity.Property(e => e.BasvuruImzaVerenId).HasColumnName("basvuru_imza_veren_id");
            entity.Property(e => e.DosyaAdi)
                .HasMaxLength(255)
                .HasColumnName("dosya_adi");
            entity.Property(e => e.DosyaTuru)
                .HasMaxLength(255)
                .HasColumnName("dosya_turu");
            entity.Property(e => e.DosyaYolu)
                .HasMaxLength(500)
                .HasColumnName("dosya_yolu");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasColumnType("character varying")
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasColumnType("character varying")
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.BasvuruImzaVerenDosyaTur).WithMany(p => p.BasvuruImzaVerenDosyas)
                .HasForeignKey(d => d.BasvuruImzaVerenDosyaTurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_imza_veren_dosya_basvuru_imza_veren_dosya_tur_id");

            entity.HasOne(d => d.BasvuruImzaVeren).WithMany(p => p.BasvuruImzaVerenDosyas)
                .HasForeignKey(d => d.BasvuruImzaVerenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_imza_veren_dosya_basvuru_imza_veren_id");
        });

        modelBuilder.Entity<BasvuruImzaVerenDosyaTur>(entity =>
        {
            entity.HasKey(e => e.BasvuruImzaVerenDosyaTurId).HasName("basvuru_imza_veren_dosya_tur_pkey");

            entity.ToTable("basvuru_imza_veren_dosya_tur");

            entity.Property(e => e.BasvuruImzaVerenDosyaTurId).HasColumnName("basvuru_imza_veren_dosya_tur_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(100)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BasvuruIptalTur>(entity =>
        {
            entity.HasKey(e => e.BasvuruIptalTurId).HasName("basvuru_iptal_tur_pkey");

            entity.ToTable("basvuru_iptal_tur");

            entity.Property(e => e.BasvuruIptalTurId).HasColumnName("basvuru_iptal_tur_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(2000)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BasvuruKamuUstlenecek>(entity =>
        {
            entity.HasKey(e => e.BasvuruKamuUstlenecekId).HasName("basvuru_kamu_ustlenecek_pkey");

            entity.ToTable("basvuru_kamu_ustlenecek");

            entity.Property(e => e.BasvuruKamuUstlenecekId).HasColumnName("basvuru_kamu_ustlenecek_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BasvuruAfadDurumGuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("basvuru_afad_durum_guncelleme_tarihi");
            entity.Property(e => e.BasvuruAfadDurumId).HasColumnName("basvuru_afad_durum_id");
            entity.Property(e => e.BasvuruDestekTurId).HasColumnName("basvuru_destek_tur_id");
            entity.Property(e => e.BasvuruDurumId).HasColumnName("basvuru_durum_id");
            entity.Property(e => e.BasvuruIptalTurId).HasColumnName("basvuru_iptal_tur_id");
            entity.Property(e => e.BasvuruKamuUstlenecekGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("basvuru_kamu_ustlenecek_guid");
            entity.Property(e => e.BasvuruTurId).HasColumnName("basvuru_tur_id");
            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.CepTelefonu)
                .HasMaxLength(20)
                .HasColumnName("cep_telefonu");
            entity.Property(e => e.Eposta)
                .HasMaxLength(255)
                .HasColumnName("eposta");
            entity.Property(e => e.EskiTapuAda)
                .HasMaxLength(10)
                .HasColumnName("eski_tapu_ada");
            entity.Property(e => e.EskiTapuGuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("eski_tapu_guncelleme_tarihi");
            entity.Property(e => e.EskiTapuGuncelleyenKullaniciId).HasColumnName("eski_tapu_guncelleyen_kullanici_id");
            entity.Property(e => e.EskiTapuIlceAdi)
                .HasMaxLength(75)
                .HasColumnName("eski_tapu_ilce_adi");
            entity.Property(e => e.EskiTapuIlceId).HasColumnName("eski_tapu_ilce_id");
            entity.Property(e => e.EskiTapuMahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("eski_tapu_mahalle_adi");
            entity.Property(e => e.EskiTapuMahalleId).HasColumnName("eski_tapu_mahalle_id");
            entity.Property(e => e.EskiTapuParsel)
                .HasMaxLength(10)
                .HasColumnName("eski_tapu_parsel");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.PasifMaliyeHazinesiMi).HasColumnName("pasif_maliye_hazinesi_mi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.SonuclandirmaAciklamasi)
                .HasMaxLength(1000)
                .HasColumnName("sonuclandirma_aciklamasi");
            entity.Property(e => e.Soyad)
                .HasMaxLength(255)
                .HasColumnName("soyad");
            entity.Property(e => e.TapuAda)
                .HasMaxLength(10)
                .HasColumnName("tapu_ada");
            entity.Property(e => e.TapuAnaTasinmazId).HasColumnName("tapu_ana_tasinmaz_id");
            entity.Property(e => e.TapuArsaPay).HasColumnName("tapu_arsa_pay");
            entity.Property(e => e.TapuArsaPayda).HasColumnName("tapu_arsa_payda");
            entity.Property(e => e.TapuBagimsizBolumNo)
                .HasMaxLength(20)
                .HasColumnName("tapu_bagimsiz_bolum_no");
            entity.Property(e => e.TapuBlok)
                .HasMaxLength(15)
                .HasColumnName("tapu_blok");
            entity.Property(e => e.TapuGirisBilgisi)
                .HasMaxLength(20)
                .HasColumnName("tapu_giris_bilgisi");
            entity.Property(e => e.TapuIlAdi)
                .HasMaxLength(75)
                .HasColumnName("tapu_il_adi");
            entity.Property(e => e.TapuIlId).HasColumnName("tapu_il_id");
            entity.Property(e => e.TapuIlceAdi)
                .HasMaxLength(75)
                .HasColumnName("tapu_ilce_adi");
            entity.Property(e => e.TapuIlceId).HasColumnName("tapu_ilce_id");
            entity.Property(e => e.TapuIstirakNo).HasColumnName("tapu_istirak_no");
            entity.Property(e => e.TapuKat).HasColumnName("tapu_kat");
            entity.Property(e => e.TapuMahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("tapu_mahalle_adi");
            entity.Property(e => e.TapuMahalleId).HasColumnName("tapu_mahalle_id");
            entity.Property(e => e.TapuNitelik).HasColumnName("tapu_nitelik");
            entity.Property(e => e.TapuParsel)
                .HasMaxLength(10)
                .HasColumnName("tapu_parsel");
            entity.Property(e => e.TapuRehinDurumu)
                .HasMaxLength(500)
                .HasColumnName("tapu_rehin_durumu");
            entity.Property(e => e.TapuTasinmazId).HasColumnName("tapu_tasinmaz_id");
            entity.Property(e => e.TapuTasinmazTipi)
                .HasMaxLength(100)
                .HasColumnName("tapu_tasinmaz_tipi");
            entity.Property(e => e.TcKimlikNo)
                .HasMaxLength(11)
                .HasColumnName("tc_kimlik_no");
            entity.Property(e => e.TuzelKisiAdi)
                .HasMaxLength(500)
                .HasColumnName("tuzel_kisi_adi");
            entity.Property(e => e.TuzelKisiAdres)
                .HasMaxLength(1000)
                .HasColumnName("tuzel_kisi_adres");
            entity.Property(e => e.TuzelKisiMersisNo)
                .HasMaxLength(50)
                .HasColumnName("tuzel_kisi_mersis_no");
            entity.Property(e => e.TuzelKisiTipId).HasColumnName("tuzel_kisi_tip_id");
            entity.Property(e => e.TuzelKisiVergiNo)
                .HasMaxLength(15)
                .HasColumnName("tuzel_kisi_vergi_no");
            entity.Property(e => e.TuzelKisiYetkiTuru)
                .HasMaxLength(100)
                .HasColumnName("tuzel_kisi_yetki_turu");
            entity.Property(e => e.UavtIlAdi)
                .HasMaxLength(75)
                .HasColumnName("uavt_il_adi");
            entity.Property(e => e.UavtIlKodu)
                .HasMaxLength(75)
                .HasColumnName("uavt_il_kodu");
            entity.Property(e => e.UavtIlNo).HasColumnName("uavt_il_no");
            entity.Property(e => e.UavtIlceAdi)
                .HasMaxLength(100)
                .HasColumnName("uavt_ilce_adi");
            entity.Property(e => e.UavtIlceKodu)
                .HasMaxLength(75)
                .HasColumnName("uavt_ilce_kodu");
            entity.Property(e => e.UavtIlceNo).HasColumnName("uavt_ilce_no");
            entity.Property(e => e.UavtMahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("uavt_mahalle_adi");
            entity.Property(e => e.UavtMahalleKodu)
                .HasMaxLength(75)
                .HasColumnName("uavt_mahalle_kodu");
            entity.Property(e => e.UavtMahalleNo).HasColumnName("uavt_mahalle_no");

            entity.HasOne(d => d.BasvuruAfadDurum).WithMany(p => p.BasvuruKamuUstleneceks)
                .HasForeignKey(d => d.BasvuruAfadDurumId)
                .HasConstraintName("fk_basvuru_kamu_ustlenecek_basvuru_afad_durum");

            entity.HasOne(d => d.BasvuruDestekTur).WithMany(p => p.BasvuruKamuUstleneceks)
                .HasForeignKey(d => d.BasvuruDestekTurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_kamu_ustlenecek_basvuru_destek_tur_id");

            entity.HasOne(d => d.BasvuruDurum).WithMany(p => p.BasvuruKamuUstleneceks)
                .HasForeignKey(d => d.BasvuruDurumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_kamu_ustlenecek_basvuru_durum_id");

            entity.HasOne(d => d.BasvuruTur).WithMany(p => p.BasvuruKamuUstleneceks)
                .HasForeignKey(d => d.BasvuruTurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_kamu_ustlenecek_basvuru_tur_id");

            entity.HasOne(d => d.BinaDegerlendirme).WithMany(p => p.BasvuruKamuUstleneceks)
                .HasForeignKey(d => d.BinaDegerlendirmeId)
                .HasConstraintName("fk_basvuru_kamu_ustlenecek_bina_degerlendirme_id");
        });

        modelBuilder.Entity<BasvuruKanal>(entity =>
        {
            entity.HasKey(e => e.BasvuruKanalId).HasName("basvuru_kanal_pkey");

            entity.ToTable("basvuru_kanal");

            entity.Property(e => e.BasvuruKanalId).HasColumnName("basvuru_kanal_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BasvuruKanalGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("basvuru_kanal_guid");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BasvuruTapuBilgi>(entity =>
        {
            entity.HasKey(e => e.BasvuruTapuBilgiId).HasName("basvuru_tapu_bilgi_pkey");

            entity.ToTable("basvuru_tapu_bilgi");

            entity.Property(e => e.BasvuruTapuBilgiId).HasColumnName("basvuru_tapu_bilgi_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BasvuruId).HasColumnName("basvuru_id");
            entity.Property(e => e.HissePay).HasColumnName("hisse_pay");
            entity.Property(e => e.HissePayda).HasColumnName("hisse_payda");
            entity.Property(e => e.HisseTuru)
                .HasMaxLength(100)
                .HasComment("iştiraklı vb.")
                .HasColumnName("hisse_turu");
            entity.Property(e => e.IslemTanim)
                .HasMaxLength(255)
                .HasColumnName("islem_tanim");
            entity.Property(e => e.IstirakNo).HasColumnName("istirak_no");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.TapuMudurlugu)
                .HasMaxLength(255)
                .HasColumnName("tapu_mudurlugu");
            entity.Property(e => e.YevmiheTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("yevmihe_tarihi");
            entity.Property(e => e.YevmiyeNo).HasColumnName("yevmiye_no");

            entity.HasOne(d => d.Basvuru).WithMany(p => p.BasvuruTapuBilgis)
                .HasForeignKey(d => d.BasvuruId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_basvuru_tapu_bilgi_basvuru_id");
        });

        modelBuilder.Entity<BasvuruTur>(entity =>
        {
            entity.HasKey(e => e.BasvuruTurId).HasName("basvuru_tur_pkey");

            entity.ToTable("basvuru_tur");

            entity.Property(e => e.BasvuruTurId).HasColumnName("basvuru_tur_id");
            entity.Property(e => e.Aciklama).HasColumnName("aciklama");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BasvuruTurGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("basvuru_tur_guid");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BilgilendirmeMesaj>(entity =>
        {
            entity.HasKey(e => e.BilgilendirmeMesajId).HasName("bilgilendirme_mesaj_pkey");

            entity.ToTable("bilgilendirme_mesaj");

            entity.Property(e => e.BilgilendirmeMesajId).HasColumnName("bilgilendirme_mesaj_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.Anahtar)
                .HasMaxLength(500)
                .HasColumnName("anahtar");
            entity.Property(e => e.Deger).HasColumnName("deger");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BinaAdinaYapilanYardim>(entity =>
        {
            entity.HasKey(e => e.BinaAdinaYapilanYardimId).HasName("bina_adina_yapilan_yardim_pkey");

            entity.ToTable("bina_adina_yapilan_yardim");

            entity.Property(e => e.BinaAdinaYapilanYardimId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_adina_yapilan_yardim_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaAdinaYapilanYardimTipiId).HasColumnName("bina_adina_yapilan_yardim_tipi_id");
            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.Tarih)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("tarih");
            entity.Property(e => e.Tutar).HasColumnName("tutar");

            entity.HasOne(d => d.BinaAdinaYapilanYardimTipi).WithMany(p => p.BinaAdinaYapilanYardims)
                .HasForeignKey(d => d.BinaAdinaYapilanYardimTipiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_adina_yapilan_yardim_bina_adina_yapilan_yardim_tipi");

            entity.HasOne(d => d.BinaDegerlendirme).WithMany(p => p.BinaAdinaYapilanYardims)
                .HasForeignKey(d => d.BinaDegerlendirmeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_adina_yapilan_yardim_bina_degerlendirme_id");
        });

        modelBuilder.Entity<BinaAdinaYapilanYardimTipi>(entity =>
        {
            entity.HasKey(e => e.BinaAdinaYapilanYardimTipiId).HasName("bina_adina_yapilan_yardim_tipi_pkey");

            entity.ToTable("bina_adina_yapilan_yardim_tipi");

            entity.Property(e => e.BinaAdinaYapilanYardimTipiId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_adina_yapilan_yardim_tipi_id");
            entity.Property(e => e.Adi)
                .HasMaxLength(255)
                .HasColumnName("adi");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BinaDegerlendirme>(entity =>
        {
            entity.HasKey(e => e.BinaDegerlendirmeId).HasName("bina_degerlendirme_pkey");

            entity.ToTable("bina_degerlendirme");

            entity.HasIndex(e => new { e.AktifMi, e.SilindiMi }, "ix_bina_degerlendirme_aktif_silindi_mi");

            entity.HasIndex(e => new { e.UavtMahalleNo, e.BinaDegerlendirmeDurumId, e.BultenNo, e.IzinBelgesiSayi, e.AktifMi, e.SilindiMi }, "ix_bina_degerlendirme_uavtmahalleno_durum_bulten_izinbelgesayi");

            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.Ada)
                .HasMaxLength(10)
                .HasColumnName("ada");
            entity.Property(e => e.AdaParselGuncellemeTipiId).HasColumnName("ada_parsel_guncelleme_tipi_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BagimsizBolumSayisi).HasColumnName("bagimsiz_bolum_sayisi");
            entity.Property(e => e.BinaDegerlendirmeDurumId).HasColumnName("bina_degerlendirme_durum_id");
            entity.Property(e => e.BinaDisKapiNo)
                .HasMaxLength(255)
                .HasColumnName("bina_dis_kapi_no");
            entity.Property(e => e.BultenNo).HasColumnName("bulten_no");
            entity.Property(e => e.FenniMesulTc)
                .HasMaxLength(11)
                .HasColumnName("fenni_mesul_tc");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.HasarTespitAskiKodu)
                .HasMaxLength(500)
                .HasColumnName("hasar_tespit_aski_kodu");
            entity.Property(e => e.ImzalayanKisiSayisi).HasColumnName("imzalayan_kisi_sayisi");
            entity.Property(e => e.IzinBelgesiSayi).HasColumnName("izin_belgesi_sayi");
            entity.Property(e => e.IzinBelgesiTarih)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("izin_belgesi_tarih");
            entity.Property(e => e.KotAltKatSayisi).HasColumnName("kot_alt_kat_sayisi");
            entity.Property(e => e.KotUstKatSayisi).HasColumnName("kot_ust_kat_sayisi");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.Parsel)
                .HasMaxLength(10)
                .HasColumnName("parsel");
            entity.Property(e => e.RuhsatOnayTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("ruhsat_onay_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.ToplamKatSayisi).HasColumnName("toplam_kat_sayisi");
            entity.Property(e => e.UavtIlAdi)
                .HasMaxLength(75)
                .HasColumnName("uavt_il_adi");
            entity.Property(e => e.UavtIlNo).HasColumnName("uavt_il_no");
            entity.Property(e => e.UavtIlceAdi)
                .HasMaxLength(100)
                .HasColumnName("uavt_ilce_adi");
            entity.Property(e => e.UavtIlceNo).HasColumnName("uavt_ilce_no");
            entity.Property(e => e.UavtMahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("uavt_mahalle_adi");
            entity.Property(e => e.UavtMahalleNo).HasColumnName("uavt_mahalle_no");
            entity.Property(e => e.YapiInsaatAlan)
                .HasPrecision(18, 6)
                .HasColumnName("yapi_insaat_alan");
            entity.Property(e => e.YapiKimlikNo).HasColumnName("yapi_kimlik_no");
            entity.Property(e => e.YibfNo).HasColumnName("yibf_no");

            entity.HasOne(d => d.BinaDegerlendirmeDurum).WithMany(p => p.BinaDegerlendirmes)
                .HasForeignKey(d => d.BinaDegerlendirmeDurumId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_bina_degerlendirme_bina_degerlendirme_durum_id");
        });

        modelBuilder.Entity<BinaDegerlendirmeDosya>(entity =>
        {
            entity.HasKey(e => e.BinaDegerlendirmeDosyaId).HasName("bina_degerlendirme_dosya_pkey");

            entity.ToTable("bina_degerlendirme_dosya");

            entity.Property(e => e.BinaDegerlendirmeDosyaId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_degerlendirme_dosya_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaDegerlendirmeDosyaTurId).HasColumnName("bina_degerlendirme_dosya_tur_id");
            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.BinaDosyaGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("bina_dosya_guid");
            entity.Property(e => e.DosyaAdi)
                .HasMaxLength(255)
                .HasColumnName("dosya_adi");
            entity.Property(e => e.DosyaTuru)
                .HasMaxLength(255)
                .HasColumnName("dosya_turu");
            entity.Property(e => e.DosyaYolu)
                .HasMaxLength(500)
                .HasColumnName("dosya_yolu");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.YeniYapiSilAciklama)
                .HasMaxLength(500)
                .HasColumnName("yeni_yapi_sil_aciklama");

            entity.HasOne(d => d.BinaDegerlendirmeDosyaTur).WithMany(p => p.BinaDegerlendirmeDosyas)
                .HasForeignKey(d => d.BinaDegerlendirmeDosyaTurId)
                .HasConstraintName("fk_bina_degerlendirme_dosya_bina_degerlendirme_dosya_tur_id");

            entity.HasOne(d => d.BinaDegerlendirme).WithMany(p => p.BinaDegerlendirmeDosyas)
                .HasForeignKey(d => d.BinaDegerlendirmeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_degerlendirme_dosya_bina_degerlendirme_id");
        });

        modelBuilder.Entity<BinaDegerlendirmeDosyaTur>(entity =>
        {
            entity.HasKey(e => e.BinaDegerlendirmeDosyaTurId).HasName("bina_degerlendirme_dosya_tur_pkey");

            entity.ToTable("bina_degerlendirme_dosya_tur");

            entity.Property(e => e.BinaDegerlendirmeDosyaTurId).HasColumnName("bina_degerlendirme_dosya_tur_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(100)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaDegerlendirmeDosyaTurGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("bina_degerlendirme_dosya_tur_guid");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BinaDegerlendirmeDurum>(entity =>
        {
            entity.HasKey(e => e.BinaDegerlendirmeDurumId).HasName("bina_degerlendirme_durum_pkey");

            entity.ToTable("bina_degerlendirme_durum");

            entity.Property(e => e.BinaDegerlendirmeDurumId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_degerlendirme_durum_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(100)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BinaKotUstuSayi>(entity =>
        {
            entity.HasKey(e => e.BinaKotUstuSayiId).HasName("bina_kot_ustu_sayi_pkey");

            entity.ToTable("bina_kot_ustu_sayi");

            entity.Property(e => e.BinaKotUstuSayiId).HasColumnName("bina_kot_ustu_sayi_id");
            entity.Property(e => e.Ada)
                .HasMaxLength(255)
                .HasColumnName("ada");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.KotUstuKatSayisi).HasColumnName("kot_ustu_kat_sayisi");
            entity.Property(e => e.Parsel)
                .HasMaxLength(255)
                .HasColumnName("parsel");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.UavtIlId).HasColumnName("uavt_il_id");
            entity.Property(e => e.UavtIlceId).HasColumnName("uavt_ilce_id");
            entity.Property(e => e.UavtMahalleId).HasColumnName("uavt_mahalle_id");
        });

        modelBuilder.Entity<BinaMuteahhit>(entity =>
        {
            entity.HasKey(e => e.BinaMuteahhitId).HasName("bina_muteahhit_pkey");

            entity.ToTable("bina_muteahhit");

            entity.Property(e => e.BinaMuteahhitId).HasColumnName("bina_muteahhit_id");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(1000)
                .HasColumnName("aciklama");
            entity.Property(e => e.Adres)
                .HasMaxLength(1000)
                .HasColumnName("adres");
            entity.Property(e => e.Adsoyadunvan)
                .HasMaxLength(500)
                .HasColumnName("adsoyadunvan");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.BinaMuteahhitTapuTurId).HasColumnName("bina_muteahhit_tapu_tur_id");
            entity.Property(e => e.CepTelefonu)
                .HasMaxLength(20)
                .HasColumnName("cep_telefonu");
            entity.Property(e => e.Eposta)
                .HasMaxLength(255)
                .HasColumnName("eposta");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.IbanNo)
                .HasMaxLength(50)
                .HasColumnName("iban_no");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.Telefon)
                .HasMaxLength(20)
                .HasColumnName("telefon");
            entity.Property(e => e.VergiKimlikNo)
                .HasMaxLength(20)
                .HasColumnName("vergi_kimlik_no");
            entity.Property(e => e.YetkiBelgeNo)
                .HasMaxLength(20)
                .HasColumnName("yetki_belge_no");

            entity.HasOne(d => d.BinaDegerlendirme).WithMany(p => p.BinaMuteahhits)
                .HasForeignKey(d => d.BinaDegerlendirmeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_muteahhit_bina_degerlendirme_id");

            entity.HasOne(d => d.BinaMuteahhitTapuTur).WithMany(p => p.BinaMuteahhits)
                .HasForeignKey(d => d.BinaMuteahhitTapuTurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_muteahhit_bina_muteahhit_tapu_tur_id");
        });

        modelBuilder.Entity<BinaMuteahhitTapuTur>(entity =>
        {
            entity.HasKey(e => e.BinaMuteahhitTapuTurId).HasName("bina_muteahhit_tapu_tur_pkey");

            entity.ToTable("bina_muteahhit_tapu_tur");

            entity.Property(e => e.BinaMuteahhitTapuTurId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_muteahhit_tapu_tur_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BinaNakdiYardimTaksit>(entity =>
        {
            entity.HasKey(e => e.BinaNakdiYardimTaksitId).HasName("bina_nakdi_yardim_taksit_pkey");

            entity.ToTable("bina_nakdi_yardim_taksit");

            entity.Property(e => e.BinaNakdiYardimTaksitId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_nakdi_yardim_taksit_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasColumnType("character varying")
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasColumnType("character varying")
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.TaksitYuzdesi).HasColumnName("taksit_yuzdesi");

            entity.HasOne(d => d.BinaDegerlendirme).WithMany(p => p.BinaNakdiYardimTaksits)
                .HasForeignKey(d => d.BinaDegerlendirmeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_nakdi_yardim_taksit_bina_degerlendirme_id");
        });

        modelBuilder.Entity<BinaNakdiYardimTaksitDosya>(entity =>
        {
            entity.HasKey(e => e.BinaNakdiYardimTaksitDosyaId).HasName("bina_nakdi_yardim_taksit_dosya_pkey");

            entity.ToTable("bina_nakdi_yardim_taksit_dosya");

            entity.Property(e => e.BinaNakdiYardimTaksitDosyaId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_nakdi_yardim_taksit_dosya_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaNakdiYardimTaksitDosyaGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("bina_nakdi_yardim_taksit_dosya_guid");
            entity.Property(e => e.BinaNakdiYardimTaksitId).HasColumnName("bina_nakdi_yardim_taksit_id");
            entity.Property(e => e.DosyaAdi)
                .HasMaxLength(255)
                .HasColumnName("dosya_adi");
            entity.Property(e => e.DosyaTuru)
                .HasMaxLength(255)
                .HasColumnName("dosya_turu");
            entity.Property(e => e.DosyaYolu)
                .HasMaxLength(500)
                .HasColumnName("dosya_yolu");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasColumnType("character varying")
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasColumnType("character varying")
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.BinaNakdiYardimTaksit).WithMany(p => p.BinaNakdiYardimTaksitDosyas)
                .HasForeignKey(d => d.BinaNakdiYardimTaksitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_nakdi_yardim_taksit_dosya_bina_nakdi_yardim_taksit_id");
        });

        modelBuilder.Entity<BinaOdeme>(entity =>
        {
            entity.HasKey(e => e.BinaOdemeId).HasName("bina_odeme_pkey");

            entity.ToTable("bina_odeme");

            entity.HasIndex(e => new { e.BinaDegerlendirmeId, e.AktifMi, e.SilindiMi }, "ix_bina_odeme_bina_degerlendirme_id");

            entity.HasIndex(e => new { e.BinaDegerlendirmeId, e.BinaOdemeDurumId, e.AktifMi, e.SilindiMi }, "ix_bina_odeme_bina_degerlendirme_id_bina_odeme_id");

            entity.Property(e => e.BinaOdemeId).HasColumnName("bina_odeme_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.BinaOdemeDurumId).HasColumnName("bina_odeme_durum_id");
            entity.Property(e => e.DigerHibeOdemeTutari)
                .HasPrecision(18, 6)
                .HasColumnName("diger_hibe_odeme_tutari");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasColumnType("character varying")
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.HibeOdemeTutari)
                .HasPrecision(18, 6)
                .HasColumnName("hibe_odeme_tutari");
            entity.Property(e => e.KrediOdemeTutari)
                .HasPrecision(18, 6)
                .HasColumnName("kredi_odeme_tutari");
            entity.Property(e => e.OdemeTutari)
                .HasPrecision(18, 6)
                .HasColumnName("odeme_tutari");
            entity.Property(e => e.OlusturanIp)
                .HasColumnType("character varying")
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.Seviye).HasColumnName("seviye");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.TalepDurumu)
                .HasColumnType("character varying")
                .HasColumnName("talep_durumu");
            entity.Property(e => e.TalepKapatmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("talep_kapatma_tarihi");
            entity.Property(e => e.TalepNumarasi)
                .HasDefaultValueSql("'2'::character varying")
                .HasColumnType("character varying")
                .HasColumnName("talep_numarasi");

            entity.HasOne(d => d.BinaDegerlendirme).WithMany(p => p.BinaOdemes)
                .HasForeignKey(d => d.BinaDegerlendirmeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_odeme_bina_degerlendirme_id");

            entity.HasOne(d => d.BinaOdemeDurum).WithMany(p => p.BinaOdemes)
                .HasForeignKey(d => d.BinaOdemeDurumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_odeme_bina_odeme_durum_id");

            entity.HasOne(d => d.GuncelleyenKullanici).WithMany(p => p.BinaOdemeGuncelleyenKullanicis)
                .HasForeignKey(d => d.GuncelleyenKullaniciId)
                .HasConstraintName("fk_bina_odeme_guncelleyen_kullanici_id");

            entity.HasOne(d => d.OlusturanKullanici).WithMany(p => p.BinaOdemeOlusturanKullanicis)
                .HasForeignKey(d => d.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_odeme_olusturan_kullanici_id");
        });

        modelBuilder.Entity<BinaOdemeDetay>(entity =>
        {
            entity.HasKey(e => e.BinaOdemeDetayId).HasName("bina_odeme_detay_pkey");

            entity.ToTable("bina_odeme_detay");

            entity.Property(e => e.BinaOdemeDetayId).HasColumnName("bina_odeme_detay_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaOdemeId).HasColumnName("bina_odeme_id");
            entity.Property(e => e.DigerHibeOdemeTutari)
                .HasPrecision(18, 6)
                .HasColumnName("diger_hibe_odeme_tutari");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasColumnType("character varying")
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.HibeOdemeTutari)
                .HasPrecision(18, 6)
                .HasColumnName("hibe_odeme_tutari");
            entity.Property(e => e.Iban)
                .HasMaxLength(50)
                .HasColumnName("iban");
            entity.Property(e => e.KrediOdemeTutari)
                .HasPrecision(18, 6)
                .HasColumnName("kredi_odeme_tutari");
            entity.Property(e => e.MuteahhitMi).HasColumnName("muteahhit_mi");
            entity.Property(e => e.OdemeTutari)
                .HasPrecision(18, 6)
                .HasColumnName("odeme_tutari");
            entity.Property(e => e.OlusturanIp)
                .HasColumnType("character varying")
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.BinaOdeme).WithMany(p => p.BinaOdemeDetays)
                .HasForeignKey(d => d.BinaOdemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_odeme_detay_bina_odeme_id");

            entity.HasOne(d => d.GuncelleyenKullanici).WithMany(p => p.BinaOdemeDetayGuncelleyenKullanicis)
                .HasForeignKey(d => d.GuncelleyenKullaniciId)
                .HasConstraintName("fk_bina_odeme_detay_guncelleyen_kullanici_id");

            entity.HasOne(d => d.OlusturanKullanici).WithMany(p => p.BinaOdemeDetayOlusturanKullanicis)
                .HasForeignKey(d => d.OlusturanKullaniciId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_odeme_detay_olusturan_kullanici_id");
        });

        modelBuilder.Entity<BinaOdemeDurum>(entity =>
        {
            entity.HasKey(e => e.BinaOdemeDurumId).HasName("bina_odeme_durum_pkey");

            entity.ToTable("bina_odeme_durum");

            entity.Property(e => e.BinaOdemeDurumId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_odeme_durum_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<BinaYapiDenetimSeviyeTespit>(entity =>
        {
            entity.HasKey(e => e.BinaYapiDenetimSeviyeTespitId).HasName("bina_yapi_denetim_seviye_tespit_pkey");

            entity.ToTable("bina_yapi_denetim_seviye_tespit");

            entity.Property(e => e.BinaYapiDenetimSeviyeTespitId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_yapi_denetim_seviye_tespit_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasColumnType("character varying")
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.IlerlemeYuzdesi).HasColumnName("ilerleme_yuzdesi");
            entity.Property(e => e.OlusturanIp)
                .HasColumnType("character varying")
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.BinaDegerlendirme).WithMany(p => p.BinaYapiDenetimSeviyeTespits)
                .HasForeignKey(d => d.BinaDegerlendirmeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_yapi_denetim_seviye_tespit_bina_degerlendirme_id");
        });

        modelBuilder.Entity<BinaYapiDenetimSeviyeTespitDosya>(entity =>
        {
            entity.HasKey(e => e.BinaYapiDenetimSeviyeTespitDosyaId).HasName("bina_yapi_denetim_seviye_tespit_dosya_pkey");

            entity.ToTable("bina_yapi_denetim_seviye_tespit_dosya");

            entity.Property(e => e.BinaYapiDenetimSeviyeTespitDosyaId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_yapi_denetim_seviye_tespit_dosya_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaYapiDenetimSeviyeTespitDosyaGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("bina_yapi_denetim_seviye_tespit_dosya_guid");
            entity.Property(e => e.BinaYapiDenetimSeviyeTespitId).HasColumnName("bina_yapi_denetim_seviye_tespit_id");
            entity.Property(e => e.DosyaAdi)
                .HasMaxLength(255)
                .HasColumnName("dosya_adi");
            entity.Property(e => e.DosyaTuru)
                .HasMaxLength(255)
                .HasColumnName("dosya_turu");
            entity.Property(e => e.DosyaYolu)
                .HasMaxLength(500)
                .HasColumnName("dosya_yolu");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasColumnType("character varying")
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasColumnType("character varying")
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.BinaYapiDenetimSeviyeTespit).WithMany(p => p.BinaYapiDenetimSeviyeTespitDosyas)
                .HasForeignKey(d => d.BinaYapiDenetimSeviyeTespitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_yapi_denetim_seviye_tespit_yapi_denetim_seviye_tespit_d");
        });

        modelBuilder.Entity<BinaYapiRuhsatIzinDosya>(entity =>
        {
            entity.HasKey(e => e.BinaYapiRuhsatIzinDosyaId).HasName("bina_yapi_ruhsat_izin_dosya_pkey");

            entity.ToTable("bina_yapi_ruhsat_izin_dosya");

            entity.Property(e => e.BinaYapiRuhsatIzinDosyaId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("bina_yapi_ruhsat_izin_dosya_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BinaDegerlendirmeId).HasColumnName("bina_degerlendirme_id");
            entity.Property(e => e.BinaYapiRuhsatIzinDosyaGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("bina_yapi_ruhsat_izin_dosya_guid");
            entity.Property(e => e.DosyaAdi)
                .HasMaxLength(255)
                .HasColumnName("dosya_adi");
            entity.Property(e => e.DosyaTuru)
                .HasMaxLength(255)
                .HasColumnName("dosya_turu");
            entity.Property(e => e.DosyaYolu)
                .HasMaxLength(500)
                .HasColumnName("dosya_yolu");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.BinaDegerlendirme).WithMany(p => p.BinaYapiRuhsatIzinDosyas)
                .HasForeignKey(d => d.BinaDegerlendirmeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bina_yapi_ruhsat_izin_dosya_bina_degerlendirme_id");
        });

        modelBuilder.Entity<Birim>(entity =>
        {
            entity.HasKey(e => e.BirimId).HasName("birim_pkey");

            entity.ToTable("birim");

            entity.Property(e => e.BirimId).HasColumnName("birim_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(100)
                .HasColumnName("ad");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.KurumId).HasColumnName("kurum_id");

            entity.HasOne(d => d.Kurum).WithMany(p => p.Birims)
                .HasForeignKey(d => d.KurumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_birim_kurum_id");
        });

        modelBuilder.Entity<HizmetAciklama>(entity =>
        {
            entity.HasKey(e => e.HizmetAciklamaId).HasName("hizmet_aciklama_pkey");

            entity.ToTable("hizmet_aciklama");

            entity.Property(e => e.HizmetAciklamaId).HasColumnName("hizmet_aciklama_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.Icerik).HasColumnName("icerik");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.TuzelMi)
                .HasComment("tüzel veya gerçek kişi açıklaması")
                .HasColumnName("tuzel_mi");
        });

        modelBuilder.Entity<IstisnaAskiKodu>(entity =>
        {
            entity.HasKey(e => e.IstisnaAskiKoduId).HasName("ayar_copy1_pkey");

            entity.ToTable("istisna_aski_kodu");

            entity.Property(e => e.IstisnaAskiKoduId).HasColumnName("istisna_aski_kodu_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.AskiKodu)
                .HasMaxLength(25)
                .HasColumnName("aski_kodu");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<Kullanici>(entity =>
        {
            entity.HasKey(e => e.KullaniciId).HasName("kullanici_pkey");

            entity.ToTable("kullanici");

            entity.Property(e => e.KullaniciId).HasColumnName("kullanici_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.BirimId).HasColumnName("birim_id");
            entity.Property(e => e.CepTelefonu)
                .HasMaxLength(20)
                .HasColumnName("cep_telefonu");
            entity.Property(e => e.Eposta)
                .HasMaxLength(255)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("eposta");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.KullaniciAdi)
                .HasMaxLength(75)
                .HasColumnName("kullanici_adi");
            entity.Property(e => e.KullaniciGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("kullanici_guid");
            entity.Property(e => e.KullaniciHesapTipId).HasColumnName("kullanici_hesap_tip_id");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(255)
                .HasColumnName("refresh_token");
            entity.Property(e => e.Sifre)
                .HasMaxLength(255)
                .HasColumnName("sifre");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.SistemKullanicisiMi)
                .HasComment("webservis kullanıcısı ise true olacak, bu kullanıcılar admin panelinden görünmez")
                .HasColumnName("sistem_kullanicisi_mi");
            entity.Property(e => e.SonGirisYapilanIp)
                .HasMaxLength(50)
                .HasColumnName("son_giris_yapilan_ip");
            entity.Property(e => e.SonGirisYapilanTarih)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("son_giris_yapilan_tarih");
            entity.Property(e => e.SonSifreDegisimTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("son_sifre_degisim_tarihi");
            entity.Property(e => e.Soyad)
                .HasMaxLength(255)
                .HasColumnName("soyad");
            entity.Property(e => e.TcKimlikNo).HasColumnName("tc_kimlik_no");

            entity.HasOne(d => d.Birim).WithMany(p => p.Kullanicis)
                .HasForeignKey(d => d.BirimId)
                .HasConstraintName("fk_kullanici_birim_id");

            entity.HasOne(d => d.KullaniciHesapTip).WithMany(p => p.Kullanicis)
                .HasForeignKey(d => d.KullaniciHesapTipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_kullanici_kullanici_hesap_tip_id");
        });

        modelBuilder.Entity<KullaniciGirisBasarili>(entity =>
        {
            entity.HasKey(e => e.KullaniciGirisBasariliId).HasName("kullanici_giris_basarili_pkey");

            entity.ToTable("kullanici_giris_basarili");

            entity.Property(e => e.KullaniciGirisBasariliId).HasColumnName("kullanici_giris_basarili_id");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(255)
                .HasColumnName("aciklama");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.IpAdres)
                .HasMaxLength(20)
                .HasColumnName("ip_adres");
            entity.Property(e => e.KullaniciAdi)
                .HasMaxLength(75)
                .HasColumnName("kullanici_adi");
            entity.Property(e => e.KullaniciId).HasColumnName("kullanici_id");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.RequestHeader).HasColumnName("request_header");
            entity.Property(e => e.Sifre)
                .HasMaxLength(255)
                .HasColumnName("sifre");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.KullaniciGirisBasarilis)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("fk_kullanici_giris_basarili_kullanici_id");
        });

        modelBuilder.Entity<KullaniciGirisHatum>(entity =>
        {
            entity.HasKey(e => e.KullaniciGirisHataId).HasName("kullanici_giris_hata_pkey");

            entity.ToTable("kullanici_giris_hata");

            entity.Property(e => e.KullaniciGirisHataId).HasColumnName("kullanici_giris_hata_id");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(255)
                .HasColumnName("aciklama");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.GirisGuid)
                .HasMaxLength(255)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("giris_guid");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.IpAdres)
                .HasMaxLength(20)
                .HasColumnName("ip_adres");
            entity.Property(e => e.KullaniciAdi)
                .HasMaxLength(75)
                .HasColumnName("kullanici_adi");
            entity.Property(e => e.KullaniciId).HasColumnName("kullanici_id");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.RequestHeader).HasColumnName("request_header");
            entity.Property(e => e.Sifre)
                .HasMaxLength(255)
                .HasColumnName("sifre");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.KullaniciGirisHata)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("fk_kullanici_giris_hata_kullanici_id");
        });

        modelBuilder.Entity<KullaniciGirisKod>(entity =>
        {
            entity.HasKey(e => e.KullaniciGirisKodId).HasName("kullanici_giris_kod_pkey");

            entity.ToTable("kullanici_giris_kod");

            entity.Property(e => e.KullaniciGirisKodId).HasColumnName("kullanici_giris_kod_id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.GirisGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("giris_guid");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.KullaniciId).HasColumnName("kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.SmsGonderildiMi).HasColumnName("sms_gonderildi_mi");
            entity.Property(e => e.Tamamlandi).HasColumnName("tamamlandi");
            entity.Property(e => e.Telefon)
                .HasMaxLength(50)
                .HasColumnName("telefon");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.KullaniciGirisKods)
                .HasForeignKey(d => d.KullaniciId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_kullanici_giris_kod_kullanici_id");
        });

        modelBuilder.Entity<KullaniciGirisKodDeneme>(entity =>
        {
            entity.HasKey(e => e.KullaniciGirisKodDenemeId).HasName("kullanici_giris_kod_deneme_pkey");

            entity.ToTable("kullanici_giris_kod_deneme");

            entity.Property(e => e.KullaniciGirisKodDenemeId).HasColumnName("kullanici_giris_kod_deneme_id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.GirisGuid)
                .HasMaxLength(255)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("giris_guid");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.IpAdres)
                .HasMaxLength(20)
                .HasColumnName("ip_adres");
            entity.Property(e => e.KullaniciId).HasColumnName("kullanici_id");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi)
                .HasDefaultValueSql("false")
                .HasColumnName("silindi_mi");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.KullaniciGirisKodDenemes)
                .HasForeignKey(d => d.KullaniciId)
                .HasConstraintName("fk_kullanici_giris_kod_deneme_kullanici_id");
        });

        modelBuilder.Entity<KullaniciHesapTip>(entity =>
        {
            entity.HasKey(e => e.KullaniciHesapTipId).HasName("kullanici_hesap_tip_pkey");

            entity.ToTable("kullanici_hesap_tip");

            entity.Property(e => e.KullaniciHesapTipId).HasColumnName("kullanici_hesap_tip_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(100)
                .HasColumnName("ad");
        });

        modelBuilder.Entity<KullaniciRol>(entity =>
        {
            entity.HasKey(e => e.KullaniciRolId).HasName("kullanici_rol_pkey");

            entity.ToTable("kullanici_rol");

            entity.Property(e => e.KullaniciRolId).HasColumnName("kullanici_rol_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.KullaniciId).HasColumnName("kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");

            entity.HasOne(d => d.Kullanici).WithMany(p => p.KullaniciRols)
                .HasForeignKey(d => d.KullaniciId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_kullanici_rol_kullanici_id");

            entity.HasOne(d => d.Rol).WithMany(p => p.KullaniciRols)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_kullanici_rol_rol_id");
        });

        modelBuilder.Entity<Kurum>(entity =>
        {
            entity.HasKey(e => e.KurumId).HasName("kurum_pkey");

            entity.ToTable("kurum");

            entity.Property(e => e.KurumId).HasColumnName("kurum_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(100)
                .HasColumnName("ad");
        });

        modelBuilder.Entity<OfisKonum>(entity =>
        {
            entity.HasKey(e => e.OfisKonumId).HasName("ofis_konum_pkey");

            entity.ToTable("ofis_konum");

            entity.Property(e => e.OfisKonumId).HasColumnName("ofis_konum_id");
            entity.Property(e => e.Adres)
                .HasMaxLength(255)
                .HasColumnName("adres");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.HaritaUrl)
                .HasMaxLength(255)
                .HasColumnName("harita_url");
            entity.Property(e => e.IlAdi)
                .HasMaxLength(255)
                .HasColumnName("il_adi");
            entity.Property(e => e.IlceAdi)
                .HasMaxLength(255)
                .HasColumnName("ilce_adi");
            entity.Property(e => e.Konum).HasColumnName("konum");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("rol_pkey");

            entity.ToTable("rol");

            entity.Property(e => e.RolId).HasColumnName("rol_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(100)
                .HasColumnName("ad");
        });

        modelBuilder.Entity<SikcaSorulanSoru>(entity =>
        {
            entity.HasKey(e => e.SikcaSorulanSoruId).HasName("sikca_sorulan_soru_pkey");

            entity.ToTable("sikca_sorulan_soru");

            entity.Property(e => e.SikcaSorulanSoruId).HasColumnName("sikca_sorulan_soru_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.Cevap).HasColumnName("cevap");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasMaxLength(50)
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.SiraNo).HasColumnName("sira_no");
            entity.Property(e => e.Soru)
                .HasMaxLength(500)
                .HasColumnName("soru");
        });

        modelBuilder.Entity<SmsLog>(entity =>
        {
            entity.HasKey(e => e.SmsLogId).HasName("sms_log_pkey");

            entity.ToTable("sms_log");

            entity.Property(e => e.SmsLogId).HasColumnName("sms_log_id");
            entity.Property(e => e.ApiMesaj)
                .HasMaxLength(1000)
                .HasColumnName("api_mesaj");
            entity.Property(e => e.ApiSmsId)
                .HasMaxLength(100)
                .HasColumnName("api_sms_id");
            entity.Property(e => e.GonderildiMi).HasColumnName("gonderildi_mi");
            entity.Property(e => e.Icerik)
                .HasMaxLength(1000)
                .HasColumnName("icerik");
            entity.Property(e => e.OlusturmaTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.Telefon)
                .HasMaxLength(20)
                .HasColumnName("telefon");
        });

        modelBuilder.Entity<TakbisIl>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("takbis_il");

            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.Aktif).HasColumnName("aktif");
            entity.Property(e => e.EklenmeTarihi)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("eklenme_tarihi");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.IlKod).HasColumnName("il_kod");
            entity.Property(e => e.TakbisIlId)
                .ValueGeneratedOnAdd()
                .HasColumnName("takbis_il_id");
            entity.Property(e => e.TakbisIlKod).HasColumnName("takbis_il_kod");
        });

        modelBuilder.Entity<TebligatGonderim>(entity =>
        {
            entity.HasKey(e => e.TebligatGonderimId).HasName("tebligat_gonderim_pkey");

            entity.ToTable("tebligat_gonderim");

            entity.Property(e => e.TebligatGonderimId).HasColumnName("tebligat_gonderim_id");
            entity.Property(e => e.EdevletTakipId).HasColumnName("edevlet_takip_id");
            entity.Property(e => e.GonderenKullaniciId).HasColumnName("gonderen_kullanici_id");
            entity.Property(e => e.GonderimAciklama)
                .HasMaxLength(1000)
                .HasColumnName("gonderim_aciklama");
            entity.Property(e => e.GonderimBasariliMi).HasColumnName("gonderim_basarili_mi");
            entity.Property(e => e.GonderimTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("gonderim_tarihi");

            entity.HasOne(d => d.GonderenKullanici).WithMany(p => p.TebligatGonderims)
                .HasForeignKey(d => d.GonderenKullaniciId)
                .HasConstraintName("fk_tebligat_gonderim_kullanici_gonderen_kullanici_id");
        });

        modelBuilder.Entity<TebligatGonderimDetay>(entity =>
        {
            entity.HasKey(e => e.TebligatGonderimDetayId).HasName("tebligat_gonderim_detay_pkey");

            entity.ToTable("tebligat_gonderim_detay");

            entity.Property(e => e.TebligatGonderimDetayId).HasColumnName("tebligat_gonderim_detay_id");
            entity.Property(e => e.HasarTespitAskiKodu)
                .HasMaxLength(100)
                .HasColumnName("hasar_tespit_aski_kodu");
            entity.Property(e => e.HasarTespitHasarDurumu)
                .HasMaxLength(100)
                .HasColumnName("hasar_tespit_hasar_durumu");
            entity.Property(e => e.TapuAda)
                .HasMaxLength(10)
                .HasColumnName("tapu_ada");
            entity.Property(e => e.TapuIlAdi)
                .HasMaxLength(75)
                .HasColumnName("tapu_il_adi");
            entity.Property(e => e.TapuIlceAdi)
                .HasMaxLength(75)
                .HasColumnName("tapu_ilce_adi");
            entity.Property(e => e.TapuMahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("tapu_mahalle_adi");
            entity.Property(e => e.TapuParsel)
                .HasMaxLength(10)
                .HasColumnName("tapu_parsel");
            entity.Property(e => e.TapuTasinmazId).HasColumnName("tapu_tasinmaz_id");
            entity.Property(e => e.TcKimlikNo)
                .HasMaxLength(11)
                .HasColumnName("tc_kimlik_no");
            entity.Property(e => e.TebligTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("teblig_tarihi");
            entity.Property(e => e.TebligatGonderimId).HasColumnName("tebligat_gonderim_id");
            entity.Property(e => e.TebligatTipiId).HasColumnName("tebligat_tipi_id");
            entity.Property(e => e.TuzelKisiVergiNo)
                .HasMaxLength(15)
                .HasColumnName("tuzel_kisi_vergi_no");

            entity.HasOne(d => d.TebligatGonderim).WithMany(p => p.TebligatGonderimDetays)
                .HasForeignKey(d => d.TebligatGonderimId)
                .HasConstraintName("tebligat_gonderim_detay_tebligat_gonderim_tebligat_gonderim_id");
        });

        modelBuilder.Entity<TebligatGonderimDetayDosya>(entity =>
        {
            entity.HasKey(e => e.TebligatGonderimDetayDosyaId).HasName("tebligat_gonderim_detay_dosya_pkey");

            entity.ToTable("tebligat_gonderim_detay_dosya");

            entity.Property(e => e.TebligatGonderimDetayDosyaId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("tebligat_gonderim_detay_dosya_id");
            entity.Property(e => e.AktifMi)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif_mi");
            entity.Property(e => e.DosyaAdi)
                .HasMaxLength(255)
                .HasColumnName("dosya_adi");
            entity.Property(e => e.DosyaTuru)
                .HasMaxLength(255)
                .HasColumnName("dosya_turu");
            entity.Property(e => e.DosyaYolu)
                .HasMaxLength(500)
                .HasColumnName("dosya_yolu");
            entity.Property(e => e.GuncellemeTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.GuncelleyenIp)
                .HasColumnType("character varying")
                .HasColumnName("guncelleyen_ip");
            entity.Property(e => e.GuncelleyenKullaniciId).HasColumnName("guncelleyen_kullanici_id");
            entity.Property(e => e.OlusturanIp)
                .HasColumnType("character varying")
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("olusturma_tarihi");
            entity.Property(e => e.SilindiMi).HasColumnName("silindi_mi");
            entity.Property(e => e.TebligatGonderimDetayDosyaGuid)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("tebligat_gonderim_detay_dosya_guid");
            entity.Property(e => e.TebligatGonderimDetayId).HasColumnName("tebligat_gonderim_detay_id");

            entity.HasOne(d => d.TebligatGonderimDetay).WithMany(p => p.TebligatGonderimDetayDosyas)
                .HasForeignKey(d => d.TebligatGonderimDetayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tebligat_gonderim_detay_dosya_tebligat_gonderim_detay_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
