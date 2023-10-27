namespace Ubique.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ICategoryRepository Category { get; }
		ISubCategoryRepository SubCategory { get; }
		IProductRepository Product { get; }
		ICompanyRepository Company { get; }
		IShoppingCartRepository ShoppingCart { get; }
		void Save();
	}
}
