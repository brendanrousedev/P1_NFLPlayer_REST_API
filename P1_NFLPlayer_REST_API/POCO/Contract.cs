using System;
using System.Collections.Generic;

namespace P1_NFLPlayer_REST_API.POCO;

public partial class Contract
{
    public int ContractId { get; set; }

    public int? Qbid { get; set; }

    public DateOnly? StartDate { get; set; }

    public decimal? Salary { get; set; }

    public string? SalaryFormatted { get; set; }

    public int? TeamId { get; set; }

    public virtual Quarterback? Qb { get; set; }

    public virtual Team? Team { get; set; }
}
