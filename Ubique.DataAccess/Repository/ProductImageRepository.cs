using Ubique.DataAccess.Datza;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.DataAccess.Repository
{
	public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
	{
		private ApplicationDbContext _db;
		public ProductImageRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(ProductImage productImage)
		{
			_db.ProductImages.Update(productImage);
		}
	}
}
