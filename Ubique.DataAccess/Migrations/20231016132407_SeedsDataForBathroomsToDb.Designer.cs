﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Ubique.DataAccess.Data;

#nullable disable

namespace Ubique.DataAccess.Migrations
{
	[DbContext(typeof(ApplicationDbContext))]
    [Migration("20231016132407_SeedsDataForBathroomsToDb")]
    partial class SeedsDataForBathroomsToDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ubique.Models.Bath", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BathComponents");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Rubinetteria Lavabo"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Rubinetteria Bidet"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "Rubinetti Giardino"
                        },
                        new
                        {
                            Id = 4,
                            DisplayOrder = 4,
                            Name = "Rubinetti per Camper e Barche"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}