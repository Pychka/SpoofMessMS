using System;
using System.Collections.Generic;

namespace SpoofSettingsService;

public partial class Avatar
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public long? ChatId { get; set; }

    public string? FilePath { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime LastModified { get; set; }

    public DateTime CreatedTime { get; set; }

    public virtual Chat? Chat { get; set; }

    public virtual User? User { get; set; }
}
