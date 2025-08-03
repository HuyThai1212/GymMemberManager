using System;
using System.Collections.Generic;

namespace Gym.Models;

public partial class UserAccount
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
