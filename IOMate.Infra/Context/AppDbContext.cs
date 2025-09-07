using IOMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IOMate.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<EventEntity<User>> UserEvents { get; set; }
        public DbSet<ClaimGroup> ClaimGroups { get; set; } = null!;
        public DbSet<ResourceClaim> ResourceClaims { get; set; } = null!;
        public DbSet<UserClaimGroup> UserClaimGroups { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventEntity<User>>()
                .HasOne(e => e.Entity)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.EntityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EventEntity<User>>()
                .HasOne(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClaimGroup>()
                .HasMany(cg => cg.Claims)
                .WithOne(c => c.ClaimGroup)
                .HasForeignKey(c => c.ClaimGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClaimGroup>()
                .HasMany(cg => cg.UserClaimGroups)
                .WithOne(ucg => ucg.ClaimGroup)
                .HasForeignKey(ucg => ucg.ClaimGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserClaimGroups)
                .WithOne(ucg => ucg.User)
                .HasForeignKey(ucg => ucg.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserClaimGroup>()
                .HasIndex(ucg => new { ucg.UserId, ucg.ClaimGroupId })
                .IsUnique();
        }
    }
}