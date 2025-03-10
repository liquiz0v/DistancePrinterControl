﻿// <auto-generated />
using DistancePrinterControl.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DistancePrinterControl.Database.Migrations
{
    [DbContext(typeof(DistancePrinterControlContext))]
    partial class DistancePrinterControlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DistancePrinterControl.Database.Models.Printer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PrinterUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Printers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PrinterUrl = "http://192.168.1.125:5000"
                        },
                        new
                        {
                            Id = 2,
                            PrinterUrl = "http://192.168.1.125:5000"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
