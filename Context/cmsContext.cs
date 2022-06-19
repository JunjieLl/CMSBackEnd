using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CMS
{
    public partial class cmsContext : DbContext
    {
        public cmsContext()
        {
        }

        public cmsContext(DbContextOptions<cmsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activities { get; set; } = null!;
        public virtual DbSet<CommonUser> CommonUsers { get; set; } = null!;
        public virtual DbSet<Favorite> Favorites { get; set; } = null!;
        public virtual DbSet<Manage> Manages { get; set; } = null!;
        public virtual DbSet<ModifyRecord> ModifyRecords { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<RoomManager> RoomManagers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=101.35.111.182;port=52125;uid=cms;pwd=hello;database=cms", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.Property(e => e.ActivityId).IsFixedLength();

                entity.Property(e => e.CommonUserId).HasComment("user who is going to organize an activity");

                entity.Property(e => e.RoomId).IsFixedLength();

                entity.HasOne(d => d.CommonUser)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.CommonUserId)
                    .HasConstraintName("activity_ibfk_1");

                entity.HasOne(d => d.ManagerUser)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.ManagerUserId)
                    .HasConstraintName("activity_ibfk_4");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("activity_ibfk_3");
            });

            modelBuilder.Entity<CommonUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.CommonUser)
                    .HasForeignKey<CommonUser>(d => d.UserId)
                    .HasConstraintName("common_user_ibfk_1");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoomId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.Property(e => e.RoomId).IsFixedLength();

                entity.Property(e => e.Placeholder).HasComment("占位");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("favorite_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("favorite_ibfk_1");
            });

            modelBuilder.Entity<Manage>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoomId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.Property(e => e.RoomId).IsFixedLength();

                entity.Property(e => e.Placeholder).HasComment("占位符");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Manages)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("manage_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Manages)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("manage_ibfk_1");
            });

            modelBuilder.Entity<ModifyRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PRIMARY");

                entity.Property(e => e.RecordId).IsFixedLength();

                entity.Property(e => e.ActivityId).IsFixedLength();

                entity.Property(e => e.UserId).HasComment("any user who change the detail");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ModifyRecords)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("modify_record_ibfk_2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ModifyRecords)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("modify_record_ibfk_1");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId).IsFixedLength();
            });

            modelBuilder.Entity<RoomManager>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.RoomManager)
                    .HasForeignKey<RoomManager>(d => d.UserId)
                    .HasConstraintName("room_manager_ibfk_1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.ActivityStatus)
                    .HasDefaultValueSql("'1'")
                    .HasComment("1代表正常，0代表禁用");

                entity.Property(e => e.Password).HasDefaultValueSql("'1234'");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
