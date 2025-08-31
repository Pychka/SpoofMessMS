using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class User
{
    public long Id { get; set; }

    public DateTime WasOnline { get; set; }

    public string Name { get; set; } = null!;

    public int MonthsBeforeDelete { get; set; }

    public bool SearchMe { get; set; }

    public bool ShowMe { get; set; }

    public bool ForwardMessage { get; set; }

    public bool InviteMe { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsOnline { get; set; }

    public virtual ICollection<Avatar> Avatars { get; set; } = new List<Avatar>();

    public virtual ICollection<ChannelWriter> ChannelWriters { get; set; } = new List<ChannelWriter>();

    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();

    public virtual ICollection<ChatUser> ChatUsers { get; set; } = new List<ChatUser>();

    public virtual ICollection<GroupChat> GroupChats { get; set; } = new List<GroupChat>();

    public virtual ICollection<PrivateChat> PrivateChatUser1s { get; set; } = new List<PrivateChat>();

    public virtual ICollection<PrivateChat> PrivateChatUser2s { get; set; } = new List<PrivateChat>();

    public virtual ICollection<StickerPack> StickerPacks { get; set; } = new List<StickerPack>();

    public virtual ICollection<UniqueName> UniqueNames { get; set; } = new List<UniqueName>();
}
