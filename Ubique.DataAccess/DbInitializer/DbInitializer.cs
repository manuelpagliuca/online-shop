using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ubique.DataAccess.Datza;
using Ubique.Models;
using Ubique.Utility;

namespace Ubique.DataAccess.DbInitializer
{
	public class DbInitializer : IDbInitializer
	{
		public readonly UserManager<IdentityUser> _userManager;
		public readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _db;

		public DbInitializer(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			ApplicationDbContext db
			)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_db = db;
		}

		public void Initialize()
		{
			// push migrations if they are not applied
			try
			{
				if (_db.Database.GetPendingMigrations().Count() > 0)
				{
					_db.Database.Migrate();
				}
			}
			catch (Exception ex) { }

			// create roles if thet are not created + create admin user
			if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Customer).GetAwaiter().GetResult())
			{
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company)).GetAwaiter().GetResult();

				_userManager.CreateAsync(new ApplicationUser
				{
					UserName = "admin@ubique.it",
					Email = "admin@ubique.it",
					Name = "Capo dei capi",
					PhoneNumber = "1234567890",
					StreetAddress = "Via dell vie",
					State = "IT",
					PostalCode = "1234",
					City = "Aronaaaaa"
				}, "Admin123*").GetAwaiter().GetResult();

				ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@ubique.it");
				_userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();
			}
		}
	}
}
