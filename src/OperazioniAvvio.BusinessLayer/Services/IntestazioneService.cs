namespace OperazioniAvvio.BusinessLayer.Services;

public class IntestazioneService(ILogger<IntestazioneService> logger, IRepository<Intestazione, int> repository, IUnitOfWork unitOfWork) : IIntestazioneService
{
    public async Task<Result<IEnumerable<IntestazioneModel>>> GetIntestazioniFesteAsync(CancellationToken cancellationToken)
    {
        var query = await repository.GetAllAsync(cancellationToken);

        if (query is null)
        {
            logger.LogWarning("No Intestazioni found in the database.");
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        var result = query.Select(x => new IntestazioneModel
        {
            Id = x.Id,
            IdFesta = x.IdFesta,
            Titolo = x.Titolo,
            Edizione = x.Edizione,
            Luogo = x.Luogo,
            Logo = x.Logo
        }).ToList();

        return result;
    }

    public async Task<Result<IntestazioneModel>> GetIntestazioneFestaByIdAsync(int id, CancellationToken cancellationToken)
    {
        var query = await repository.GetByIdAsync(id, cancellationToken);

        if (query is null)
        {
            logger.LogWarning("Intestazione with ID {Id} not found.", id);
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        var result = new IntestazioneModel
        {
            Id = query.Id,
            IdFesta = query.IdFesta,
            Titolo = query.Titolo,
            Edizione = query.Edizione,
            Luogo = query.Luogo,
            Logo = query.Logo
        };

        return result;
    }

    public async Task<Result<IntestazioneModel>> CreateIntestazioneFestaAsync(IntestazioneCreateModel model, CancellationToken cancellationToken)
    {
        var entity = new Intestazione
        {
            IdFesta = model.IdFesta,
            Titolo = model.Titolo,
            Edizione = model.Edizione,
            Luogo = model.Luogo,
            Logo = model.Logo
        };

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await repository.AddAsync(entity, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error creating Intestazione");

            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Fail(CustomFailureReasons.InvalidRequest);
        }

        var createdModel = new IntestazioneModel
        {
            Id = entity.Id,
            IdFesta = entity.IdFesta,
            Titolo = entity.Titolo,
            Edizione = entity.Edizione,
            Luogo = entity.Luogo,
            Logo = entity.Logo
        };

        return createdModel;
    }

    public async Task<Result<IntestazioneModel>> UpdateIntestazioneFestaAsync(int id, IntestazioneUpdateModel model, CancellationToken cancellationToken)
    {
        var query = await repository.GetByIdAsync(id, cancellationToken);

        if (query is null)
        {
            logger.LogWarning("Intestazione with ID {Id} not found for update.", id);
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        query.IdFesta = model.IdFesta;
        query.Titolo = model.Titolo;
        query.Edizione = model.Edizione;
        query.Luogo = model.Luogo;
        query.Logo = model.Logo;

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            repository.Update(query);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error updating Intestazione with id {Id}", id);

            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Fail(CustomFailureReasons.InvalidRequest);
        }

        var updatedModel = new IntestazioneModel
        {
            Id = query.Id,
            IdFesta = query.IdFesta,
            Titolo = query.Titolo,
            Edizione = query.Edizione,
            Luogo = query.Luogo,
            Logo = query.Logo
        };

        return updatedModel;
    }

    public async Task<Result<bool>> DeleteIntestazioneFestaAsync(int id, CancellationToken cancellationToken)
    {
        var query = await repository.GetByIdAsync(id, cancellationToken);

        if (query is null)
        {
            logger.LogWarning("Intestazione with ID {Id} not found for deletion.", id);
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            repository.Delete(query);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error deleting Intestazione with id {Id}", id);

            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Fail(FailureReasons.InvalidContent);
        }

        return true;
    }
}