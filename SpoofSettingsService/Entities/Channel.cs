using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class Channel
{
    public long ChatId { get; set; }

    public long OwnerId { get; set; }

    public string ChannelName { get; set; } = null!;

    public bool IsPublic { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime LastModified { get; set; }

    public virtual ICollection<ChannelWriter> ChannelWriters { get; set; } = new List<ChannelWriter>();

    public virtual Chat Chat { get; set; } = null!;

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<UniqueName> UniqueNames { get; set; } = new List<UniqueName>();
}
