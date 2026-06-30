namespace OperazioniAvvio.BusinessLayer.Services.Interfaces;

public interface IIntestazioneService
{
    Task<Result<IEnumerable<IntestazioneModel>>> GetIntestazioniFesteAsync(CancellationToken cancellationToken);
    Task<Result<IntestazioneModel>> GetIntestazioneFestaByIdAsync(int id, CancellationToken cancellationToken);
    Task<Result<IntestazioneModel>> CreateIntestazioneFestaAsync(IntestazioneCreateModel model, CancellationToken cancellationToken);
    Task<Result<IntestazioneModel>> UpdateIntestazioneFestaAsync(int id, IntestazioneUpdateModel model, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteIntestazioneFestaAsync(int id, CancellationToken cancellationToken);
}