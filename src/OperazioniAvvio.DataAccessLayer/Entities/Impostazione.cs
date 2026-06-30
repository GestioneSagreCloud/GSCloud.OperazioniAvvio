namespace OperazioniAvvio.DataAccessLayer.Entities;

public class Impostazione : BaseEntity<int>
{
	// Foreign key
	public int IdFesta { get; set; }

	// Navigation property
	public Festa Festa { get; set; } = new Festa();

	public bool GestioneCoperti { get; set; }
	public bool GestioneMenu { get; set; }
	public bool GestioneCategorie { get; set; }
	public bool StampaCarta { get; set; }
	public bool StampaLogo { get; set; }
	public bool StampaRicevuta { get; set; }
}