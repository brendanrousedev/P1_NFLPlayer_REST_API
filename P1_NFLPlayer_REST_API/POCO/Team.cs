using System;
using System.Collections.Generic;

namespace P1_NFLPlayer_REST_API.POCO;

public partial class Team
{
    public int TeamId { get; set; }

    public string Name { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? Logo { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Quarterback> Quarterbacks { get; set; } = new List<Quarterback>();
}
