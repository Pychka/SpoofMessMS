using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class PrivateChat
{
    public long ChatId { get; set; }

    public long User1Id { get; set; }

    public long User2Id { get; set; }

    public bool IsMuttedUser1 { get; set; }

    public bool IsMuttedUser2 { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User User1 { get; set; } = null!;

    public virtual User User2 { get; set; } = null!;
}
