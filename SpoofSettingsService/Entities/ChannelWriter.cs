using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class ChannelWriter
{
    public long ChannelId { get; set; }

    public long UserId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Channel Channel { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
