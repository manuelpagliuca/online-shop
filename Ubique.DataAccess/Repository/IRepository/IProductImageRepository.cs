using Ubique.Models;

namespace Ubique.DataAccess.Repository.IRepository
{
	public interface IProductImageRepository : IRepository<ProductImage>
	{
		void Update(ProductImage productImage);
	}
}
