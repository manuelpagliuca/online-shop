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

	}
}
