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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Bagno", DisplayOrder = 1 },
				new Category { Id = 2, Name = "Serramenti", DisplayOrder = 2 },
				new Category { Id = 3, Name = "Piastrelle", DisplayOrder = 3 },
				new Category { Id = 4, Name = "Varie", DisplayOrder = 4 }
				);

			base.OnModelCreating(modelBuilder);
		}

	}
}
