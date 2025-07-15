using System;
using System.Collections.Generic;
using Csb.YerindeDonusum.Domain.Entities.Kds;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Persistance.Context;

public partial class KdsDbContext : DbContext
{
    public KdsDbContext()
    {
    }

    public KdsDbContext(DbContextOptions<KdsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Basvuru> Basvurus { get; set; }

    public virtual DbSet<Hane> Hanes { get; set; }

    public virtual DbSet<HasartespitTespitVeri> HasartespitTespitVeris { get; set; }

    public virtual DbSet<KdsYerindedonusumBinabazliOran> KdsYerindedonusumBinabazliOrans { get; set; }

    public virtual DbSet<KdsYerindedonusumRezervAlanlar> KdsYerindedonusumRezervAlanlars { get; set; }

    public virtual DbSet<TblBagimsizBolum> TblBagimsizBolums { get; set; }

    public virtual DbSet<TblCsbm> TblCsbms { get; set; }

    public virtual DbSet<TblIl> TblIls { get; set; }

    public virtual DbSet<TblIlce> TblIlces { get; set; }

    public virtual DbSet<TblKoy> TblKoys { get; set; }

    public virtual DbSet<TblMahalle> TblMahalles { get; set; }

    public virtual DbSet<TblNumarataj> TblNumaratajs { get; set; }

    public virtual DbSet<TblTanim> TblTanims { get; set; }

    public virtual DbSet<TblYapi> TblYapis { get; set; }

    public virtual DbSet<TblYetkiliIdare> TblYetkiliIdares { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:Kds", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("btree_gist")
            .HasPostgresExtension("pg_prewarm")
            .HasPostgresExtension("pg_stat_statements")
            .HasPostgresExtension("pg_trgm")
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Basvuru>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("basvuru", "ods_yerindedonusum");

            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasColumnName("ad");
            entity.Property(e => e.AktifMi).HasColumnName("aktif_mi");
            entity.Property(e => e.AydinlatmaMetniId).HasColumnName("aydinlatma_metni_id");
            entity.Property(e => e.BasvuruAfadDurumGuncellemeTarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("basvuru_afad_durum_guncelleme_tarihi");
            entity.Property(e => e.BasvuruAfadDurumId).HasColumnName("basvuru_afad_durum_id");
            entity.Property(e => e.BasvuruDestekTurId).HasColumnName("basvuru_destek_tur_id");
            entity.Property(e => e.BasvuruDurumId).HasColumnName("basvuru_durum_id");
            entity.Property(e => e.BasvuruGuid).HasColumnName("basvuru_guid");
            entity.Property(e => e.BasvuruId).HasColumnName("basvuru_id");
            entity.Property(e => e.BasvuruIptalAciklamasi)
                .HasMaxLength(2000)
                .HasColumnName("basvuru_iptal_aciklamasi");
            entity.Property(e => e.BasvuruKamuUstlenecekGuncellemeTarihi)
                .HasColumnType("timestamp without time zone")
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
                .HasColumnType("timestamp without time zone")
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
                .HasColumnType("timestamp without time zone")
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
            entity.Property(e => e.KacakYapiMi).HasColumnName("kacak_yapi_mi");
            entity.Property(e => e.OlusturanIp)
                .HasMaxLength(50)
                .HasColumnName("olusturan_ip");
            entity.Property(e => e.OlusturanKullaniciId).HasColumnName("olusturan_kullanici_id");
            entity.Property(e => e.OlusturmaTarihi)
                .HasColumnType("timestamp without time zone")
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
                .HasPrecision(33, 15)
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
            entity.Property(e => e.VatandaslikDurumu).HasColumnName("vatandaslik_durumu");
        });

        modelBuilder.Entity<Hane>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("hane", "kds");

            entity.Property(e => e.AdaNo)
                .HasMaxLength(255)
                .HasColumnName("ada_no");
            entity.Property(e => e.AfetId).HasColumnName("afet_id");
            entity.Property(e => e.Asci)
                .HasMaxLength(50)
                .HasColumnName("asci");
            entity.Property(e => e.AskiKodu)
                .HasMaxLength(255)
                .HasColumnName("aski_kodu");
            entity.Property(e => e.DisKapiNo).HasColumnName("dis_kapi_no");
            entity.Property(e => e.GuclendirmeMahkemeSonucu)
                .HasMaxLength(255)
                .HasColumnName("guclendirme_mahkeme_sonucu");
            entity.Property(e => e.GuclendirmeMahkemeSonucuId).HasColumnName("guclendirme_mahkeme_sonucu_id");
            entity.Property(e => e.HasarDurumu)
                .HasMaxLength(255)
                .HasColumnName("hasar_durumu");
            entity.Property(e => e.HasarDurumuId).HasColumnName("hasar_durumu_id");
            entity.Property(e => e.Huid).HasColumnName("huid");
            entity.Property(e => e.IlAd)
                .HasMaxLength(255)
                .HasColumnName("il_ad");
            entity.Property(e => e.IlKod).HasColumnName("il_kod");
            entity.Property(e => e.IlceAd)
                .HasMaxLength(255)
                .HasColumnName("ilce_ad");
            entity.Property(e => e.IlceKod).HasColumnName("ilce_kod");
            entity.Property(e => e.ItirazSonucu)
                .HasMaxLength(255)
                .HasColumnName("itiraz_sonucu");
            entity.Property(e => e.MahalleAd)
                .HasMaxLength(255)
                .HasColumnName("mahalle_ad");
            entity.Property(e => e.MahalleKod).HasColumnName("mahalle_kod");
            entity.Property(e => e.ParselNo)
                .HasMaxLength(255)
                .HasColumnName("parsel_no");
            entity.Property(e => e.Sokak).HasColumnName("sokak");
            entity.Property(e => e.TespitTabloMu)
                .HasDefaultValueSql("false")
                .HasColumnName("tespit_tablo_mu");
            entity.Property(e => e.Uavtkod).HasColumnName("uavtkod");
            entity.Property(e => e.Uid)
                .HasMaxLength(64)
                .HasColumnName("uid");
            entity.Property(e => e.YayinlansinMi).HasColumnName("yayinlansin_mi");
            entity.Property(e => e.YayinlansinMi2).HasColumnName("yayinlansin_mi_2");
        });

        modelBuilder.Entity<HasartespitTespitVeri>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("pk_hasartespit_tespit_veri");

            entity.ToTable("hasartespit_tespit_veri", "kds");

            entity.HasIndex(e => new { e.Uid, e.IlKod, e.YayinlansinMi }, "hasartespit_tespit_veri_uid_ilkod_yayinlansinmi");

            entity.HasIndex(e => e.AskiKodu, "ix_hasartespit_tespit_veri_aski_kodu");

            entity.HasIndex(e => new { e.AskiKodu, e.IlKod, e.YayinlansinMi }, "ix_hasartespit_tespit_veri_aski_kodu_il_kod");

            entity.HasIndex(e => e.HasarDurumuId, "ix_hasartespit_tespit_veri_hasar_durumu_id");

            entity.HasIndex(e => e.IlKod, "ix_hasartespit_tespit_veri_il_kod");

            entity.HasIndex(e => e.IlceKod, "ix_hasartespit_tespit_veri_ilce_kod");

            entity.HasIndex(e => e.MaksMahalleKod, "ix_hasartespit_tespit_veri_maks_mahalle_kod");

            entity.HasIndex(e => e.Uid, "ix_hasartespit_tespit_veri_uid");

            entity.HasIndex(e => new { e.Uid, e.YayinlansinMi }, "ix_hasartespit_tespit_veri_uid_yayinlansinmi");

            entity.HasIndex(e => e.YapiKimlikNo, "ix_hasartespit_tespit_veri_yapi_kimlik_no");

            entity.HasIndex(e => e.MahalleKod, "sx_hasartespit_tespit_veri_mahalle_kod");

            entity.Property(e => e.Uid)
                .HasMaxLength(64)
                .HasComment("tespıt uıd bılgısı")
                .HasColumnName("uid");
            entity.Property(e => e.Aciklama).HasColumnName("aciklama");
            entity.Property(e => e.AdaNo)
                .HasMaxLength(255)
                .HasColumnName("ada_no");
            entity.Property(e => e.AfetGrupId).HasColumnName("afet_grup_id");
            entity.Property(e => e.AfetId).HasColumnName("afet_id");
            entity.Property(e => e.AfetTanim)
                .HasMaxLength(255)
                .HasColumnName("afet_tanim");
            entity.Property(e => e.AfetTarih)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("afet_tarih");
            entity.Property(e => e.AhirSayisi).HasColumnName("ahir_sayisi");
            entity.Property(e => e.Alan1).HasColumnName("alan_1");
            entity.Property(e => e.Alan2).HasColumnName("alan_2");
            entity.Property(e => e.Alan3).HasColumnName("alan_3");
            entity.Property(e => e.Alan4).HasColumnName("alan_4");
            entity.Property(e => e.Alan5).HasColumnName("alan_5");
            entity.Property(e => e.Alan6).HasColumnName("alan_6");
            entity.Property(e => e.AskiKodu)
                .HasMaxLength(255)
                .HasColumnName("aski_kodu");
            entity.Property(e => e.BinaKodu)
                .HasMaxLength(64)
                .HasColumnName("bina_kodu");
            entity.Property(e => e.CreateDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_date");
            entity.Property(e => e.DisKapiNo).HasColumnName("dis_kapi_no");
            entity.Property(e => e.DoluKonutSayisi).HasColumnName("dolu_konut_sayisi");
            entity.Property(e => e.EtlDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("etl_date");
            entity.Property(e => e.GuclendirmeMahkemeSonucu)
                .HasMaxLength(255)
                .HasColumnName("guclendirme_mahkeme_sonucu");
            entity.Property(e => e.GuclendirmeMahkemeSonucuId).HasColumnName("guclendirme_mahkeme_sonucu_id");
            entity.Property(e => e.HacimM2).HasColumnName("hacim_m2");
            entity.Property(e => e.HacimMolozM3).HasColumnName("hacim_moloz_m3");
            entity.Property(e => e.HafriyatIslemBugun).HasColumnName("hafriyat_islem_bugun");
            entity.Property(e => e.HafriyatIslemTarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("hafriyat_islem_tarihi");
            entity.Property(e => e.HasarDurumu)
                .HasMaxLength(255)
                .HasColumnName("hasar_durumu");
            entity.Property(e => e.HasarDurumuId).HasColumnName("hasar_durumu_id");
            entity.Property(e => e.HasarDurumuRenkKod)
                .HasMaxLength(64)
                .HasColumnName("hasar_durumu_renk_kod");
            entity.Property(e => e.IlAd)
                .HasMaxLength(255)
                .HasColumnName("il_ad");
            entity.Property(e => e.IlKod).HasColumnName("il_kod");
            entity.Property(e => e.IlceAd)
                .HasMaxLength(255)
                .HasColumnName("ilce_ad");
            entity.Property(e => e.IlceKod).HasColumnName("ilce_kod");
            entity.Property(e => e.ItirazSonucu)
                .HasMaxLength(255)
                .HasColumnName("itiraz_sonucu");
            entity.Property(e => e.ItirazSonucuId).HasColumnName("itiraz_sonucu_id");
            entity.Property(e => e.KatAdedi).HasColumnName("kat_adedi");
            entity.Property(e => e.KisiKimlik).HasColumnName("kisi_kimlik");
            entity.Property(e => e.KonutSayisi).HasColumnName("konut_sayisi");
            entity.Property(e => e.KullanimAmaci)
                .HasMaxLength(255)
                .HasColumnName("kullanim_amaci");
            entity.Property(e => e.MahalleAd)
                .HasMaxLength(255)
                .HasColumnName("mahalle_ad");
            entity.Property(e => e.MahalleKod).HasColumnName("mahalle_kod");
            entity.Property(e => e.MaksMahalleKod).HasColumnName("maks_mahalle_kod");
            entity.Property(e => e.ParselNo)
                .HasMaxLength(255)
                .HasColumnName("parsel_no");
            entity.Property(e => e.SamanlikSayisi).HasColumnName("samanlik_sayisi");
            entity.Property(e => e.SehirMerkezi).HasColumnName("sehir_merkezi");
            entity.Property(e => e.Sokak).HasColumnName("sokak");
            entity.Property(e => e.SonTespitZamani)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("son_tespit_zamani");
            entity.Property(e => e.TabanAlani).HasColumnName("taban_alani");
            entity.Property(e => e.TapuAlani)
                .HasMaxLength(64)
                .HasColumnName("tapu_alani");
            entity.Property(e => e.TapuKimlikNo).HasColumnName("tapu_kimlik_no");
            entity.Property(e => e.TapuZeminRef).HasColumnName("tapu_zemin_ref");
            entity.Property(e => e.TicarethaneSayisi).HasColumnName("ticarethane_sayisi");
            entity.Property(e => e.Uavtkod).HasColumnName("uavtkod");
            entity.Property(e => e.VeriTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("veri_tarihi");
            entity.Property(e => e.YapiKimlikNo)
                .HasMaxLength(255)
                .HasColumnName("yapi_kimlik_no");
            entity.Property(e => e.YapiKullanimIzin).HasColumnName("yapi_kullanim_izin");
            entity.Property(e => e.YapiNufus)
                .HasComment("Yapi içerisindeki nüfus verisinin nvi verisi ile eşleşme sonucu elde edilmiş nüfüs verisi")
                .HasColumnName("yapi_nufus");
            entity.Property(e => e.YayinlansinMi)
                .HasDefaultValueSql("false")
                .HasColumnName("yayinlansin_mi");
            entity.Property(e => e.YerdenYuksekKatSayisi).HasColumnName("yerden_yuksek_kat_sayisi");
            entity.Property(e => e.ZeminTipi)
                .HasMaxLength(2000)
                .HasColumnName("zemin_tipi");
        });

        modelBuilder.Entity<KdsYerindedonusumBinabazliOran>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kds_yerindedonusum_binabazli_oran_pk");

            entity.ToTable("kds_yerindedonusum_binabazli_oran", "ods_yerindedonusum");

            entity.HasIndex(e => e.UavtIlNo, "ix_kds_yerindedonusum_binabazli_oran_uavt_il_no");

            entity.HasIndex(e => e.UavtIlceNo, "ix_kds_yerindedonusum_binabazli_oran_uavt_ilce_no");

            entity.HasIndex(e => e.UavtMahalleNo, "ix_kds_yerindedonusum_binabazli_oran_uavt_mahalle_no");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BasvuruSay).HasColumnName("basvuru_say");
            entity.Property(e => e.HasarDurumu)
                .HasMaxLength(255)
                .HasColumnName("hasar_durumu");
            entity.Property(e => e.HasarTespitAskiKodu)
                .HasMaxLength(80)
                .HasColumnName("hasar_tespit_aski_kodu");
            entity.Property(e => e.HasarTespitIlAdi)
                .HasMaxLength(75)
                .HasColumnName("hasar_tespit_il_adi");
            entity.Property(e => e.HasarTespitIlceAdi)
                .HasMaxLength(100)
                .HasColumnName("hasar_tespit_ilce_adi");
            entity.Property(e => e.HasarTespitMahalleAdi)
                .HasMaxLength(273)
                .HasColumnName("hasar_tespit_mahalle_adi");
            entity.Property(e => e.ItirazSonucu)
                .HasMaxLength(255)
                .HasColumnName("itiraz_sonucu");
            entity.Property(e => e.KatAdedi).HasColumnName("kat_adedi");
            entity.Property(e => e.Oran).HasColumnName("oran");
            entity.Property(e => e.SonHasarDurumu)
                .HasMaxLength(255)
                .HasColumnName("son_hasar_durumu");
            entity.Property(e => e.SutunRenk).HasColumnName("sutun_renk");
            entity.Property(e => e.UavtIlNo).HasColumnName("uavt_il_no");
            entity.Property(e => e.UavtIlceNo).HasColumnName("uavt_ilce_no");
            entity.Property(e => e.UavtMahalleNo).HasColumnName("uavt_mahalle_no");
            entity.Property(e => e.YapiKimlikNo)
                .HasMaxLength(255)
                .HasColumnName("yapi_kimlik_no");
        });

        modelBuilder.Entity<KdsYerindedonusumRezervAlanlar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("kds_yerindedonusum_rezerv_alanlar_pk");

            entity.ToTable("kds_yerindedonusum_rezerv_alanlar", "ods_yerindedonusum");

            entity.HasIndex(e => e.HasarTespitAskiKodu, "idx_kds_yerindedonusum_rezerv_alanlar_hasar_tespit_aski_kodu");

            entity.HasIndex(e => e.IlId, "idx_kds_yerindedonusum_rezerv_alanlar_il_id");

            entity.HasIndex(e => e.IlceId, "idx_kds_yerindedonusum_rezerv_alanlar_ilce_id");

            entity.HasIndex(e => e.MahalleId, "idx_kds_yerindedonusum_rezerv_alanlar_mahalle_id");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AfetId).HasColumnName("afet_id");
            entity.Property(e => e.Aktifmi)
                .HasDefaultValueSql("true")
                .HasColumnName("aktifmi");
            entity.Property(e => e.EtlDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("etl_date");
            entity.Property(e => e.HasarTespitAskiKodu)
                .HasMaxLength(80)
                .HasColumnName("hasar_tespit_aski_kodu");
            entity.Property(e => e.HasarTespitUid)
                .HasMaxLength(64)
                .HasColumnName("hasar_tespit_uid");
            entity.Property(e => e.IlAdi)
                .HasMaxLength(75)
                .HasColumnName("il_adi");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.IlceAdi)
                .HasMaxLength(100)
                .HasColumnName("ilce_adi");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.ItirazSonucuHasarDurum)
                .HasMaxLength(255)
                .HasColumnName("itiraz_sonucu_hasar_durum");
            entity.Property(e => e.KentselKırsal).HasColumnName("kentsel_kırsal");
            entity.Property(e => e.MahalleAdi)
                .HasMaxLength(255)
                .HasColumnName("mahalle_adi");
            entity.Property(e => e.MahalleId).HasColumnName("mahalle_id");
            entity.Property(e => e.RezevrAlanAdi)
                .HasMaxLength(254)
                .HasColumnName("rezevr_alan_adi");
            entity.Property(e => e.Tarih)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("tarih");
        });

        modelBuilder.Entity<TblBagimsizBolum>(entity =>
        {
            entity.HasKey(e => e.TblBagimsizBolumId).HasName("pk_tbl_bagimsiz_bolum");

            entity.ToTable("tbl_bagimsiz_bolum", "boyut");

            entity.HasIndex(e => e.BinaNo, "ix_tbl_bagimsiz_bolum_binano");

            entity.HasIndex(e => e.Versiyon, "ix_tbl_bagimsiz_bolum_versiyon");

            entity.HasIndex(e => e.YapiKod, "ix_tbl_bagimsiz_bolum_yapikod");

            entity.HasIndex(e => e.BagimsizBolumKod, "uk_tbl_bagimsiz_bolum").IsUnique();

            entity.Property(e => e.TblBagimsizBolumId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_bagimsiz_bolum'::regclass)")
                .HasColumnName("tbl_bagimsiz_bolum_id");
            entity.Property(e => e.AdresNo).HasColumnName("adres_no");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif");
            entity.Property(e => e.BagimsizBolumKod).HasColumnName("bagimsiz_bolum_kod");
            entity.Property(e => e.BinaNo).HasColumnName("bina_no");
            entity.Property(e => e.DurumAck)
                .HasMaxLength(255)
                .HasColumnName("durum_ack");
            entity.Property(e => e.DurumKod).HasColumnName("durum_kod");
            entity.Property(e => e.IcKapiNo)
                .HasMaxLength(64)
                .HasColumnName("ic_kapi_no");
            entity.Property(e => e.Katno)
                .HasMaxLength(64)
                .HasColumnName("katno");
            entity.Property(e => e.Tapubagimsizbolumno)
                .HasMaxLength(64)
                .HasColumnName("tapubagimsizbolumno");
            entity.Property(e => e.TipAck)
                .HasMaxLength(255)
                .HasColumnName("tip_ack");
            entity.Property(e => e.TipKod).HasColumnName("tip_kod");
            entity.Property(e => e.Versiyon).HasColumnName("versiyon");
            entity.Property(e => e.YapiKod).HasColumnName("yapi_kod");
        });

        modelBuilder.Entity<TblCsbm>(entity =>
        {
            entity.HasKey(e => e.TblCsbmId).HasName("pk_tbl_csbm");

            entity.ToTable("tbl_csbm", "boyut");

            entity.HasIndex(e => e.MahalleKod, "ix_tbl_csbm_mahalle");

            entity.HasIndex(e => e.CsbmKod, "uk_tbl_csbm").IsUnique();

            entity.HasIndex(e => e.Versiyon, "uk_tbl_csbm_versiyon").IsUnique();

            entity.Property(e => e.TblCsbmId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_csbm'::regclass)")
                .HasComment("primary key")
                .HasColumnName("tbl_csbm_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(4000)
                .HasComment("#YolAd")
                .HasColumnName("ad");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasComment("#Aktif")
                .HasColumnName("aktif");
            entity.Property(e => e.CsbmKod)
                .HasComment("#YolKod")
                .HasColumnName("csbm_kod");
            entity.Property(e => e.GelismislikDurumKod)
                .HasMaxLength(64)
                .HasComment("yol gelismislik tanim kodu #TanimKod")
                .HasColumnName("gelismislik_durum_kod");
            entity.Property(e => e.IlKod)
                .HasComment("#IlKod")
                .HasColumnName("il_kod");
            entity.Property(e => e.IlceKod)
                .HasComment("#IlceKod")
                .HasColumnName("ilce_kod");
            entity.Property(e => e.MahalleKod)
                .HasComment("#MahalleKod")
                .HasColumnName("mahalle_kod");
            entity.Property(e => e.TipKod)
                .HasMaxLength(64)
                .HasComment("yol tipinin tanim kodu #TanimKod")
                .HasColumnName("tip_kod");
            entity.Property(e => e.Versiyon)
                .HasComment("#Versiyon")
                .HasColumnName("versiyon");
            entity.Property(e => e.YetkiliIdareKod)
                .HasComment("#YerelIdareKod")
                .HasColumnName("yetkili_idare_kod");

            entity.HasOne(d => d.GelismislikDurumKodNavigation).WithMany(p => p.TblCsbmGelismislikDurumKodNavigations)
                .HasPrincipalKey(p => p.TanimKod)
                .HasForeignKey(d => d.GelismislikDurumKod)
                .HasConstraintName("fk_tbl_csbm_gelismislik_durum");

            entity.HasOne(d => d.IlKodNavigation).WithMany(p => p.TblCsbms)
                .HasPrincipalKey(p => p.IlKod)
                .HasForeignKey(d => d.IlKod)
                .HasConstraintName("fk_tbl_csbm_ilkod");

            entity.HasOne(d => d.IlceKodNavigation).WithMany(p => p.TblCsbms)
                .HasPrincipalKey(p => p.IlceKod)
                .HasForeignKey(d => d.IlceKod)
                .HasConstraintName("fk_tbl_csbm_ilcekod");

            entity.HasOne(d => d.TipKodNavigation).WithMany(p => p.TblCsbmTipKodNavigations)
                .HasPrincipalKey(p => p.TanimKod)
                .HasForeignKey(d => d.TipKod)
                .HasConstraintName("fk_tbl_csbm_tip");

            entity.HasOne(d => d.YetkiliIdareKodNavigation).WithMany(p => p.TblCsbms)
                .HasPrincipalKey(p => p.YetkiliIdareKod)
                .HasForeignKey(d => d.YetkiliIdareKod)
                .HasConstraintName("fk_tbl_csbm_yetkili_idare");
        });

        modelBuilder.Entity<TblIl>(entity =>
        {
            entity.HasKey(e => e.IlId).HasName("pk_tbl_il");

            entity.ToTable("tbl_il", "boyut", tb => tb.HasComment("Aks sistemindeki il kayıtlarının tutulduğu tablodur. #Il"));

            entity.HasIndex(e => e.IlKod, "uk_tbl_il").IsUnique();

            entity.HasIndex(e => e.Versiyon, "uk_tbl_il_versiyon").IsUnique();

            entity.Property(e => e.IlId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_il'::regclass)")
                .HasComment("tablodaki anahtar alan, otomatik artan degerdedir.")
                .HasColumnName("il_id");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(2000)
                .HasComment("acikalma verisini tutmaktadir. #Aciklama")
                .HasColumnName("aciklama");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasComment("ilin ad bilgisini tutmaktadir. #IlAd")
                .HasColumnName("ad");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasComment("verinin gecerli kullanilabilir veri olma durumu #Aktif")
                .HasColumnName("aktif");
            entity.Property(e => e.Buyuksehir).HasColumnName("buyuksehir");
            entity.Property(e => e.IlKod)
                .HasComment("aks sistemindaki il degeri = plaka kodu, tabloda essizdir. #IlKod")
                .HasColumnName("il_kod");
            entity.Property(e => e.ResmiGazeteTarihi)
                .HasComment("il verisi ile ilgili son resmi gazate tarihi #ResmiTarih")
                .HasColumnName("resmi_gazete_tarihi");
            entity.Property(e => e.TapuKod)
                .HasComment("tapu sisteminde o ile ait karsilik gelen id degeridir. #TapuIlKod")
                .HasColumnName("tapu_kod");
            entity.Property(e => e.TuikKod)
                .HasMaxLength(64)
                .HasComment("tuik tarafinda ilin kod degerini tutmaktadir.")
                .HasColumnName("tuik_kod");
            entity.Property(e => e.Versiyon)
                .HasComment("guncel versiyon numarasi #Versiyon")
                .HasColumnName("versiyon");
        });

        modelBuilder.Entity<TblIlce>(entity =>
        {
            entity.HasKey(e => e.IlceId).HasName("pk_tbl_ilce");

            entity.ToTable("tbl_ilce", "boyut");

            entity.HasIndex(e => e.IlKod, "ix_tbl_ilce_il_kod");

            entity.HasIndex(e => e.Versiyon, "uk_tbl_ilce").IsUnique();

            entity.HasIndex(e => e.IlceKod, "uk_tbl_ilce_kod").IsUnique();

            entity.Property(e => e.IlceId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_ilce'::regclass)")
                .HasComment("tablodaki anahtar alan, otomatik artan degerdedir.")
                .HasColumnName("ilce_id");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(2000)
                .HasComment("acikalma verisini tutmaktadir. #Aciklama")
                .HasColumnName("aciklama");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasComment("ilcenin ad bilgisini tutmaktadir. #IlceAd")
                .HasColumnName("ad");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasComment("verinin gecerli kullanilabilir veri olma durumu #Aktif")
                .HasColumnName("aktif");
            entity.Property(e => e.IlKod)
                .HasComment("il tablosundaki kod bilgisi degerini tutmaktadir #IlceKod")
                .HasColumnName("il_kod");
            entity.Property(e => e.IlceKod)
                .HasComment("aks sistemindaki ilce degeri. #IlceKod")
                .HasColumnName("ilce_kod");
            entity.Property(e => e.MerkezIlce)
                .HasDefaultValueSql("false")
                .HasComment("İlçe merkezi ilçemi değil mi belirtmek için kullanılmakatdır.")
                .HasColumnName("merkez_ilce");
            entity.Property(e => e.ResmiGazeteTarihi)
                .HasComment("ilce verisi ile ilgili son resmi gazate tarihi #ResmiTarih")
                .HasColumnName("resmi_gazete_tarihi");
            entity.Property(e => e.TapuKod)
                .HasComment("tapu sisteminde o ilceye karsilik gelen id degeridir. #TapuIlceKod")
                .HasColumnName("tapu_kod");
            entity.Property(e => e.TuikKod)
                .HasMaxLength(64)
                .HasComment("tuik tarafinda ilcenin kod degerini tutmaktadir.")
                .HasColumnName("tuik_kod");
            entity.Property(e => e.Versiyon)
                .HasComment("guncel versiyon numarasi #Versiyon")
                .HasColumnName("versiyon");

            entity.HasOne(d => d.IlKodNavigation).WithMany(p => p.TblIlces)
                .HasPrincipalKey(p => p.IlKod)
                .HasForeignKey(d => d.IlKod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tbl_ilce_il_kod");
        });

        modelBuilder.Entity<TblKoy>(entity =>
        {
            entity.HasKey(e => e.TblKoyId).HasName("pk_tbl_koy");

            entity.ToTable("tbl_koy", "boyut");

            entity.HasIndex(e => e.BucakKod, "ix_tbl_koy_ilce");

            entity.HasIndex(e => e.KoyKod, "uk_tbl_koy").IsUnique();

            entity.Property(e => e.TblKoyId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_koy'::regclass)")
                .HasComment("primary key")
                .HasColumnName("tbl_koy_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasComment("#KoyAd")
                .HasColumnName("ad");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasComment("#Aktif")
                .HasColumnName("aktif");
            entity.Property(e => e.BucakKod)
                .HasComment("#BucakKod")
                .HasColumnName("bucak_kod");
            entity.Property(e => e.KoyKod)
                .HasComment("#KoyKod")
                .HasColumnName("koy_kod");
            entity.Property(e => e.Versiyon)
                .HasComment("#versiyonNo")
                .HasColumnName("versiyon");
        });

        modelBuilder.Entity<TblMahalle>(entity =>
        {
            entity.HasKey(e => e.MahalleId).HasName("pk_tbl_mahalle");

            entity.ToTable("tbl_mahalle", "boyut", tb => tb.HasComment("mahalle ve koy kayitlari bu tabloda toplanmaktadir. Hizli search islemeri icin tum ustr veri iliskisi taboya eklenmistir. #Mahalle"));

            entity.HasIndex(e => e.TakbisMahalleKod, "ix_tbl_mahalle_takbis_mahalle_kod");

            entity.HasIndex(e => e.MahalleTanimKod, "ix_tbl_mahalle_veri_tipi");

            entity.HasIndex(e => e.MahalleKod, "uk_tbl_mahalle_kod").IsUnique();

            entity.HasIndex(e => e.Versiyon, "uk_tbl_mahalle_versiyon").IsUnique();

            entity.Property(e => e.MahalleId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_mahalle'::regclass)")
                .HasComment("primary key")
                .HasColumnName("mahalle_id");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(4000)
                .HasComment("#Aciklama")
                .HasColumnName("aciklama");
            entity.Property(e => e.Ad)
                .HasMaxLength(2048)
                .HasComment("mahalle adi #MahalleAd")
                .HasColumnName("ad");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasComment("#Aktif")
                .HasColumnName("aktif");
            entity.Property(e => e.BucakKod)
                .HasComment("#BucakKod")
                .HasColumnName("bucak_kod");
            entity.Property(e => e.IlKod)
                .HasComment("#IlKod")
                .HasColumnName("il_kod");
            entity.Property(e => e.IlceKod)
                .HasComment("#IlceKod")
                .HasColumnName("ilce_kod");
            entity.Property(e => e.KoyKod)
                .HasComment("#KoyKod")
                .HasColumnName("koy_kod");
            entity.Property(e => e.MahalleKod)
                .HasComment("Mahalle  kaydinin aks kodu #MahalleKod")
                .HasColumnName("mahalle_kod");
            entity.Property(e => e.MahalleTanimKod)
                .HasMaxLength(64)
                .HasComment("ilcenin uavt kodu #TanimKod")
                .HasColumnName("mahalle_tanim_kod");
            entity.Property(e => e.ResmiGazeteTarihi)
                .HasComment("#ResmiTarih")
                .HasColumnName("resmi_gazete_tarihi");
            entity.Property(e => e.TakbisMahalleKod)
                .HasComment("#TapuMahalleKod")
                .HasColumnName("takbis_mahalle_kod");
            entity.Property(e => e.Versiyon)
                .HasComment("guncel versiyon numarasi")
                .HasColumnName("versiyon");
            entity.Property(e => e.YetkiliIdareKod)
                .HasComment("#YetkiliIdareKod")
                .HasColumnName("yetkili_idare_kod");

            entity.HasOne(d => d.IlKodNavigation).WithMany(p => p.TblMahalles)
                .HasPrincipalKey(p => p.IlKod)
                .HasForeignKey(d => d.IlKod)
                .HasConstraintName("fk_tbl_mahalle_il");

            entity.HasOne(d => d.IlceKodNavigation).WithMany(p => p.TblMahalles)
                .HasPrincipalKey(p => p.IlceKod)
                .HasForeignKey(d => d.IlceKod)
                .HasConstraintName("fk_tbl_mahalle_ilce");

            entity.HasOne(d => d.KoyKodNavigation).WithMany(p => p.TblMahalles)
                .HasPrincipalKey(p => p.KoyKod)
                .HasForeignKey(d => d.KoyKod)
                .HasConstraintName("fk_tbl_mahalle_koy");

            entity.HasOne(d => d.MahalleTanimKodNavigation).WithMany(p => p.TblMahalles)
                .HasPrincipalKey(p => p.TanimKod)
                .HasForeignKey(d => d.MahalleTanimKod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tbl_mahalle_mahalletanim");

            entity.HasOne(d => d.YetkiliIdareKodNavigation).WithMany(p => p.TblMahalles)
                .HasPrincipalKey(p => p.YetkiliIdareKod)
                .HasForeignKey(d => d.YetkiliIdareKod)
                .HasConstraintName("fk_tbl_mahalle_yetkili_idate");
        });

        modelBuilder.Entity<TblNumarataj>(entity =>
        {
            entity.HasKey(e => e.TblNumaratajId).HasName("pk_tbl_numarataj");

            entity.ToTable("tbl_numarataj", "boyut");

            entity.HasIndex(e => e.NumaratajKod, "uk_tbl_numarataj").IsUnique();

            entity.HasIndex(e => e.NumaratajKimlikno, "uk_tbl_numarataj_kimlikno").IsUnique();

            entity.HasIndex(e => e.Versiyon, "uk_tbl_numarataj_versiyon").IsUnique();

            entity.Property(e => e.TblNumaratajId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_numarataj'::regclass)")
                .HasColumnName("tbl_numarataj_id");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif");
            entity.Property(e => e.AnaNumaratajKod).HasColumnName("ana_numarataj_kod");
            entity.Property(e => e.CsbmKod).HasColumnName("csbm_kod");
            entity.Property(e => e.DigerYapiKod).HasColumnName("diger_yapi_kod");
            entity.Property(e => e.KapiNo)
                .HasMaxLength(255)
                .HasColumnName("kapi_no");
            entity.Property(e => e.NumaratajKimlikno).HasColumnName("numarataj_kimlikno");
            entity.Property(e => e.NumaratajKod).HasColumnName("numarataj_kod");
            entity.Property(e => e.NumaratajTipTanimKod)
                .HasMaxLength(64)
                .HasColumnName("numarataj_tip_tanim_kod");
            entity.Property(e => e.Versiyon).HasColumnName("versiyon");
            entity.Property(e => e.YapiKod).HasColumnName("yapi_kod");

            entity.HasOne(d => d.CsbmKodNavigation).WithMany(p => p.TblNumaratajs)
                .HasPrincipalKey(p => p.CsbmKod)
                .HasForeignKey(d => d.CsbmKod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tbl_numarataj_csbm");
        });

        modelBuilder.Entity<TblTanim>(entity =>
        {
            entity.HasKey(e => e.TanimId).HasName("pk_tbl_tanim");

            entity.ToTable("tbl_tanim", "boyut");

            entity.HasIndex(e => e.Aktif, "ix_tbl_tanim_aktif");

            entity.HasIndex(e => e.TanimGrupKod, "ix_tbl_tanim_tanim_grup_kod");

            entity.HasIndex(e => e.TanimKod, "uk_tbl_tanim_kod").IsUnique();

            entity.HasIndex(e => e.Versiyon, "uk_tbl_tanim_versiyon").IsUnique();

            entity.Property(e => e.TanimId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_tanim'::regclass)")
                .HasComment("tablodaki anahtar alan, otomatik artan degerdedir.")
                .HasColumnName("tanim_id");
            entity.Property(e => e.Aciklama)
                .HasMaxLength(2000)
                .HasComment("ek aciklama bilgileri burada tutulmaktadir.")
                .HasColumnName("aciklama");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasComment("ad bilgisini tutmaktadir.")
                .HasColumnName("ad");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasComment("verinin gecerli kullanilabilir veri olma durumu")
                .HasColumnName("aktif");
            entity.Property(e => e.BagliTanimKod)
                .HasMaxLength(64)
                .HasComment("Kendi icersinde topoloji bilgisini tutmaktadir.")
                .HasColumnName("bagli_tanim_kod");
            entity.Property(e => e.EklenmeTarihi)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Tabloya verinin eklendiği tarih")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("eklenme_tarihi");
            entity.Property(e => e.GuncellemeTarihi)
                .HasComment("İlgili satırın en son ne zaman update edildiği bilgisini tutmaktadır.")
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("guncelleme_tarihi");
            entity.Property(e => e.IlKod)
                .HasComment("il tablosundaki il kod alani eslesmketedir. Bazi tanimlarda il gerektigi icin eklendi")
                .HasColumnName("il_kod");
            entity.Property(e => e.IlceKod)
                .HasComment("tbl_ilce ilce_kod alani ile eslemektedir. Bazi enumlarda ilce bilgisi oldugu icin eklendi")
                .HasColumnName("ilce_kod");
            entity.Property(e => e.KaynakTanimKod)
                .HasMaxLength(16)
                .HasComment("Kaynak Sistemdeki Kodu Ornek  MernisKodu")
                .HasColumnName("kaynak_tanim_kod");
            entity.Property(e => e.TanimGrupKod)
                .HasMaxLength(64)
                .HasComment("bagli oldugu tanim grup tablosundaki kaydin kod degeri.")
                .HasColumnName("tanim_grup_kod");
            entity.Property(e => e.TanimKod)
                .HasMaxLength(64)
                .HasComment("Tanim grup kodu, bu tablo ile join olacak tablolar bu alan ile eşleşmelidir.")
                .HasColumnName("tanim_kod");
            entity.Property(e => e.Versiyon)
                .HasComment("guncel versiyon numarasi")
                .HasColumnName("versiyon");

            entity.HasOne(d => d.BagliTanimKodNavigation).WithMany(p => p.InverseBagliTanimKodNavigation)
                .HasPrincipalKey(p => p.TanimKod)
                .HasForeignKey(d => d.BagliTanimKod)
                .HasConstraintName("fk_tbl_tanim_bagli_tanim_kod");
        });

        modelBuilder.Entity<TblYapi>(entity =>
        {
            entity.HasKey(e => e.TblYapiId).HasName("pk_tbl_yapi");

            entity.ToTable("tbl_yapi", "boyut");

            entity.HasIndex(e => e.YapiKod, "uk_tbl_yapi").IsUnique();

            entity.HasIndex(e => e.YapiKimlikno, "uk_tbl_yapi_kimlikno").IsUnique();

            entity.Property(e => e.TblYapiId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_yapi'::regclass)")
                .HasColumnName("tbl_yapi_id");
            entity.Property(e => e.AdaNo)
                .HasMaxLength(255)
                .HasColumnName("ada_no");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("aktif");
            entity.Property(e => e.BlokAdi)
                .HasMaxLength(255)
                .HasColumnName("blok_adi");
            entity.Property(e => e.DisKapiNo)
                .HasMaxLength(255)
                .HasColumnName("dis_kapi_no");
            entity.Property(e => e.Pafta)
                .HasMaxLength(255)
                .HasColumnName("pafta");
            entity.Property(e => e.ParselNo)
                .HasMaxLength(255)
                .HasColumnName("parsel_no");
            entity.Property(e => e.PostaKodu)
                .HasMaxLength(255)
                .HasColumnName("posta_kodu");
            entity.Property(e => e.SiteAdi)
                .HasMaxLength(255)
                .HasColumnName("site_adi");
            entity.Property(e => e.ToplamKatSayisi)
                .HasComputedColumnSql("(yol_alti_kat_sayisi + yol_ustu_kat_sayisi)", true)
                .HasColumnName("toplam_kat_sayisi");
            entity.Property(e => e.Versiyon).HasColumnName("versiyon");
            entity.Property(e => e.YapiDurumTanimKod)
                .HasMaxLength(64)
                .HasColumnName("yapi_durum_tanim_kod");
            entity.Property(e => e.YapiKimlikno).HasColumnName("yapi_kimlikno");
            entity.Property(e => e.YapiKod).HasColumnName("yapi_kod");
            entity.Property(e => e.YapiTipTanimKod)
                .HasMaxLength(64)
                .HasColumnName("yapi_tip_tanim_kod");
            entity.Property(e => e.YolAltiKatSayisi).HasColumnName("yol_alti_kat_sayisi");
            entity.Property(e => e.YolUstuKatSayisi).HasColumnName("yol_ustu_kat_sayisi");

            entity.HasOne(d => d.YapiDurumTanimKodNavigation).WithMany(p => p.TblYapiYapiDurumTanimKodNavigations)
                .HasPrincipalKey(p => p.TanimKod)
                .HasForeignKey(d => d.YapiDurumTanimKod)
                .HasConstraintName("fk_tbl_yapi_yapidurum");

            entity.HasOne(d => d.YapiTipTanimKodNavigation).WithMany(p => p.TblYapiYapiTipTanimKodNavigations)
                .HasPrincipalKey(p => p.TanimKod)
                .HasForeignKey(d => d.YapiTipTanimKod)
                .HasConstraintName("fk_tbl_yapi_yapitip");
        });

        modelBuilder.Entity<TblYetkiliIdare>(entity =>
        {
            entity.HasKey(e => e.TblYetkiliIdareId).HasName("pk_tbl_yetkili_idare");

            entity.ToTable("tbl_yetkili_idare", "boyut");

            entity.HasIndex(e => e.KurumTurKod, "ix_tbl_yetkili_idare_ilce");

            entity.HasIndex(e => e.YetkiliIdareKod, "uk_tbl_yetkili_idare").IsUnique();

            entity.Property(e => e.TblYetkiliIdareId)
                .HasDefaultValueSql("nextval('boyut.seq_tbl_yetkili_idare'::regclass)")
                .HasComment("primary key")
                .HasColumnName("tbl_yetkili_idare_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(255)
                .HasComment("#YerelIdareAd")
                .HasColumnName("ad");
            entity.Property(e => e.Aktif)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasComment("#Aktif")
                .HasColumnName("aktif");
            entity.Property(e => e.IlKod)
                .HasComment("#IlKod")
                .HasColumnName("il_kod");
            entity.Property(e => e.IlceKod)
                .HasComment("#IlceKod")
                .HasColumnName("ilce_kod");
            entity.Property(e => e.KoyKod)
                .HasComment("#KoyKod")
                .HasColumnName("koy_kod");
            entity.Property(e => e.KurumTurKod)
                .HasMaxLength(64)
                .HasComment("#TanimKod")
                .HasColumnName("kurum_tur_kod");
            entity.Property(e => e.Versiyon)
                .HasComment("#VersiyonNo")
                .HasColumnName("versiyon");
            entity.Property(e => e.YetkiliIdareKod)
                .HasComment("#YerelIdareKod")
                .HasColumnName("yetkili_idare_kod");

            entity.HasOne(d => d.IlKodNavigation).WithMany(p => p.TblYetkiliIdares)
                .HasPrincipalKey(p => p.IlKod)
                .HasForeignKey(d => d.IlKod)
                .HasConstraintName("fk_tbl_yetkili_idare_il");

            entity.HasOne(d => d.IlceKodNavigation).WithMany(p => p.TblYetkiliIdares)
                .HasPrincipalKey(p => p.IlceKod)
                .HasForeignKey(d => d.IlceKod)
                .HasConstraintName("fk_tbl_yetkili_idare_ilce");

            entity.HasOne(d => d.KoyKodNavigation).WithMany(p => p.TblYetkiliIdares)
                .HasPrincipalKey(p => p.KoyKod)
                .HasForeignKey(d => d.KoyKod)
                .HasConstraintName("fk_tbl_yetkili_idare_koy");

            entity.HasOne(d => d.KurumTurKodNavigation).WithMany(p => p.TblYetkiliIdares)
                .HasPrincipalKey(p => p.TanimKod)
                .HasForeignKey(d => d.KurumTurKod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tbl_yetkili_idare_kurum_tur_tanim");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
