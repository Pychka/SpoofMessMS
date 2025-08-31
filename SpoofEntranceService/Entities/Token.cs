using System;
using System.Collections.Generic;

namespace SpoofEntranceService.Entities;

public partial class Token
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Token1 { get; set; } = null!;

    public DateTime ValidTo { get; set; }

    public bool IsDeleted { get; set; }
}
