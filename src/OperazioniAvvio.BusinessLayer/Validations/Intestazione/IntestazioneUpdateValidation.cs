namespace OperazioniAvvio.BusinessLayer.Validations.Intestazione;

public class IntestazioneUpdateValidation : AbstractValidator<IntestazioneUpdateModel>
{
	public IntestazioneUpdateValidation()
	{
		RuleFor(x => x.IdFesta)
			.GreaterThan(0).WithMessage("IdFesta must be greater than 0.");

		RuleFor(x => x.Titolo)
			.NotEmpty().WithMessage("Titolo is required.")
			.MinimumLength(5).WithMessage("Titolo must be at least 5 characters.")
			.MaximumLength(100).WithMessage("Titolo must be at most 100 characters.");

		RuleFor(x => x.Edizione)
			.NotEmpty().WithMessage("Edizione is required.")
			.MinimumLength(5).WithMessage("Edizione must be at least 5 characters.")
			.MaximumLength(200).WithMessage("Edizione must be at most 200 characters.");

		RuleFor(x => x.Luogo)
			.NotEmpty().WithMessage("Luogo is required.")
			.MinimumLength(5).WithMessage("Luogo must be at least 5 characters.")
			.MaximumLength(100).WithMessage("Luogo must be at most 100 characters.");

		//RuleFor(x => x.Logo)
		//	.NotEmpty().WithMessage("Logo is required.");
	}
}
