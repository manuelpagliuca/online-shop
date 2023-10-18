using Ubique.Models;

namespace Ubique.DataAccess.Repository.IRepository
{
	public interface IProductRepository : IRepository<Product>
	{
		void Update(Product product);

	}
}
