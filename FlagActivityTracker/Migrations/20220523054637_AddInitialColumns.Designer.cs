﻿// <auto-generated />
using System;
using FlagActivityTracker.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    [DbContext(typeof(FlagActivityTrackerDbContext))]
    [Migration("20220523054637_AddInitialColumns")]
    partial class AddInitialColumns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FlagActivityTracker.Entities.Crew", b =>
                {
                    b.Property<int>("CrewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CrewId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CrewName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FlagId")
                        .HasColumnType("int");

                    b.HasKey("CrewId");

                    b.HasIndex("FlagId");

                    b.ToTable("Crews");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Flag", b =>
                {
                    b.Property<int>("FlagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FlagId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("FlagId");

                    b.ToTable("Flags");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Pirate", b =>
                {
                    b.Property<int>("PirateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PirateId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CrewId")
                        .HasColumnType("int");

                    b.Property<string>("CrewLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlagLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PirateLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PirateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PirateId");

                    b.HasIndex("CrewId");

                    b.ToTable("Pirates");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Crew", b =>
                {
                    b.HasOne("FlagActivityTracker.Entities.Flag", "Flag")
                        .WithMany()
                        .HasForeignKey("FlagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flag");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Pirate", b =>
                {
                    b.HasOne("FlagActivityTracker.Entities.Crew", "Crew")
                        .WithMany()
                        .HasForeignKey("CrewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Crew");
                });
#pragma warning restore 612, 618
        }
    }
}
