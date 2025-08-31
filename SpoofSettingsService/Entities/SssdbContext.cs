using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SpoofSettingsService;

public partial class SssdbContext : DbContext
{
    public SssdbContext()
    {
    }

    public SssdbContext(DbContextOptions<SssdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Avatar> Avatars { get; set; }

    public virtual DbSet<Channel> Channels { get; set; }

    public virtual DbSet<ChannelWriter> ChannelWriters { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatUser> ChatUsers { get; set; }

    public virtual DbSet<GroupChat> GroupChats { get; set; }

    public virtual DbSet<PrivateChat> PrivateChats { get; set; }

    public virtual DbSet<Sticker> Stickers { get; set; }

    public virtual DbSet<StickerPack> StickerPacks { get; set; }

    public virtual DbSet<UniqueName> UniqueNames { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=SSSDB;TrustServerCertificate=True;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Avatar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Avatar__3214EC07DA0F78E5");

            entity.ToTable("Avatar");

            entity.Property(e => e.CreatedTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Chat).WithMany(p => p.Avatars)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("FK__Avatar__ChatId__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.Avatars)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Avatar__UserId__7C4F7684");
        });

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__Channel__A9FBE7C6B3EF19E2");

            entity.ToTable("Channel");

            entity.Property(e => e.ChatId).ValueGeneratedNever();
            entity.Property(e => e.ChannelName).HasMaxLength(100);
            entity.Property(e => e.IsPublic).HasDefaultValue(true);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Chat).WithOne(p => p.Channel)
                .HasForeignKey<Channel>(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Channel__ChatId__5535A963");

            entity.HasOne(d => d.Owner).WithMany(p => p.Channels)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Channel__OwnerId__5629CD9C");
        });

        modelBuilder.Entity<ChannelWriter>(entity =>
        {
            entity.HasKey(e => new { e.ChannelId, e.UserId }).HasName("PK__ChannelW__E9BB64D08661E9FC");

            entity.ToTable("ChannelWriter");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Channel).WithMany(p => p.ChannelWriters)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChannelWr__Chann__6D0D32F4");

            entity.HasOne(d => d.User).WithMany(p => p.ChannelWriters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChannelWr__UserI__6E01572D");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat__3214EC07D2271DD8");

            entity.ToTable("Chat");

            entity.Property(e => e.ChatType).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ChatUser>(entity =>
        {
            entity.HasKey(e => new { e.ChatId, e.UserId }).HasName("PK__ChatUser__78836B02FB2774CA");

            entity.ToTable("ChatUser");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("member");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatUsers)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatUser__ChatId__6477ECF3");

            entity.HasOne(d => d.User).WithMany(p => p.ChatUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatUser__UserId__656C112C");
        });

        modelBuilder.Entity<GroupChat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__GroupCha__A9FBE7C6B10ADF28");

            entity.ToTable("GroupChat");

            entity.Property(e => e.ChatId).ValueGeneratedNever();
            entity.Property(e => e.GroupName).HasMaxLength(100);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Chat).WithOne(p => p.GroupChat)
                .HasForeignKey<GroupChat>(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupChat__ChatI__4F7CD00D");

            entity.HasOne(d => d.Owner).WithMany(p => p.GroupChats)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupChat__Owner__5070F446");
        });

        modelBuilder.Entity<PrivateChat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__PrivateC__A9FBE7C6F5E51B8B");

            entity.ToTable("PrivateChat");

            entity.Property(e => e.ChatId).ValueGeneratedNever();
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Chat).WithOne(p => p.PrivateChat)
                .HasForeignKey<PrivateChat>(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PrivateCh__ChatI__45F365D3");

            entity.HasOne(d => d.User1).WithMany(p => p.PrivateChatUser1s)
                .HasForeignKey(d => d.User1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PrivateCh__User1__46E78A0C");

            entity.HasOne(d => d.User2).WithMany(p => p.PrivateChatUser2s)
                .HasForeignKey(d => d.User2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PrivateCh__User2__47DBAE45");
        });

        modelBuilder.Entity<Sticker>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sticker__3214EC07BC38D8E7");

            entity.ToTable("Sticker");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.StickerPack).WithMany(p => p.Stickers)
                .HasForeignKey(d => d.StickerPackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sticker__Sticker__778AC167");
        });

        modelBuilder.Entity<StickerPack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StickerP__3214EC0748AA5269");

            entity.ToTable("StickerPack");

            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Author).WithMany(p => p.StickerPacks)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StickerPa__Autho__72C60C4A");
        });

        modelBuilder.Entity<UniqueName>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UniqueNa__3214EC0725075FA0");

            entity.ToTable("UniqueName");

            entity.HasIndex(e => e.Name, "UQ__UniqueNa__737584F669E1051B").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastModified)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Channel).WithMany(p => p.UniqueNames)
                .HasForeignKey(d => d.ChannelId)
                .HasConstraintName("FK__UniqueNam__Chann__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.UniqueNames)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UniqueNam__UserI__5CD6CB2B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_User_Id");

            entity.ToTable("User");

            entity.Property(e => e.ForwardMessage).HasDefaultValue(true);
            entity.Property(e => e.InviteMe).HasDefaultValue(true);
            entity.Property(e => e.MonthsBeforeDelete).HasDefaultValue(6);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.SearchMe).HasDefaultValue(true);
            entity.Property(e => e.ShowMe).HasDefaultValue(true);
            entity.Property(e => e.WasOnline)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
