using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class GroupChat
{
    public long ChatId { get; set; }

    public long OwnerId { get; set; }

    public string GroupName { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Chat Chat { get; set; } = null!;

    public virtual User Owner { get; set; } = null!;
}
