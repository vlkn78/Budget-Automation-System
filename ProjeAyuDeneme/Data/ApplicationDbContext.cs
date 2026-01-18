using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjeAyuDeneme.Models;

namespace ProjeAyuDeneme.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSet<Agirlama> Agirlamalar { get; set; }
        public DbSet<Fuar> Fuarlar { get; set; }
        public DbSet<KulturSanat> KulturSanatlar { get; set; }
        public DbSet<Reklam> Reklamlar { get; set; }
        public DbSet<Sektor> Sektorler { get; set; }
        public DbSet<Mahsup> Mahsuplar { get; set; }
        public DbSet<Odenek> Odenekler { get; set; }
        public DbSet<Ofis> Ofisler { get; set; }
        public DbSet<GenelButce> GenelButceler { get; set; }  

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            // --- Decimal Tipleri İçin Hassasiyet Ayarları ---
            modelBuilder.Entity<Fuar>().Property(p => p.Degeri).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Fuar>().Property(p => p.StandM2).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<KulturSanat>().Property(p => p.Degeri).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Reklam>().Property(p => p.Degeri).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Agirlama>().Property(p => p.Degeri).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Sektor>().Property(p => p.Degeri).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Mahsup>().Property(p => p.Tutar).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<Odenek>().Property(p => p.Tutar).HasColumnType("decimal(18, 2)");
            modelBuilder.Entity<GenelButce>().Property(p => p.BaslangicTutari).HasColumnType("decimal(18,2)");

            var restrictCascadeDelete = (ModelBuilder mb) =>
            {
                foreach (var relationship in mb.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                {
                    relationship.DeleteBehavior = DeleteBehavior.Restrict;
                }
            };

            restrictCascadeDelete(modelBuilder);
        }
    }
}