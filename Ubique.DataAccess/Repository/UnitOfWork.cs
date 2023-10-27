using Ubique.DataAccess.Data;
using Ubique.DataAccess.Repository.IRepository;

namespace Ubique.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private ApplicationDbContext _db;
		public ICategoryRepository Category { get; private set; }
		public ISubCategoryRepository SubCategory { get; private set; }
		public IProductRepository Product { get; private set; }
		public ICompanyRepository Company { get; private set; }
		public IShoppingCartRepository ShoppingCart { get; private set; }

		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			Category = new CategoryRepository(_db);
			SubCategory = new SubCategoryRepository(_db);
			Product = new ProductRepository(_db);
			Company = new CompanyRepository(_db);
			ShoppingCart = new ShoppingCartRepository(_db);
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
