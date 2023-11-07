using Ubique.Models;

namespace Ubique.DataAccess.Repository.IRepository
{
	public interface IApplicationUserRepository : IRepository<ApplicationUser>
	{
		public void Update(ApplicationUser applicationUser);
	}
}