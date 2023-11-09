using Ubique.DataAccess.Datza;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;

namespace Ubique.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company obj)
        {
            
        }
    }
}
