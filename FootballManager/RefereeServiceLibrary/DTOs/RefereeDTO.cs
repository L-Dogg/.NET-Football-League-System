using System.Runtime.Serialization;

namespace RefereeServiceLibrary.DTOs
{
	[DataContract]
	public class RefereeDTO
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string LastName { get; set; }
	}
}
