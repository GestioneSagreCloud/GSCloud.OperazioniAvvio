namespace OperazioniAvvio.DataAccessLayer.Entities;

public class Intestazione : BaseEntity<int>
{
	// Foreign key
	public int IdFesta { get; set; }

	// Navigation property
	public Festa Festa { get; set; } = new Festa();

	public string Titolo { get; set; } = null!;
	public string Edizione { get; set; } = null!;
	public string Luogo { get; set; } = null!;
	public string Logo { get; set; } = null!;
}