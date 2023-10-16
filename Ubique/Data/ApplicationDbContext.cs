using Microsoft.EntityFrameworkCore;
using Ubique.Models;

namespace Ubique.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Bath> BathComponents { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Bath>().HasData(
				new Bath { Id = 1, Name = "Rubinetteria Lavabo", DisplayOrder = 1 },
				new Bath { Id = 2, Name = "Rubinetteria Bidet", DisplayOrder = 2 },
				new Bath { Id = 3, Name = "Rubinetti Giardino", DisplayOrder = 3 },
				new Bath { Id = 4, Name = "Rubinetti per Camper e Barche", DisplayOrder = 4 });

			base.OnModelCreating(modelBuilder);
		}

	}
}
