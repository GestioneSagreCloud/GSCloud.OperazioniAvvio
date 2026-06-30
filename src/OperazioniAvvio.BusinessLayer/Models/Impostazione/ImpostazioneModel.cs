namespace OperazioniAvvio.BusinessLayer.Models.Impostazione;

public class ImpostazioneModel
{
	public int Id { get; set; }
	public int IdFesta { get; set; }
	public bool GestioneCoperti { get; set; }
	public bool GestioneMenu { get; set; }
	public bool GestioneCategorie { get; set; }
	public bool StampaCarta { get; set; }
	public bool StampaLogo { get; set; }
	public bool StampaRicevuta { get; set; }
}