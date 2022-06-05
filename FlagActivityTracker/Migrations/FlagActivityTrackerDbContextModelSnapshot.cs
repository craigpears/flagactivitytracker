﻿// <auto-generated />
using System;
using FlagActivityTracker.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlagActivityTracker.Migrations
{
    [DbContext(typeof(FlagActivityTrackerDbContext))]
    partial class FlagActivityTrackerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FlagId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("JobbersLastSeen")
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

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlagName")
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("FlagActivityTracker.Entities.PageScrape", b =>
                {
                    b.Property<int>("PageScrapeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PageScrapeId"), 1L, 1);

                    b.Property<int>("Attempts")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DownloadedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DownloadedHtml")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EntityId")
                        .HasColumnType("int");

                    b.Property<string>("EntityName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PageType")
                        .HasColumnType("int");

                    b.Property<string>("PageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Processed")
                        .HasColumnType("bit");

                    b.Property<string>("ProcessingErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PuzzlePiratesId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PageScrapeId");

                    b.ToTable("PageScrapes");
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

                    b.Property<DateTime?>("DeletedDate")
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

            modelBuilder.Entity("FlagActivityTracker.Entities.Skill", b =>
                {
                    b.Property<int>("SkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SkillId"), 1L, 1);

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<int>("PirateId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("SkillName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SkillType")
                        .HasColumnType("int");

                    b.HasKey("SkillId");

                    b.HasIndex("PirateId");

                    b.ToTable("Skill");
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
                        .WithMany("JobbingActivities")
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

            modelBuilder.Entity("FlagActivityTracker.Entities.Skill", b =>
                {
                    b.HasOne("FlagActivityTracker.Entities.Pirate", "Pirate")
                        .WithMany("Skills")
                        .HasForeignKey("PirateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pirate");
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

                    b.Navigation("Skills");
                });

            modelBuilder.Entity("FlagActivityTracker.Entities.Voyage", b =>
                {
                    b.Navigation("JobbingActivities");
                });
#pragma warning restore 612, 618
        }
    }
}
