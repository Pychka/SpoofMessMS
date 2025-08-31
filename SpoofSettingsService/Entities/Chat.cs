using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class Chat
{
    public long Id { get; set; }

    public string ChatType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Avatar> Avatars { get; set; } = new List<Avatar>();

    public virtual Channel? Channel { get; set; }

    public virtual ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();

    public virtual GroupChat? GroupChat { get; set; }

    public virtual PrivateChat? PrivateChat { get; set; }
}
