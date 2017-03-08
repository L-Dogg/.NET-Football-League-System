using System.Data.Entity;
using FootballManager.Domain.Entity.Models.Authentication;

namespace FootballManager.Domain.Entity.Contexts.AuthenticationContext
{
	public interface IAuthenticationContext : IDbContext
	{
		IDbSet<User> Users { get; set; }

		/// <summary>
		/// Compares two byte arrays.
		/// </summary>
		/// <param name="array1">First array</param>
		/// <param name="array2">Second array</param>
		/// <returns>True if arrays are equal, false otherwise</returns>
		bool CompareByteArrays(byte[] array1, byte[] array2);

		/// <summary>
		/// Generates hash with salt using SHA256 algorithm.
		/// </summary>
		/// <param name="plainText">Byte array of password to be hashed.</param>
		/// <param name="salt">Unique salt.</param>
		/// <returns>Byte array of salted password hash.</returns>
		byte[] GenerateSaltedHash(byte[] plainText, byte[] salt);

		/// <summary>
		/// Performs user authentication.
		/// </summary>
		/// <param name="username">UTF8 username.</param>
		/// <param name="password">UTF8 plaintext password.</param>
		/// <returns>UserID if succeeded, -1 otherwise.</returns>
		int AuthenticateUser(string username, string password, Models.Authentication.Enums.UserType userType = Models.Authentication.Enums.UserType.Admin);

		/// <summary>
		/// Changes user password.
		/// </summary>
		/// <param name="userID">ID of user.</param>
		/// <param name="oldPassword">UTF8 plaintext password.</param>
		/// <param name="newPassword">UTF8 plaintext new password.</param>
		/// <returns><c>True</c> if authentication and password change succeeded,
		/// false otherwise (wrong username/oldPassword combination).</returns>
		bool ChangePassword(int userID, string oldPassword, string newPassword);
	}
}
