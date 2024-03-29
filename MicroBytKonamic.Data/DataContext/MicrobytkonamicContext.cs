﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using MicroBytKonamic.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MicroBytKonamic.Data.DataContext;

public partial class MicrobytkonamicContext : DbContext
{
    public MicrobytkonamicContext(DbContextOptions<MicrobytkonamicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Filesfortune> Filesfortunes { get; set; }

    public virtual DbSet<Fortune> Fortunes { get; set; }

    public virtual DbSet<Fortunesofday> Fortunesofdays { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Postale> Postales { get; set; }

    public virtual DbSet<Topicsfortune> Topicsfortunes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Filesfortune>(entity =>
        {
            entity.HasKey(e => e.IdFilesFortunes).HasName("PRIMARY");

            entity.ToTable("filesfortunes");

            entity.HasIndex(e => e.Filename, "Filename_UNIQUE").IsUnique();

            entity.HasIndex(e => e.IdLanguages, "filesfortunes_idLanguages_idx");

            entity.HasIndex(e => e.IdTopicsFortunes, "filesfortunes_idTopicsFortunes_idx");

            entity.Property(e => e.IdFilesFortunes).HasColumnName("idFilesFortunes");
            entity.Property(e => e.Filename)
                .IsRequired()
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.IdLanguages).HasColumnName("idLanguages");
            entity.Property(e => e.IdTopicsFortunes).HasColumnName("idTopicsFortunes");

            entity.HasOne(d => d.IdLanguagesNavigation).WithMany(p => p.Filesfortunes)
                .HasForeignKey(d => d.IdLanguages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("filesfortunes_idLanguages");

            entity.HasOne(d => d.IdTopicsFortunesNavigation).WithMany(p => p.Filesfortunes)
                .HasForeignKey(d => d.IdTopicsFortunes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("filesfortunes_idTopicsFortunes");
        });

        modelBuilder.Entity<Fortune>(entity =>
        {
            entity.HasKey(e => e.IdFortunes).HasName("PRIMARY");

            entity.ToTable("fortunes");

            entity.HasIndex(e => e.IdFilesFortunes, "fortunes_idFilesFortunes_idx");

            entity.Property(e => e.IdFortunes).HasColumnName("idFortunes");
            entity.Property(e => e.Fortune1)
                .HasColumnType("text")
                .HasColumnName("Fortune");
            entity.Property(e => e.IdFilesFortunes).HasColumnName("idFilesFortunes");

            entity.HasOne(d => d.IdFilesFortunesNavigation).WithMany(p => p.Fortunes)
                .HasForeignKey(d => d.IdFilesFortunes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fortunes_idFilesFortunes_fx");
        });

        modelBuilder.Entity<Fortunesofday>(entity =>
        {
            entity.HasKey(e => e.IdfortunesOfDay).HasName("PRIMARY");

            entity.ToTable("fortunesofday");

            entity.HasIndex(e => e.IdFortunes, "fortunesofday_idFortunes_idx");

            entity.HasIndex(e => e.IdLanguages, "fortunesofday_idLanguages_idx");

            entity.HasIndex(e => new { e.Day, e.IdLanguages }, "fortunesofday_u1");

            entity.Property(e => e.IdfortunesOfDay).HasColumnName("idfortunesOfDay");
            entity.Property(e => e.Day).HasColumnType("datetime");
            entity.Property(e => e.IdFortunes).HasColumnName("idFortunes");
            entity.Property(e => e.IdLanguages).HasColumnName("idLanguages");

            entity.HasOne(d => d.IdFortunesNavigation).WithMany(p => p.Fortunesofdays)
                .HasForeignKey(d => d.IdFortunes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fortunesofday_idFortunes_fx");

            entity.HasOne(d => d.IdLanguagesNavigation).WithMany(p => p.Fortunesofdays)
                .HasForeignKey(d => d.IdLanguages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fortunesofday_idLanguages_fx");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.IdLanguages).HasName("PRIMARY");

            entity.ToTable("languages");

            entity.HasIndex(e => e.Culture, "Culture_UNIQUE").IsUnique();

            entity.Property(e => e.IdLanguages).HasColumnName("idLanguages");
            entity.Property(e => e.Culture)
                .IsRequired()
                .HasMaxLength(5);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(45)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Postale>(entity =>
        {
            entity.HasKey(e => e.IdPostales).HasName("PRIMARY");

            entity.ToTable("postales");

            entity.Property(e => e.IdPostales).HasColumnName("idPostales");
            entity.Property(e => e.Anyo).HasColumnName("anyo");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Nick)
                .HasMaxLength(50)
                .HasColumnName("nick");
            entity.Property(e => e.Texto)
                .HasColumnType("text")
                .HasColumnName("texto");
        });

        modelBuilder.Entity<Topicsfortune>(entity =>
        {
            entity.HasKey(e => e.IdTopicsFortunes).HasName("PRIMARY");

            entity.ToTable("topicsfortunes");

            entity.HasIndex(e => e.Topic, "Topic_UNIQUE").IsUnique();

            entity.Property(e => e.IdTopicsFortunes).HasColumnName("idTopicsFortunes");
            entity.Property(e => e.Topic)
                .IsRequired()
                .HasMaxLength(45)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}