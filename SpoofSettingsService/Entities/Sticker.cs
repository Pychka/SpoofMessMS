using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class Sticker
{
    public long Id { get; set; }

    public long StickerPackId { get; set; }

    public string? FilePath { get; set; }

    public string Title { get; set; } = null!;

    public DateTime LastModified { get; set; }

    public bool IsDeleted { get; set; }

    public virtual StickerPack StickerPack { get; set; } = null!;
}
