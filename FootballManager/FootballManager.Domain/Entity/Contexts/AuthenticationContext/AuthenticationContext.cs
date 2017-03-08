using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FootballManager.Domain.Entity.Models.Authentication;
using FootballManager.Domain.Entity.Models.Authentication.Enums;

namespace FootballManager.Domain.Entity.Contexts.AuthenticationContext
{
	/// <summary>
	/// Context for auth purposes.
	/// </summary>
	public class AuthenticationContext : DbContext, IAuthenticationContext
	{
		public AuthenticationContext()
		{
			Database.SetInitializer(new AuthenticationContextInitializer());
        }

		/// <summary>
		/// Compares two byte arrays.
		/// </summary>
		/// <param name="array1">First array</param>
		/// <param name="array2">Second array</param>
		/// <returns>True if arrays are equal, false otherwise</returns>
		public bool CompareByteArrays(byte[] array1, byte[] array2)
		{
			return array1.Length == array2.Length && !array1.Where((t, i) => t != array2[i]).Any();
		}

		/// <summary>
		/// Generates hash with salt using SHA256 algorithm.
		/// </summary>
		/// <param name="plainText">Byte array of password to be hashed.</param>
		/// <param name="salt">Unique salt.</param>
		/// <returns>Byte array of salted password hash.</returns>
		public byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
		{
			HashAlgorithm algorithm = new SHA256Managed();

			var plainTextWithSaltBytes = new byte[plainText.Length + salt.Length];

			for (var i = 0; i < plainText.Length; i++)
			{
				plainTextWithSaltBytes[i] = plainText[i];
			}
			for (var i = 0; i < salt.Length; i++)
			{
				plainTextWithSaltBytes[plainText.Length + i] = salt[i];
			}

			return algorithm.ComputeHash(plainTextWithSaltBytes);
		}

		/// <summary>
		/// Performs user authentication.
		/// </summary>
		/// <param name="username">UTF8 username.</param>
		/// <param name="password">UTF8 plaintext password.</param>
		/// <param name="userType">Type of user.</param>
		/// <returns>UserID if succeeded, -1 otherwise.</returns>
		public int AuthenticateUser(string username, string password, UserType userType = UserType.Admin)
		{
			foreach (var user in Users.Where(x => x.UserType == userType))
			{
				if (user.Username == username)
				{
					var currentHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(password), user.Salt);
					if (CompareByteArrays(currentHash, user.PasswordHash))
						return user.Id;
				}
			}

			return -1;
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
			var user = Users.First(u => u.Id == userID);

			if (user == null || string.IsNullOrEmpty(newPassword))
				return false;

			var currentHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(oldPassword), user.Salt);
			if (!CompareByteArrays(currentHash, user.PasswordHash))
				return false;

			var rng = new RNGCryptoServiceProvider();
			var salt = new byte[8];
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = GenerateSaltedHash(Encoding.UTF8.GetBytes(newPassword), salt);

			this.SaveChanges();
			return true;
		}

		public virtual IDbSet<User> Users { get; set; }
	}
}
