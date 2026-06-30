namespace OperazioniAvvio.DataAccessLayer.Entities;

public class Festa : BaseEntity<int>
{
	public DateTime DataInizio { get; set; }
	public DateTime DataFine { get; set; }

	// Navigation property
	public Intestazione Intestazione { get; set; } = new Intestazione();
	public Impostazione Impostazione { get; set; } = new Impostazione();
}