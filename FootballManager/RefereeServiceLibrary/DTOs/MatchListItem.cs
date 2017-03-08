using System;
using System.Runtime.Serialization;


namespace RefereeServiceLibrary.DTOs
{
	/// <summary>
	/// Summary description for MatchListItem
	/// </summary>
	[DataContract]
	public class MatchListItem
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string HomeTeamName { get; set; }
		[DataMember]
		public string AwayTeamName { get; set; }
		[DataMember]
		public int? HomeTeamGoals { get; set; }
		[DataMember]
		public int? AwayTeamGoals { get; set; }
		[DataMember]
		public DateTime Date { get; set; }
	}
}
