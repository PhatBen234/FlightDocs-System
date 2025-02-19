using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FlightDocs_System.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Flight>? Flights { get; set; }
        public DbSet<FlightDocument>? FlightDocuments { get; set; }
        public DbSet<GroupPermission>? GroupPermissions { get; set; }
        public DbSet<GroupPermission_User>? GroupPermission_User { get; set; }
        public DbSet<TypeDocument>? TypeDocument { get; set; }
        public DbSet<TypeDocument_Group>? TypeDocument_Group { get; set; }
        public DbSet<Document_Group>? Document_Group { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            // Your code for configuring the database model
        }
        // Cấu hình database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document_Group>()
                .HasOne(s => s.GroupPermission)
                .WithMany()
               .HasForeignKey(s => s.ID_GroupPermission)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Document_Group>()
                .HasOne(s => s.FlightDocument)
                .WithMany()
               .HasForeignKey(s => s.ID_Document)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FlightDocument>()
                .HasOne(s => s.Flight)
                .WithMany()
               .HasForeignKey(s => s.ID_Flight)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FlightDocument>()
               .HasOne(s => s.TypeDocument)
               .WithMany()
              .HasForeignKey(s => s.ID_TypeDocument)
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FlightDocument>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupPermission>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupPermission_User>()
                .HasOne(s => s.GroupPermission)
                .WithMany()
               .HasForeignKey(s => s.ID_GroupPermission)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GroupPermission_User>()
               .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TypeDocument>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TypeDocument_Group>()
                .HasOne(s => s.TypeDocument)
                .WithMany()
               .HasForeignKey(s => s.ID_TypeDocument)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TypeDocument_Group>()
               .HasOne(s => s.GroupPermission)
               .WithMany()
              .HasForeignKey(s => s.ID_GroupPermission)
              .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
