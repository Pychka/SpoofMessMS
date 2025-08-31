using System;
using System.Collections.Generic;

namespace SpoofEntranceService.Entities;

public partial class UserEntry
{
    public long Id { get; set; }

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public bool IsDeleted { get; set; }
}
