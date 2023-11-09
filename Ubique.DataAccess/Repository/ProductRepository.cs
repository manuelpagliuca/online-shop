using Ubique.DataAccess.Datza;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.DataAccess.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
		private ApplicationDbContext _db;

		public ProductRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Product product)
		{
			var objFromDb = _db.Products.FirstOrDefault(u => u.Id == product.Id);

			if (objFromDb != null)
			{
				objFromDb.Name = product.Name;
				objFromDb.Brand = product.Brand;
				objFromDb.Description = product.Description;
				objFromDb.Price = product.Price;
				objFromDb.ListPrice = product.ListPrice;
				objFromDb.SubCategoryId = product.SubCategoryId;
				objFromDb.ProductImages = product.ProductImages;
			}
		}
	}
}