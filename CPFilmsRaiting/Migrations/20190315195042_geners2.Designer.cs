﻿// <auto-generated />
using CPFilmsRaiting.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CPFilmsRaiting.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190315195042_geners2")]
    partial class geners2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CPFilmsRaiting.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Role");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CPFilmsRaiting.Models.CommentModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("FilmId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CPFilmsRaiting.Models.FilmModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<double>("IMDbRaiting");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PosterURL")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Films");
                });

            modelBuilder.Entity("CPFilmsRaiting.Models.GenreModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FilmId")
                        .IsRequired();

                    b.Property<string>("Genre")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("CPFilmsRaiting.Models.RaitingModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FilmId");

                    b.Property<string>("UserId");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.HasIndex("FilmId");

                    b.HasIndex("UserId");

                    b.ToTable("Raiting");
                });

            modelBuilder.Entity("CPFilmsRaiting.Models.CommentModel", b =>
                {
                    b.HasOne("CPFilmsRaiting.Models.FilmModel", "Film")
                        .WithMany("Comments")
                        .HasForeignKey("FilmId");

                    b.HasOne("CPFilmsRaiting.Models.ApplicationUser", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("CPFilmsRaiting.Models.GenreModel", b =>
                {
                    b.HasOne("CPFilmsRaiting.Models.FilmModel", "Film")
                        .WithMany("Genres")
                        .HasForeignKey("FilmId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CPFilmsRaiting.Models.RaitingModel", b =>
                {
                    b.HasOne("CPFilmsRaiting.Models.FilmModel", "Film")
                        .WithMany("Raitings")
                        .HasForeignKey("FilmId");

                    b.HasOne("CPFilmsRaiting.Models.ApplicationUser", "User")
                        .WithMany("Raitings")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
