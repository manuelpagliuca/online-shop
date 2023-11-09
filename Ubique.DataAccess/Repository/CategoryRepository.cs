using Ubique.DataAccess.Datza;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.DataAccess.Repository
{
	public class CategoryRepository : Repository<Category>, ICategoryRepository
	{
		private ApplicationDbContext _db;
		public CategoryRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Category category)
		{
			_db.Categories.Update(category);
		}
	}
}
