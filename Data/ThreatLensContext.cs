using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ThreatLens.Api.Models;
namespace ThreatLens.Api.Data
{
    public class ThreatLensContext : IdentityDbContext<IdentityUser>
    {
        public ThreatLensContext(DbContextOptions<ThreatLensContext> options) : base(options) { }

        public DbSet<Scan> Scans { get; set; }
        public DbSet<DomainScan> DomainScans { get; set; }
        public DbSet<IpScan> IpScans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            

            modelBuilder.Entity<Scan>(entity =>
            { 
                entity.HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // deleting a user deletes their scans


                entity.Property(s => s.TargetType)
                    .HasConversion<string>()
                    .HasMaxLength(20);


                entity.HasOne(s => s.DomainScan)
                        .WithOne(d => d.Scan)
                        .HasForeignKey<DomainScan>(d => d.ScanId)
                        .OnDelete(DeleteBehavior.Cascade);



                entity.HasOne(s => s.IpScan)
                        .WithOne(i => i.Scan)
                        .HasForeignKey<IpScan>(i => i.ScanId)
                        .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(s => new { s.UserId, s.CreatedAtUtc });
            });

            modelBuilder.Entity<DomainScan>().HasKey(d => d.ScanId);
            modelBuilder.Entity<IpScan>().HasKey(i => i.ScanId);
        }
    }
}
