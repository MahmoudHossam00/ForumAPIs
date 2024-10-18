namespace BlogProject.Models
{
	public class BanUserModel
	{
		public string UserName { set; get; }


		public int? banDurationInDays { get; set; } = 0;
		public int? BanDurationInHours { get; set; } = 0;
		public int? BanDurationInMinutes { get; set; } = 0;
	}
}
