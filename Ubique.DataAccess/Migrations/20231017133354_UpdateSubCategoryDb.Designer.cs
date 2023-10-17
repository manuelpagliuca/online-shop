﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Ubique.DataAccess.Data;

#nullable disable

namespace Ubique.DataAccess.Migrations
{
	[DbContext(typeof(ApplicationDbContext))]
    [Migration("20231017133354_UpdateSubCategoryDb")]
    partial class UpdateSubCategoryDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Ubique.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Lavabo"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Bidet"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "Cascata"
                        });
                });

            modelBuilder.Entity("Ubique.Models.SubCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("SubCategories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Name = "Monoleva"
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            Name = "Due maniglie"
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            Name = "Termostatico"
                        });
                });

            modelBuilder.Entity("Ubique.Models.SubCategory", b =>
                {
                    b.HasOne("Ubique.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
