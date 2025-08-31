using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class ChatUser
{
    public long ChatId { get; set; }

    public long UserId { get; set; }

    public bool IsMutted { get; set; }

    public string Role { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
