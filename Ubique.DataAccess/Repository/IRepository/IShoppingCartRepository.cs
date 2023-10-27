using Ubique.Models;

namespace Ubique.DataAccess.Repository.IRepository
{
	public interface IShoppingCartRepository : IRepository<ShoppingCart>
	{
		void Update(ShoppingCart obj);
	}
}
