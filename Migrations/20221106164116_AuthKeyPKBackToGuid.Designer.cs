﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StonkBot.StonkBot.Database;

#nullable disable

namespace StonkBot.Migrations
{
    [DbContext(typeof(StonkBotDbContext))]
    [Migration("20221106164116_AuthKeyPKBackToGuid")]
    partial class AuthKeyPKBackToGuid
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("StonkBot.StonkBot.Services.TDAmeritrade.Models.AuthToken", b =>
                {
                    b.Property<Guid>("PrimaryKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("access_token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("expires_in")
                        .HasColumnType("INTEGER");

                    b.Property<string>("refresh_token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("refresh_token_expires_in")
                        .HasColumnType("INTEGER");

                    b.Property<string>("scope")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("tokenCreatedTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("token_type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.ToTable("AuthTokens");
                });

            modelBuilder.Entity("StonkBot.StonkBot.Services.WebScrape.Models.IpoListing", b =>
                {
                    b.Property<Guid>("DbKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExpectedListingDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("OfferAmmount")
                        .HasColumnType("TEXT");

                    b.Property<string>("OfferingEndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("OfferingPrice")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ScrapeDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("DbKey");

                    b.ToTable("IpoListings");
                });
#pragma warning restore 612, 618
        }
    }
}
