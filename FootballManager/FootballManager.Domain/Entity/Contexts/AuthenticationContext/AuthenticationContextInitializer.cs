using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using FootballManager.Domain.Entity.Models.Authentication;
using FootballManager.Domain.Entity.Models.Authentication.Enums;

namespace FootballManager.Domain.Entity.Contexts.AuthenticationContext
{
	/// <summary>
	/// Context initializer with example users who can log into the application.
	/// </summary>
	public class AuthenticationContextInitializer : DropCreateDatabaseIfModelChanges<AuthenticationContext>
	{
		/// <summary>
		/// Generates example users for authentication.
		/// </summary>
		/// <param name="context">Authentication context.</param>
		protected override void Seed(AuthenticationContext context)
		{
			var rng = new RNGCryptoServiceProvider();
            var user = new User() {Username = "admin"};
			var salt = new byte[8];
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = context.GenerateSaltedHash(Encoding.UTF8.GetBytes("admin"), salt);
			user.UserType = UserType.Admin;
			user.IsFacebookUser = false;
			context.Users.Add(user);

			user = new User() { Username = "marciniaks" };
			salt = new byte[8];
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = context.GenerateSaltedHash(Encoding.UTF8.GetBytes("password"), salt);
			user.UserType = UserType.Referee;
			user.IsFacebookUser = false;
			context.Users.Add(user);

			user = new User() { Username = "colinap" };
			salt = new byte[8];
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = context.GenerateSaltedHash(Encoding.UTF8.GetBytes("password"), salt);
			user.UserType = UserType.Referee;
			user.IsFacebookUser = false;
			context.Users.Add(user);

			user = new User() {Username = "password"};
			salt = new byte[8];
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = context.GenerateSaltedHash(Encoding.UTF8.GetBytes("user"), salt);
			user.UserType = UserType.Referee;
			user.IsFacebookUser = false;
			context.Users.Add(user);

			user = new User() { Username = "testUser" };
			salt = new byte[8];
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = context.GenerateSaltedHash(Encoding.UTF8.GetBytes("ThisIsVeryLongAndComplicatedPassword"), salt);
			user.UserType = UserType.User;
			user.IsFacebookUser = false;
			context.Users.Add(user);

			context.SaveChanges();
			base.Seed(context);
		}
	}
}
