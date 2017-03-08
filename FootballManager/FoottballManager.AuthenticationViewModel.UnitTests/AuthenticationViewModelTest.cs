using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using FootballManager.Domain.Entity.Contexts.AuthenticationContext;
using FootballManager.Domain.Entity.Models.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace FoottballManager.AuthenticationService.UnitTests
{
	/// <summary>
	/// Class for testing authentication.
	/// </summary>
	[TestClass]
	public class AuthenticationTest
	{
		/// <summary>
		/// Authentication Context Mock.
		/// </summary>
		private Mock<AuthenticationContext> context;

		private FootballManager.AuthenticationLayer.AuthenticationService authenticationService;
		/// <summary>
		/// List of users.
		/// </summary>
		private List<User> UsersList { get; set; } = new List<User>();

		/// <summary>
		/// Test initialization.
		/// </summary>
		[TestInitialize]
		public void Initialize()
		{
			var contextMock = new Mock<AuthenticationContext>();

			var rng = new RNGCryptoServiceProvider();
			var salt = new byte[8];
			var id = 1;

			#region User Creation
			var user = new User() { Username = "admin", Id = id++};
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = GenerateSaltedHash(Encoding.UTF8.GetBytes("admin"), salt);
			user.UserType = FootballManager.Domain.Entity.Models.Authentication.Enums.UserType.Admin;
			UsersList.Add(user);

			user = new User() { Username = "password", Id = id++ };
			salt = new byte[8];
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = GenerateSaltedHash(Encoding.UTF8.GetBytes("user"), salt);
			user.UserType = FootballManager.Domain.Entity.Models.Authentication.Enums.UserType.Admin;
			UsersList.Add(user);

			user = new User() { Username = "testUser", Id = id++ };
			salt = new byte[8];
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = GenerateSaltedHash(Encoding.UTF8.GetBytes("ThisIsVeryLongAndComplicatedPassword"), salt);
			user.UserType = FootballManager.Domain.Entity.Models.Authentication.Enums.UserType.Admin;
			UsersList.Add(user);

			user = new User() { Username = "zażółćgęśląjaźń", Id = id++ };
			salt = new byte[8];
			rng.GetBytes(salt);
			user.Salt = salt;
			user.PasswordHash = GenerateSaltedHash(Encoding.UTF8.GetBytes("zażółćgęśląjaźń"), salt);
			user.UserType = FootballManager.Domain.Entity.Models.Authentication.Enums.UserType.Admin;
			UsersList.Add(user);
			#endregion

			contextMock.Setup(x => x.Users).Returns(CreateDbSetMock(UsersList).Object);
			this.context = contextMock;
			this.authenticationService = new FootballManager.AuthenticationLayer.AuthenticationService(context.Object);
		}

		/// <summary>
		/// Test cleanup.
		/// </summary>
		[TestCleanup]
		public void Cleanup()
		{
			this.context.Object.Dispose();
		}

		/// <summary>
		/// Login tests for users without polish characters in both username and password.
		/// </summary>
		[TestMethod]
		public void TryProperLoginForNormalUsers()
		{
			context.Object.AuthenticateUser("admin", "admin").Should().NotBe(-1);
			context.Object.AuthenticateUser("password", "user").Should().NotBe(-1);
			context.Object.AuthenticateUser("testUser", "ThisIsVeryLongAndComplicatedPassword").Should().NotBe(-1);
		}

		/// <summary>
		/// Login tests for users with polish characters in username and password
		/// </summary>
		[TestMethod]
		public void TryProperLoginForPolishUser()
		{
			context.Object.AuthenticateUser("zażółćgęśląjaźń", "zażółćgęśląjaźń").Should().NotBe(-1);
		}

		/// <summary>
		/// Login tests for non-existing users.
		/// </summary>
		[TestMethod]
		public void TryLoginWithWrongUsername()
		{
			context.Object.AuthenticateUser("blebleble", "admin").Should().Be(-1);
			context.Object.AuthenticateUser("user", "user").Should().Be(-1);
			context.Object.AuthenticateUser("ThisIsVeryLongAndComplicatedAndWrongUserName", 
				"ThisIsVeryLongAndComplicatedPassword").Should().Be(-1);
		}

		/// <summary>
		/// Adds referee with unique login value.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task AddRefereeWithUniqueLogin()
		{
			await this.authenticationService.CreateRefereeAccount("test", "test");
			this.UsersList.Single(x => x.Username == "testt").Should().NotBeNull();
		}

		/// <summary>
		/// Adds referee, when his login already exists.
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task AddRefereeWithExistingLogin()
		{
			await this.authenticationService.CreateRefereeAccount("n", "admi");
			this.UsersList.Single(x => x.Username == "admin2").Should().NotBeNull();
		}

		/// <summary>
		/// Login tests for existing users, but with wrong password.
		/// </summary>
		[TestMethod]
		public void TryLoginWithWrongPassword()
		{
			context.Object.AuthenticateUser("admin", "ThisIsVeryLongAndComplicatedPassword").Should().Be(-1);
			context.Object.AuthenticateUser("password", "ThisIsVeryLongAndComplicatedPassword").Should().Be(-1);
			context.Object.AuthenticateUser("testUser", "BLEBELBLEBLEBLE!@#!@#!@#!").Should().Be(-1);
		}

		[TestMethod]
		public void ChangePasswordSuccess()
		{
			context.Object.ChangePassword(1, "admin", "newPassword")
				.Should()
				.BeTrue("it IS a proper username/password combination");
		}

		[TestMethod]
		public void ChangePasswordWrongOldPassword()
		{
			context.Object.ChangePassword(2, "userrrrrr", "newPassword")
				.Should()
				.BeFalse("it IS NOT a proper username/password combination");
		}

		[TestMethod]
		public void ChangePasswordEmptyNewPassword()
		{
			context.Object.ChangePassword(3, "ThisIsVeryLongAndComplicatedPassword", string.Empty)
				.Should()
				.BeFalse("it IS NOT a proper new password since it is empty string");
		}

		/// <summary>
		/// Creates DbSet Mocks.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="elements"></param>
		/// <returns></returns>
		private static Mock<DbSet<T>> CreateDbSetMock<T>(List<T> elements) where T : class
		{
			var elementsAsQueryable = elements.AsQueryable();
			var dbSetMock = new Mock<DbSet<T>>();

			dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
			dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
			dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
			dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());
			dbSetMock.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(elements.Add);

			return dbSetMock;
		}

		/// <summary>
		/// Generates hash with salt using SHA256 algorithm.
		/// </summary>
		/// <param name="plainText">Byte array of password to be hashed.</param>
		/// <param name="salt">Unique salt.</param>
		/// <returns>Byte array of salted password hash.</returns>
		private static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
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

	}
}
