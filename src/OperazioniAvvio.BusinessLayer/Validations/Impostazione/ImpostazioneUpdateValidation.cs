namespace OperazioniAvvio.BusinessLayer.Validations.Impostazione;

public class ImpostazioneUpdateValidation : AbstractValidator<ImpostazioneUpdateModel>
{
    public ImpostazioneUpdateValidation()
    {
        RuleFor(x => x.IdFesta)
            .GreaterThan(0).WithMessage("IdFesta must be greater than 0.");
    }
}