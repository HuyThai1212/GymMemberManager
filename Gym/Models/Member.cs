using System;
using System.Collections.Generic;

namespace Gym.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateOnly JoinDate { get; set; }

    public int? MembershipPackageId { get; set; }

    public int? PtpackageId { get; set; }

    public virtual MembershipPackage? MembershipPackage { get; set; }

    public virtual PersonalTrainerPackage? Ptpackage { get; set; }
}
