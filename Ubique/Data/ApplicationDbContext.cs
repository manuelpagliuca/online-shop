using Microsoft.EntityFrameworkCore;
using Ubique.Models;

namespace Ubique.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<SubCategory> SubCategories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Lavabo", DisplayOrder = 1 },
				new Category { Id = 2, Name = "Bidet", DisplayOrder = 2 },
				new Category { Id = 3, Name = "Lavabo", DisplayOrder = 3 },
				new Category { Id = 4, Name = "Lavabo", DisplayOrder = 4 },
				new Category { Id = 5, Name = "Bidet", DisplayOrder = 5 },
				new Category { Id = 6, Name = "Cascata", DisplayOrder = 6 });

			modelBuilder.Entity<SubCategory>().HasData(
				new SubCategory { Id = 1, Name = "Monoleva", CategoryId = 1, DisplayOrder = 1 },
				new SubCategory { Id = 2, Name = "Due maniglie", CategoryId = 1, DisplayOrder = 2 },
				new SubCategory { Id = 3, Name = "Termostatico", CategoryId = 6, DisplayOrder = 3 }
				);
		}
	}
}
