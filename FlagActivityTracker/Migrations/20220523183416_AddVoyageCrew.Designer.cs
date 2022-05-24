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
    [Migration("20220523183416_AddVoyageCrew")]
    partial class AddVoyageCrew
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ErrorCount")
                        .HasColumnType("int");

                    b.Property<int?>("FlagId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("JobbersLastSeen")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastErrorDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastParsedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PPCrewId")
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

                    b.Property<int>("ErrorCount")
                        .HasColumnType("int");

                    b.Property<string>("FlagName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastErrorDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastParsedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PPFlagId")
                        .HasColumnType("int");

                    b.HasKey("FlagId");

                    b.ToTable("Flags");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.JobbingActivity", b =>
                {
                    b.Property<int>("JobbingActivityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("JobbingActivityId"), 1L, 1);

                    b.Property<DateTime>("ActivityDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CrewId")
                        .HasColumnType("int");

                    b.Property<int>("PirateId")
                        .HasColumnType("int");

                    b.Property<int?>("VoyageId")
                        .HasColumnType("int");

                    b.HasKey("JobbingActivityId");

                    b.HasIndex("CrewId");

                    b.HasIndex("PirateId");

                    b.HasIndex("VoyageId");

                    b.ToTable("JobbingActivities");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Pirate", b =>
                {
                    b.Property<int>("PirateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PirateId"), 1L, 1);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CrewId")
                        .HasColumnType("int");

                    b.Property<int>("ErrorCount")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastErrorDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastParsedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PirateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PirateId");

                    b.HasIndex("CrewId");

                    b.ToTable("Pirates");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Voyage", b =>
                {
                    b.Property<int>("VoyageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VoyageId"), 1L, 1);

                    b.Property<int>("CrewId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("VoyageId");

                    b.HasIndex("CrewId");

                    b.ToTable("Voyages");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Crew", b =>
                {
                    b.HasOne("FlagActivityTracker.Entities.Flag", "Flag")
                        .WithMany()
                        .HasForeignKey("FlagId");

                    b.Navigation("Flag");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.JobbingActivity", b =>
                {
                    b.HasOne("FlagActivityTracker.Entities.Crew", "Crew")
                        .WithMany("JobbingActivities")
                        .HasForeignKey("CrewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlagActivityTracker.Entities.Pirate", "Pirate")
                        .WithMany("JobbingActivities")
                        .HasForeignKey("PirateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlagActivityTracker.Entities.Voyage", "Voyage")
                        .WithMany()
                        .HasForeignKey("VoyageId");

                    b.Navigation("Crew");

                    b.Navigation("Pirate");

                    b.Navigation("Voyage");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Pirate", b =>
                {
                    b.HasOne("FlagActivityTracker.Entities.Crew", "Crew")
                        .WithMany()
                        .HasForeignKey("CrewId");

                    b.Navigation("Crew");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Voyage", b =>
                {
                    b.HasOne("FlagActivityTracker.Entities.Crew", "Crew")
                        .WithMany("Voyages")
                        .HasForeignKey("CrewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Crew");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Crew", b =>
                {
                    b.Navigation("JobbingActivities");

                    b.Navigation("Voyages");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Pirate", b =>
                {
                    b.Navigation("JobbingActivities");
                });
#pragma warning restore 612, 618
        }
    }
}
