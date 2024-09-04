using System;
using System.Collections.Generic;

namespace P1_NFLPlayer_REST_API.POCO;

public partial class Quarterback
{
    public int Qbid { get; set; }

    public string Name { get; set; } = null!;

    public int? TeamId { get; set; }

    public DateOnly? Birthdate { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

    public virtual ICollection<Stat> Stats { get; set; } = new List<Stat>();

    public virtual Team? Team { get; set; }
}
