using Microsoft.EntityFrameworkCore;
using Ubique.Models;

namespace Ubique.DataAccess.Data
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
				new Category { Id = 3, Name = "Cascata", DisplayOrder = 3 });

			modelBuilder.Entity<SubCategory>().HasData(
				new SubCategory { Id = 1, Name = "Monoleva", CategoryId = 1 },
				new SubCategory { Id = 2, Name = "Due maniglie", CategoryId = 1 },
				new SubCategory { Id = 3, Name = "Termostatico", CategoryId = 3 });
		}
	}
}
