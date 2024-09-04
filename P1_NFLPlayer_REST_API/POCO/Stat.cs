using System;
using System.Collections.Generic;

namespace P1_NFLPlayer_REST_API.POCO;

public partial class Stat
{
    public int StatId { get; set; }

    public int? GameId { get; set; }

    public int? Qbid { get; set; }

    public int PassingYards { get; set; }

    public int Touchdowns { get; set; }

    public int Interceptions { get; set; }

    public virtual Game? Game { get; set; }

    public virtual Quarterback? Qb { get; set; }
}
