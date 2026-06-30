namespace OperazioniAvvio.BusinessLayer.Validations.Festa;

public class FestaCreateValidation : AbstractValidator<FestaCreateModel>
{
	public FestaCreateValidation()
	{
		RuleFor(x => x.DataInizio)
			.NotEmpty().WithMessage("La data di inizio è obbligatoria.")
			.GreaterThanOrEqualTo(DateTime.Today).WithMessage("La data di inizio deve essere oggi o in futuro.");

		RuleFor(x => x.DataFine)
			.NotEmpty().WithMessage("La data di fine è obbligatoria.")
			.GreaterThan(x => x.DataInizio).WithMessage("La data di fine deve essere successiva alla data di inizio.");
	}
}