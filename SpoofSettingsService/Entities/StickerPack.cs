using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class StickerPack
{
    public long Id { get; set; }

    public long AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string? PreviewPath { get; set; }

    public DateTime LastModified { get; set; }

    public bool IsDeleted { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<Sticker> Stickers { get; set; } = new List<Sticker>();
}
