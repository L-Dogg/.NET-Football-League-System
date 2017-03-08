using FootballManager.Domain.Entity.Models.Authentication.Enums;
using RefereeServiceLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RefereeServiceLibrary
{

	[ServiceContract]
	public interface IRefereeService
	{
		[OperationContract]
		int AuthenticateReferee(string username, string password, UserType userType = UserType.Referee);

		[OperationContract]
		bool ChangePassword(int userID, string oldPassword, string newPassword);

		[OperationContract]
		PlayerListItem GetPlayer();

		[OperationContract]
		Task<List<MatchListItem>> GetMatchesList(int refereeId);

		[OperationContract]
		Task<MatchDTO> GetMatch(int id);

		[OperationContract]
		Task SaveGoals(MatchDTO match);
	}
}
