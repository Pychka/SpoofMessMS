using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class UniqueName
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public long? ChannelId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime LastModified { get; set; }

    public virtual Channel? Channel { get; set; }

    public virtual User? User { get; set; }
}
