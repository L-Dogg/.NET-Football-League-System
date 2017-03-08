using System.Runtime.Serialization;

namespace RefereeServiceLibrary.DTOs
{
	/// <summary>
	/// Summary description for StadiumDTO
	/// </summary>
	[DataContract]
	public class StadiumDTO
	{
		[DataMember]
		public int Id { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public AddressDTO Address { get; set; }
	}
}
