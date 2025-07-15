using System;
using System.Collections.Generic;
using Csb.YerindeDonusum.Domain.Entities.YapiDenetimSeviye;
using Microsoft.EntityFrameworkCore;

namespace Csb.YerindeDonusum.Persistance.Context;

public partial class YapiDenetimSeviyeDbContext : DbContext
{
    public YapiDenetimSeviyeDbContext()
    {
    }

    public YapiDenetimSeviyeDbContext(DbContextOptions<YapiDenetimSeviyeDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MvMuteahhitYibfList> MvMuteahhitYibfLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:YapiDenetimSeviye", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("postgis")
            .HasPostgresExtension("postgres_fdw");

        modelBuilder.Entity<MvMuteahhitYibfList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("mv_muteahhit_yibf_list");

            entity.Property(e => e.Ada)
                .HasMaxLength(255)
                .HasColumnName("ada");
            entity.Property(e => e.Il)
                .HasMaxLength(100)
                .HasColumnName("il");
            entity.Property(e => e.IlId).HasColumnName("il_id");
            entity.Property(e => e.IlKod)
                .HasMaxLength(255)
                .HasColumnName("il_kod");
            entity.Property(e => e.Ilce)
                .HasMaxLength(100)
                .HasColumnName("ilce");
            entity.Property(e => e.IlceId).HasColumnName("ilce_id");
            entity.Property(e => e.IlceKod)
                .HasMaxLength(255)
                .HasColumnName("ilce_kod");
            entity.Property(e => e.Mahalle)
                .HasMaxLength(100)
                .HasColumnName("mahalle");
            entity.Property(e => e.MahalleId).HasColumnName("mahalle_id");
            entity.Property(e => e.MahalleKod)
                .HasMaxLength(255)
                .HasColumnName("mahalle_kod");
            entity.Property(e => e.MutTicKayitNo)
                .HasMaxLength(255)
                .HasColumnName("mut_tic_kayit_no");
            entity.Property(e => e.Parsel)
                .HasMaxLength(255)
                .HasColumnName("parsel");
            entity.Property(e => e.RuhsatNo)
                .HasMaxLength(255)
                .HasColumnName("ruhsat_no");
            entity.Property(e => e.SantiyeSefi).HasColumnName("santiye_sefi");
            entity.Property(e => e.Seviye).HasColumnName("seviye");
            entity.Property(e => e.YapiMuteahhiti)
                .HasMaxLength(1024)
                .HasColumnName("yapi_muteahhiti");
            entity.Property(e => e.YapiRuhsatOnayTarihi)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("yapi_ruhsat_onay_tarihi");
            entity.Property(e => e.YibfNo).HasColumnName("yibf_no");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
