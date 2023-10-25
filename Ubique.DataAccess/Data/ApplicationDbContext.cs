﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ubique.Models;

namespace Ubique.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<SubCategory> SubCategories { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Rubinetteria Lavabo", DisplayOrder = 1 },
				new Category { Id = 2, Name = "Rubinetteria Bidet", DisplayOrder = 2 },
				new Category { Id = 3, Name = "Rubinetteria Cascata", DisplayOrder = 3 },
				new Category { Id = 10, Name = "Serramenti Casa", DisplayOrder = 4 });

			modelBuilder.Entity<SubCategory>().HasData(
				new SubCategory { Id = 1, Name = "Lavabo Generico", CategoryId = 1 },
				new SubCategory { Id = 2, Name = "Monoleva", CategoryId = 1 },
				new SubCategory { Id = 3, Name = "Due maniglie", CategoryId = 1 },
				new SubCategory { Id = 4, Name = "Bidet Generico", CategoryId = 2 },
				new SubCategory { Id = 5, Name = "Cascata Generico", CategoryId = 3 },
				new SubCategory { Id = 6, Name = "Termostatico", CategoryId = 3 },
				new SubCategory { Id = 7, Name = "Serramenti Generico", CategoryId = 10 },
				new SubCategory { Id = 8, Name = "Infissi", CategoryId = 10 }
				);

			modelBuilder.Entity<Product>().HasData(
				new Product
				{
					Id = 1,
					Name = "Turbo Compare v1",
					Brand = "Paini",
					Description = "Permette di lavarsi le mani.",
					ListPrice = 100,
					Price = 240,
					SubCategoryId = 2,
					ImageUrl = "\\images\\product\\b7266cec-65ea-47ae-8011-361559e03bbb.jpg"
				},
				new Product
				{
					Id = 2,
					Name = "Turbo Compare v2",
					Brand = "Paini",
					Description = "Permette di lavarsi le mani.",
					ListPrice = 140,
					Price = 340,
					SubCategoryId = 2,
					ImageUrl = "\\images\\product\\cbb0ed0f-6d37-4480-baaa-9ad4c6a7724f.jpg"
				},
				new Product
				{
					Id = 3,
					Name = "Jars 2",
					Brand = "NOBILI",
					Description = "Permette di sporcarsi le mani.",
					ListPrice = 120,
					Price = 440,
					SubCategoryId = 3,
					ImageUrl = "\\images\\product\\d12ad88d-0a26-4610-a6ed-c603728518fe.png"
				});
		}
	}
}