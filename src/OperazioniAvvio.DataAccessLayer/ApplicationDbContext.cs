namespace OperazioniAvvio.DataAccessLayer;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
	public virtual DbSet<Festa> Feste { get; set; }
	public virtual DbSet<Intestazione> Intestazioni { get; set; }
	public virtual DbSet<Impostazione> Impostazioni { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Festa>(entity =>
		{
			entity.ToTable("Feste");
			entity.HasKey(e => e.Id).HasName("PK_Feste");

			entity.HasOne(e => e.Intestazione)
				.WithOne(i => i.Festa)
				.HasForeignKey<Intestazione>(i => i.IdFesta)
				.OnDelete(DeleteBehavior.Cascade);

			entity.HasOne(e => e.Impostazione)
				.WithOne(i => i.Festa)
				.HasForeignKey<Impostazione>(i => i.IdFesta)
				.OnDelete(DeleteBehavior.Cascade);
		});
	}
}