using Microsoft.EntityFrameworkCore;
using SME.NovaSondagem.Worker.Dtos;

namespace SME.NovaSondagem.Worker.Contexts
{
    public class NovaSondagemDbContext : DbContext
    {
        public NovaSondagemDbContext(DbContextOptions<NovaSondagemDbContext> options) : base(options)
        {
        }

        public DbSet<ComponenteCurricular> ComponentesCurriculares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ComponenteCurricular>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CodigoEol).IsRequired();
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(255);
                entity.Property(e => e.DataCriacao).HasDefaultValueSql("GETUTCDATE()");
                
                entity.HasIndex(e => e.CodigoEol).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}