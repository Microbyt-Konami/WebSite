using System;
using System.Collections.Generic;
using System.Configuration;
using MicroBytKonamic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace MicroBytKonamic.Data.DataContext;

public partial class MicrobytkonamicContext : DbContext
{
    private readonly IConfiguration _configuration;

    public MicrobytkonamicContext(DbContextOptions<MicrobytkonamicContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Postale> Postales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseMySql("server=localhost;user=microbytkonamic;password=&M0z00s$;database=microbytkonamic", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));
        => optionsBuilder.UseMySql(_configuration.GetConnectionString("MicroBytKonamic"), ServerVersion.Parse("8.0.35-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Postale>(entity =>
        {
            entity.HasKey(e => e.IdPostales).HasName("PRIMARY");

            entity.ToTable("postales");

            entity.Property(e => e.IdPostales).HasColumnName("idPostales");
            entity.Property(e => e.Anyo).HasColumnName("anyo");
            entity.Property(e => e.Nick)
                .HasMaxLength(50)
                .HasColumnName("nick");
            entity.Property(e => e.Texto)
                .HasColumnType("text")
                .HasColumnName("texto");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
