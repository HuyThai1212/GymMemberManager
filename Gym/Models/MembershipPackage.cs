using System;
using System.Collections.Generic;

namespace Gym.Models;

public partial class MembershipPackage
{
    public int PackageId { get; set; }

    public string PackageName { get; set; } = null!;

    public int DurationInMonths { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
