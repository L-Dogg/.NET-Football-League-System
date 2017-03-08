using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace RefereeServiceLibrary.DTOs
{
	/// <summary>
	/// Summary description for MatchDTO
	/// </summary>
	[DataContract]
	public class MatchDTO
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public int HomeTeamId { get; set; }
		[DataMember]
		public int AwayTeamId { get; set; }
		[DataMember]
		public string HomeTeamName { get; set; }
		[DataMember]
		public string AwayTeamName { get; set; }
		[DataMember]
		public int? HomeTeamScore { get; set; }
		[DataMember]
		public int? AwayTeamScore { get; set; }
		[DataMember]
		public List<PlayerListItem> HomeTeamPlayers { get; set; }
		[DataMember]
		public List<PlayerListItem> AwayTeamPlayers { get; set; }
		[DataMember]
		public List<GoalDTO> HomeTeamGoals { get; set; }
		[DataMember]
		public List<GoalDTO> AwayTeamGoals { get; set; }
		[DataMember]
		public DateTime Date { get; set; }
		[DataMember]
		public StadiumDTO Stadium { get; set; }
		[DataMember]
		public RefereeDTO Referee { get; set; }
		[DataMember]
		public int Attendance { get; set; }
	}
}
