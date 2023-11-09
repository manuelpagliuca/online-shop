using Ubique.DataAccess.Datza;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.DataAccess.Repository
{
	public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
	{
		private ApplicationDbContext _db;
		public SubCategoryRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(SubCategory subcategory)
		{
			_db.SubCategories.Update(subcategory);
		}
	}
}
