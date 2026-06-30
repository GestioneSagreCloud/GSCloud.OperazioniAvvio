namespace OperazioniAvvio.BusinessLayer.Validations.Impostazione;

public class ImpostazioneCreateValidation : AbstractValidator<ImpostazioneCreateModel>
{
    public ImpostazioneCreateValidation()
    {
        RuleFor(x => x.IdFesta)
            .GreaterThan(0).WithMessage("IdFesta must be greater than 0.");
    }
}