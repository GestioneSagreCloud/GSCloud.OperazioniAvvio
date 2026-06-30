namespace OperazioniAvvio.BusinessLayer.Services.Interfaces;

public interface IFestaService
{
	Task<Result<IEnumerable<FestaModel>>> GetFesteAsync(CancellationToken cancellationToken);
	Task<Result<FestaModel>> GetFestaByIdAsync(int id, CancellationToken cancellationToken);
	Task<Result<FestaModel>> CreateFestaAsync(FestaCreateModel model, CancellationToken cancellationToken);
	Task<Result<FestaModel>> UpdateFestaAsync(int id, FestaUpdateModel model, CancellationToken cancellationToken);
	Task<Result<bool>> DeleteFestaAsync(int id, CancellationToken cancellationToken);
}