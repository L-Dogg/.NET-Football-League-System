using FootballManager.Domain.Entity.Models.Authentication.Enums;

namespace FootballManager.Domain.Entity.Models.Authentication
{
	/// <summary>
	/// User entity.
	/// </summary>
	public class User : Entity
	{
		/// <summary>
		/// Username.
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// SHA-256 salted hash.
		/// </summary>
		public byte[] PasswordHash { get; set; }
		/// <summary>
		/// Salt for password hash, unique for every entry.
		/// </summary>
		public byte[] Salt { get; set; }
		/// <summary>
		/// Type of user.
		/// </summary>
		public UserType UserType { get; set; }
		/// <summary>
		/// True if user was registered via facebook account.
		/// </summary>
		public bool IsFacebookUser { get; set; }
	}
}
