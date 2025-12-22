using Microsoft.EntityFrameworkCore;
using SME.NovaSondagem.Worker.Dtos.Eol;

namespace SME.NovaSondagem.Worker.Contexts
{
    public class EolDbContext : DbContext
    {
        public EolDbContext(DbContextOptions<EolDbContext> options) : base(options)
        {
        }

        public DbSet<ComponenteCurricularEol> ComponentesCurriculares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComponenteCurricularEol>(entity =>
            {
                entity.HasKey(e => e.CdComponenteCurricular);
                entity.Property(e => e.DcComponenteCurricular).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}