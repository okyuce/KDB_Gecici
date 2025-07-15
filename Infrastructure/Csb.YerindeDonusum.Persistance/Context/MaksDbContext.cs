using System;
using System.Collections.Generic;
using Csb.YerindeDonusum.Domain.Entities.Maks;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Persistance.Context;

public partial class MaksDbContext : DbContext
{
    public MaksDbContext()
    {
    }

    public MaksDbContext(DbContextOptions<MaksDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Il> Ils { get; set; }

    public virtual DbSet<Ilce> Ilces { get; set; }

    public virtual DbSet<TopluBagimsizbolum> TopluBagimsizbolums { get; set; }

    public virtual DbSet<TopluCsbm> TopluCbsms { get; set; }

    public virtual DbSet<TopluNumarataj> TopluNumaratajs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:Maks", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_repack")
            .HasPostgresExtension("pg_stat_statements")
            .HasPostgresExtension("postgis");

        modelBuilder.Entity<Il>(entity =>
        {
            entity.HasKey(e => e.Seqid).HasName("il_pkey");

            entity.ToTable("il");

            entity.HasIndex(e => e.Ad, "il_ad_IDX");

            entity.HasIndex(e => e.Geom, "il_geom_idx").HasMethod("gist");

            entity.HasIndex(e => e.Id, "il_id_IDX");

            entity.HasIndex(e => e.Kimlikno, "il_kimlikno_IDX");

            entity.HasIndex(e => e.Kimlikno, "il_kimlikno_key").IsUnique();

            entity.HasIndex(e => e.Seqid, "il_seqid_key").IsUnique();

            entity.Property(e => e.Seqid).HasColumnName("seqid");
            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .HasColumnName("ad");
            entity.Property(e => e.Degistiren).HasColumnName("degistiren");
            entity.Property(e => e.Degistirmetarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("degistirmetarihi");
            entity.Property(e => e.Gecerliliktarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("gecerliliktarihi");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(Geometry,4326)")
                .HasColumnName("geom");
            entity.Property(e => e.Globalid).HasColumnName("globalid");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ihtilafdurumu).HasColumnName("ihtilafdurumu");
            entity.Property(e => e.Ihtilafnedeni).HasColumnName("ihtilafnedeni");
            entity.Property(e => e.Islem)
                .HasMaxLength(1)
                .HasColumnName("islem");
            entity.Property(e => e.Islemtarih)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("islemtarih");
            entity.Property(e => e.Kimlikno).HasColumnName("kimlikno");
            entity.Property(e => e.Kuruluskararsayisi)
                .HasMaxLength(128)
                .HasColumnName("kuruluskararsayisi");
            entity.Property(e => e.Kuruluskarartarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("kuruluskarartarihi");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Olusturan).HasColumnName("olusturan");
            entity.Property(e => e.Olusturmatarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("olusturmatarihi");
            entity.Property(e => e.Operationdescription)
                .HasMaxLength(255)
                .HasColumnName("operationdescription");
            entity.Property(e => e.Operationnumber).HasColumnName("operationnumber");
            entity.Property(e => e.SeAnnoCadData).HasColumnName("se_anno_cad_data");
            entity.Property(e => e.ShapeArea).HasColumnName("shape_area");
            entity.Property(e => e.ShapeLength).HasColumnName("shape_length");
            entity.Property(e => e.Ulkeid).HasColumnName("ulkeid");
            entity.Property(e => e.Verikaynagi).HasColumnName("verikaynagi");
            entity.Property(e => e.Versiyon).HasColumnName("versiyon");
        });

        modelBuilder.Entity<Ilce>(entity =>
        {
            entity.HasKey(e => e.Seqid).HasName("ilce_pkey");

            entity.ToTable("ilce");

            entity.HasIndex(e => e.Ad, "ilce_ad_IDX");

            entity.HasIndex(e => e.Geom, "ilce_geom_idx").HasMethod("gist");

            entity.HasIndex(e => e.Id, "ilce_id_IDX");

            entity.HasIndex(e => e.Ilid, "ilce_ilid_IDX");

            entity.HasIndex(e => e.Kimlikno, "ilce_kimlikno_IDX");

            entity.HasIndex(e => e.Kimlikno, "ilce_kimlikno_key").IsUnique();

            entity.HasIndex(e => e.Seqid, "ilce_seqid_key").IsUnique();

            entity.Property(e => e.Seqid).HasColumnName("seqid");
            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .HasColumnName("ad");
            entity.Property(e => e.Degistiren).HasColumnName("degistiren");
            entity.Property(e => e.Degistirmetarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("degistirmetarihi");
            entity.Property(e => e.Gecerliliktarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("gecerliliktarihi");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(Geometry,4326)")
                .HasColumnName("geom");
            entity.Property(e => e.Globalid).HasColumnName("globalid");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ihtilafdurumu).HasColumnName("ihtilafdurumu");
            entity.Property(e => e.Ihtilafnedeni).HasColumnName("ihtilafnedeni");
            entity.Property(e => e.Ilid).HasColumnName("ilid");
            entity.Property(e => e.Islem)
                .HasMaxLength(1)
                .HasColumnName("islem");
            entity.Property(e => e.Islemtarih)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("islemtarih");
            entity.Property(e => e.Kimlikno).HasColumnName("kimlikno");
            entity.Property(e => e.Kuruluskararsayisi)
                .HasMaxLength(128)
                .HasColumnName("kuruluskararsayisi");
            entity.Property(e => e.Kuruluskarartarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("kuruluskarartarihi");
            entity.Property(e => e.Merkezilcemi).HasColumnName("merkezilcemi");
            entity.Property(e => e.Objectid).HasColumnName("objectid");
            entity.Property(e => e.Olusturan).HasColumnName("olusturan");
            entity.Property(e => e.Olusturmatarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("olusturmatarihi");
            entity.Property(e => e.Operationdescription)
                .HasMaxLength(255)
                .HasColumnName("operationdescription");
            entity.Property(e => e.Operationnumber).HasColumnName("operationnumber");
            entity.Property(e => e.SeAnnoCadData).HasColumnName("se_anno_cad_data");
            entity.Property(e => e.ShapeArea).HasColumnName("shape_area");
            entity.Property(e => e.ShapeLength).HasColumnName("shape_length");
            entity.Property(e => e.Uavtmerkezbucakkodu).HasColumnName("uavtmerkezbucakkodu");
            entity.Property(e => e.Uavtmerkezkoykodu).HasColumnName("uavtmerkezkoykodu");
            entity.Property(e => e.Verikaynagi).HasColumnName("verikaynagi");
            entity.Property(e => e.Versiyon).HasColumnName("versiyon");
        });

        modelBuilder.Entity<TopluBagimsizbolum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("toplu_bagimsizbolum");

            entity.Property(e => e.BagNo).HasColumnName("bag_no");
            entity.Property(e => e.Bagimsizbolumno)
                .HasMaxLength(30)
                .HasColumnName("bagimsizbolumno");
            entity.Property(e => e.Durum)
                .HasMaxLength(1)
                .HasColumnName("durum");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(Geometry,4326)")
                .HasColumnName("geom");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.Iladi)
                .HasMaxLength(50)
                .HasColumnName("iladi");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.Ilceadi)
                .HasMaxLength(50)
                .HasColumnName("ilceadi");
            entity.Property(e => e.Kullanimalttur).HasColumnName("kullanimalttur");
            entity.Property(e => e.Kullanimturu).HasColumnName("kullanimturu");
            entity.Property(e => e.MahId).HasColumnName("mah_id");
            entity.Property(e => e.Mahalleadi)
                .HasMaxLength(50)
                .HasColumnName("mahalleadi");
            entity.Property(e => e.NumaratajNo).HasColumnName("numarataj_no");
            entity.Property(e => e.NumaratajTip).HasColumnName("numarataj_tip");
            entity.Property(e => e.Uavtkodu).HasColumnName("uavtkodu");
            entity.Property(e => e.YapiNo).HasColumnName("yapi_no");
            entity.Property(e => e.YapiTip).HasColumnName("yapi_tip");
            entity.Property(e => e.YohId).HasColumnName("yoh_id");
            entity.Property(e => e.Yoladi)
                .HasMaxLength(100)
                .HasColumnName("yoladi");
        });

        modelBuilder.Entity<TopluCsbm>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("toplu_cbsm");

            entity.Property(e => e.Geom)
                .HasColumnType("geometry(Geometry,4326)")
                .HasColumnName("geom");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.Iladi)
                .HasMaxLength(50)
                .HasColumnName("iladi");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.Ilceadi)
                .HasMaxLength(50)
                .HasColumnName("ilceadi");
            entity.Property(e => e.MahId).HasColumnName("mah_id");
            entity.Property(e => e.Mahalleadi)
                .HasMaxLength(50)
                .HasColumnName("mahalleadi");
            entity.Property(e => e.Tip).HasColumnName("tip");
            entity.Property(e => e.Uavtkodu).HasColumnName("uavtkodu");
            entity.Property(e => e.YohId).HasColumnName("yoh_id");
            entity.Property(e => e.Yoladi)
                .HasMaxLength(100)
                .HasColumnName("yoladi");
        });

        modelBuilder.Entity<TopluNumarataj>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("toplu_numarataj");

            entity.Property(e => e.BinaNo).HasColumnName("bina_no");
            entity.Property(e => e.Geom)
                .HasColumnType("geometry(Geometry,4326)")
                .HasColumnName("geom");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.Iladi)
                .HasMaxLength(50)
                .HasColumnName("iladi");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.Ilceadi)
                .HasMaxLength(50)
                .HasColumnName("ilceadi");
            entity.Property(e => e.Kapino)
                .HasMaxLength(16)
                .HasColumnName("kapino");
            entity.Property(e => e.MahId).HasColumnName("mah_id");
            entity.Property(e => e.Mahalleadi)
                .HasMaxLength(50)
                .HasColumnName("mahalleadi");
            entity.Property(e => e.NumaratajNo).HasColumnName("numarataj_no");
            entity.Property(e => e.Tip).HasColumnName("tip");
            entity.Property(e => e.Uavtkodu).HasColumnName("uavtkodu");
            entity.Property(e => e.YohId).HasColumnName("yoh_id");
            entity.Property(e => e.Yoladi)
                .HasMaxLength(100)
                .HasColumnName("yoladi");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
