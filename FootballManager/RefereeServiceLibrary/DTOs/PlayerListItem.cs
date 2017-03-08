using System.Runtime.Serialization;

namespace RefereeServiceLibrary.DTOs
{
	/// <summary>
	/// Summary description for PlayerListItem
	/// </summary>
	[DataContract]
	public class PlayerListItem
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string LastName { get; set; }
	}
}
