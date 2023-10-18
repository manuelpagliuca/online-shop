namespace Ubique.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ICategoryRepository Category { get; }
		ISubCategoryRepository SubCategory { get; }
		void Save();
	}
}
