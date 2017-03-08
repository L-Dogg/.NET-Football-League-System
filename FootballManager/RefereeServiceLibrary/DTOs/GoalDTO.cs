using FootballManager.Domain.Entity.Enums;
using System.Runtime.Serialization;


namespace RefereeServiceLibrary.DTOs
{
	/// <summary>
	/// Summary description for MatchDTO
	/// </summary>
	[DataContract]
	public class GoalDTO
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public PlayerListItem Scorer { get; set; }
		[DataMember]
		public int TeamId { get; set; }
		[DataMember]
		public GoalType GoalType { get; set; }
		[DataMember]
		public int Time { get; set; }
	}
}
