namespace OperazioniAvvio.BusinessLayer.Models.Intestazione;

public class IntestazioneUpdateModel
{
	public int IdFesta { get; set; }
	public string Titolo { get; set; } = null!;
	public string Edizione { get; set; } = null!;
	public string Luogo { get; set; } = null!;
	public string Logo { get; set; } = null!;
}