using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballManager.Domain.Entity.Models.Authentication.Enums;
using FootballManager.AuthenticationViewModel;
using FootballManager.Domain.Entity.Models.Authentication;

namespace FootballManager.AuthenticationLayer
{
	/// <summary>
	/// Interface for Authentication ViewModels.
	/// </summary>
	public interface IAuthenticationService
	{
		/// <summary>
		/// Performs user authentication.
		/// </summary>
		/// <param name="username">UTF8 username.</param>
		/// <param name="password">UTF8 plaintext password.</param>
		/// <returns>UserID if succeeded, -1 otherwise.</returns>
		int AuthenticateUser(string username, string password, UserType type);

		/// <summary>
		/// Changes user password.
		/// </summary>
		/// <param name="userID">ID of user.</param>
		/// <param name="oldPassword">UTF8 plaintext password.</param>
		/// <param name="newPassword">UTF8 plaintext new password.</param>
		/// <returns><c>True</c> if authentication and password change succeeded,
		/// false otherwise (wrong username/oldPassword combination).</returns>
		bool ChangePassword(int userID, string oldPassword, string newPassword);

		/// <summary>
		/// Disposes context.
		/// </summary>
		void Dispose();

		Task<int> CreateRefereeAccount(string firstName, string lastName);

		MembershipContext ValidateUser(string user, string password);

		Task<User> CreateUser(string username, string password, bool isFacebookUser = false);
	}
}
