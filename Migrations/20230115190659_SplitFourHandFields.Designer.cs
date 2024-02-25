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
    [Migration("20230115190659_SplitFourHandFields")]
    partial class SplitFourHandFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("StonkBot.StonkBot.Database.Entities.IndustryAnalysis.IAEtfInfo", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Symbol");

                    b.ToTable("IAEtfInfo");
                });

            modelBuilder.Entity("StonkBot.StonkBot.Database.Entities.IndustryAnalysis.IAHistoricalData", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Datetime")
                        .HasColumnType("TEXT");

                    b.Property<string>("AboveUpperShadow")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Close")
                        .HasColumnType("TEXT");

                    b.Property<string>("FHTargetDay")
                        .HasColumnType("TEXT");

                    b.Property<string>("FiveDayStable")
                        .HasColumnType("TEXT");

                    b.Property<string>("FromYesterday")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("High")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastFHTarget")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Low")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Open")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("UpToday")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("UpperShadow")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Volume")
                        .HasColumnType("TEXT");

                    b.Property<string>("VolumeAlert")
                        .HasColumnType("TEXT");

                    b.Property<string>("VolumeAlert2")
                        .HasColumnType("TEXT");

                    b.HasKey("Symbol", "Datetime");

                    b.ToTable("IAHistoricalData");
                });

            modelBuilder.Entity("StonkBot.StonkBot.Database.Entities.IndustryAnalysis.IAInfo", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("AvgVolume")
                        .HasColumnType("TEXT");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GICSSector")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("InDIA")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InIWM")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InQQQ")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InSpy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Industry")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("MarketCap")
                        .HasColumnType("TEXT");

                    b.HasKey("Symbol");

                    b.ToTable("IAInfo");
                });

            modelBuilder.Entity("StonkBot.StonkBot.Services.TDAmeritrade.Models.AuthToken", b =>
                {
                    b.Property<Guid>("PrimaryKey")
                        .ValueGeneratedOnAdd()
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

                    b.HasKey("PrimaryKey");

                    b.ToTable("AuthTokens");
                });

            modelBuilder.Entity("StonkBot.StonkBot.Services.WebScrape.Models.IpoListing", b =>
                {
                    b.Property<string>("Symbol")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Close")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DiscordMessageDate")
                        .HasColumnType("TEXT");

                    b.Property<ulong?>("DiscordMessageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExpectedListingDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("High")
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Low")
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

                    b.Property<decimal?>("Open")
                        .HasColumnType("TEXT");

                    b.Property<string>("ScrapeDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal?>("Volume")
                        .HasColumnType("TEXT");

                    b.HasKey("Symbol");

                    b.ToTable("IpoListings");
                });
#pragma warning restore 612, 618
        }
    }
}
