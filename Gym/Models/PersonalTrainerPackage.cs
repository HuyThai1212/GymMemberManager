using System;
using System.Collections.Generic;

namespace Gym.Models;

public partial class PersonalTrainerPackage
{
    public int PtpackageId { get; set; }

    public string Ptname { get; set; } = null!;

    public int DurationInWeeks { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
