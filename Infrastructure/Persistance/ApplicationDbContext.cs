using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        #region Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        #endregion

        #region DbSets
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<AnnouncementUser> AnnouncementUsers { get; set; }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Announcement entity
            builder.Entity<Announcement>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Title).IsRequired().HasMaxLength(200);
                entity.Property(a => a.Content).IsRequired();
                entity.Property(a => a.Category).IsRequired().HasMaxLength(100);
                entity.Property(a => a.CreatedAt).IsRequired();
                entity.Property(a => a.UpdatedAt).IsRequired();
                entity.HasOne(a => a.Author)
                      .WithMany(u => u.Announcements)
                      .HasForeignKey(a => a.AuthorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // Configure ApplicationUser entity
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(u => u.UserName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            });
            // Configure ApplicationRole entity
            builder.Entity<ApplicationRole>(entity =>
            {
                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            });

            builder.Entity<ApplicationUser>().HasIndex(u => u.UserName).IsUnique();
            builder.Entity<ApplicationUser>().HasIndex(u => u.Email).IsUnique();

            builder.Entity<Announcement>()
            .HasOne(a => a.Author)
            .WithMany(u => u.Announcements)
            .HasForeignKey(a => a.AuthorId);

            builder.Entity<AnnouncementUser>()
                .HasKey(au => new { au.ApplicationUserId, au.AnnouncementId });

            builder.Entity<AnnouncementUser>()
                .HasOne(au => au.Announcement)
                .WithMany(a => a.JoinedUsers)
                .HasForeignKey(au => au.AnnouncementId);

            builder.Entity<AnnouncementUser>()
                .HasOne(au => au.User)
                .WithMany(au => au.JoinedAnnouncements)
                .HasForeignKey(au => au.ApplicationUserId);
        }
        #endregion

        #region OnModelConfiguring
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
        #endregion

    }
}
