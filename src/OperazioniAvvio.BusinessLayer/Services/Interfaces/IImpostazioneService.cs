namespace OperazioniAvvio.BusinessLayer.Services.Interfaces;

public interface IImpostazioneService
{
    Task<Result<IEnumerable<ImpostazioneModel>>> GetImpostazioniFesteAsync(CancellationToken cancellationToken);
    Task<Result<ImpostazioneModel>> GetImpostazioneFestaByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<ImpostazioneModel>> CreateImpostazioneFestaAsync(ImpostazioneCreateModel model, CancellationToken cancellationToken);
    Task<Result<ImpostazioneModel>> UpdateImpostazioneFestaAsync(int id, ImpostazioneUpdateModel model, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteImpostazioneFestaAsync(int id, CancellationToken cancellationToken);
}