﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zarinpal.Data.Context;

namespace ZarinPalPayment.Data.Migrations
{
    [DbContext(typeof(Zarinpal_Db_Context))]
    [Migration("20210227141806_mig_Init")]
    partial class mig_Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Zarinpal.Data.Entities.Payment", b =>
                {
                    b.Property<long>("PaymentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RequestDatetime")
                        .HasColumnType("datetime2");

                    b.Property<int>("StatusID")
                        .HasColumnType("int");

                    b.Property<string>("TerminalID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TerminalReference")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("additionalData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("amount")
                        .HasColumnType("bigint");

                    b.Property<string>("callBackUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentID");

                    b.ToTable("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
