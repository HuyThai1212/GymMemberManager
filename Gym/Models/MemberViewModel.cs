namespace Gym.Models
{
    public class MemberViewModel
    {
        public int MemberId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime JoinDate { get; set; }

        public string? MembershipPackageName { get; set; }
        public string? PersonalTrainerPackageName { get; set; }
    }
}
