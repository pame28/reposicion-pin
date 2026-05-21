using Microsoft.EntityFrameworkCore;
using reposicion_pin.Models;

namespace reposicion_pin.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<TarjetaModel> Tarjetas { get; set; } = null!;
        public DbSet<CatalogoRespuestaModel> CatalogoRespuestas { get; set; } = null!;


        // SMS
        public DbSet<BandejaEnvioSmsModel> BandejaEnvioSms { get; set; }


        // Email
        public DbSet<BandejaEnvioEmailModel> BandejaEnvioEmail { get; set; }

       
        public virtual DbSet<usp_ObtenerConfigBiMovil> usp_ObtenerConfigBiMovil { get; set; }

        public virtual DbSet<usp_ObtenerConfigEmail> usp_ObtenerConfigEmail { get; set; }
        public virtual DbSet<ParamAmbienteModel> ParamAmbiente { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<usp_ObtenerConfigBiMovil>(entity =>
            {
                entity.HasNoKey();
                entity.ToSqlQuery("EXEC usp_ObtenerConfigBiMovil");
            });


            modelBuilder.Entity<usp_ObtenerConfigEmail>(entity =>
            {
                entity.HasNoKey();
                entity.ToSqlQuery("EXEC usp_ObtenerConfigEmail");
            });
        }
    }
}
