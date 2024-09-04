using System;
using System.Collections.Generic;

namespace P1_NFLPlayer_REST_API.POCO;

public partial class Game
{
    public int GameId { get; set; }

    public DateOnly Date { get; set; }

    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public string Stadium { get; set; } = null!;

    public int HomeTeamScore { get; set; }

    public int AwayTeamScore { get; set; }

    public virtual ICollection<Stat> Stats { get; set; } = new List<Stat>();
}
