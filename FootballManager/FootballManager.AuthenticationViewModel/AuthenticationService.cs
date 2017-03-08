using System;
using System.Text;
using FootballManager.Domain.Entity.Contexts.AuthenticationContext;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FootballManager.Domain.Entity.Models.Authentication.Enums;
using FootballManager.AuthenticationViewModel;
using System.Linq;
using System.Security.Principal;
using FootballManager.Domain.Entity.Models.Authentication;

namespace FootballManager.AuthenticationLayer
{
	/// <summary>
	/// View model for authentication purposes.
	/// </summary>
	public class AuthenticationService : IAuthenticationService
    {
		/// <summary>
		/// Authentication context.
		/// </summary>
	    private readonly IAuthenticationContext _authenticationContext;

		/// <summary>
		/// AuthenticationService constructor.
		/// </summary>
		/// <param name="context">Proper Authentication context.</param>
		public AuthenticationService(IAuthenticationContext context)
	    {
		    this._authenticationContext = context;
	    }

		/// <summary>
		/// Performs user authentication.
		/// </summary>
		/// <param name="username">UTF8 username.</param>
		/// <param name="password">UTF8 plaintext password.</param>
		/// <param name="type">User type.</param>
		/// <returns>UserID if succeeded, -1 otherwise.</returns>
		public int AuthenticateUser(string username, string password, UserType type = UserType.Admin)
	    {
		    return _authenticationContext.AuthenticateUser(username, password, type);
	    }

		/// <summary>
		/// Changes user password.
		/// </summary>
		/// <param name="userID">ID of user.</param>
		/// <param name="oldPassword">UTF8 plaintext password.</param>
		/// <param name="newPassword">UTF8 plaintext new password.</param>
		/// <returns><c>True</c> if authentication and password change succeeded,
		/// false otherwise (wrong username/oldPassword combination).</returns>
		public bool ChangePassword(int userID, string oldPassword, string newPassword)
		{
			return _authenticationContext.ChangePassword(userID, oldPassword, newPassword);
		}

		public async Task<int> CreateRefereeAccount(string firstName, string lastName)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(lastName);
			sb.Append(firstName[0]);
			var username = sb.ToString().ToLower();
			var nextUsername = username;
			var usernameExists = false;
			int i = 2;
			do
			{
				usernameExists = false;
				foreach (var user in this._authenticationContext.Users)
				{
					if(user.Username == nextUsername)
					{
						usernameExists = true;
						break;
					}
				}
				if(usernameExists)
				{
					nextUsername = username + i.ToString();
					i++;
				}
			} while (usernameExists);

			var rng = new RNGCryptoServiceProvider();
			var salt = new byte[8];
			rng.GetBytes(salt);
			var newUser = new User()
			{
				Username = nextUsername,
				Salt = salt,
				PasswordHash = this._authenticationContext.GenerateSaltedHash(Encoding.UTF8.GetBytes("password"), salt),
				UserType = UserType.Referee
			};
			this._authenticationContext.Users.Add(newUser);
			await this._authenticationContext.SaveChangesAsync();
			return newUser.Id;
		}

		public MembershipContext ValidateUser(string username, string password)
		{
			var membershipCtx = new MembershipContext();

			var user = this._authenticationContext.Users.Single( x => x.Username == username);//_userRepository.GetSingleByUsername(username);
			if (user != null && this.AuthenticateUser(username,password, UserType.User) > 0)
			{
				//var userRoles = GetUserRoles(user.Username);
				membershipCtx.User = user;

				var identity = new GenericIdentity(user.Username);
				membershipCtx.Principal = new GenericPrincipal(
					identity,
					new string[] { "User" });
			}

			return membershipCtx;
		}

	    public async Task<User> CreateUser(string username, string password, bool isFacebookUser = false)
	    {
		    var existingUser = _authenticationContext.Users.Any(x => x.Username == username);

			if (existingUser)
		    {
				if (isFacebookUser)
				{
					return null;
				}
				else
				{
					throw new Exception("Username is already in use");
				}
		    }

			var rng = new RNGCryptoServiceProvider();
			var salt = new byte[8];
			rng.GetBytes(salt);
			var newUser = new User()
			{
				Username = username,
				Salt = salt,
				PasswordHash = this._authenticationContext.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), salt),
				UserType = UserType.User,
				IsFacebookUser = isFacebookUser
			};
			this._authenticationContext.Users.Add(newUser);
			await this._authenticationContext.SaveChangesAsync();

			return newUser;
	    }

		/// <summary>
		/// Disposes context.
		/// </summary>
		public void Dispose()
	    {
		    this._authenticationContext.Dispose();
	    }
    }
}
