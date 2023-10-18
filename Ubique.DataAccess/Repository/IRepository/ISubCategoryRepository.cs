using Ubique.Models;

namespace Ubique.DataAccess.Repository.IRepository
{
	public interface ISubCategoryRepository : IRepository<SubCategory>
	{
		void Update(SubCategory subCategory);
	}
}
