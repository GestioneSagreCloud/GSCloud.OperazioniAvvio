namespace OperazioniAvvio.BusinessLayer.Services;

public class FestaService(ILogger<FestaService> logger, IRepository<Festa, int> repository, IUnitOfWork unitOfWork) : IFestaService
{
    public async Task<Result<IEnumerable<FestaModel>>> GetFesteAsync(CancellationToken cancellationToken)
    {
        var query = await repository.GetAllAsync(cancellationToken);

        if (query is null)
        {
            logger.LogWarning("No Feste found in the database.");
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        var result = query.Select(x => new FestaModel
        {
            Id = x.Id,
            DataInizio = x.DataInizio,
            DataFine = x.DataFine,
        }).ToList();

        return result;
    }

    public async Task<Result<FestaModel>> GetFestaByIdAsync(int id, CancellationToken cancellationToken)
    {
        var query = await repository.GetByIdAsync(id, cancellationToken);

        if (query is null)
        {
            logger.LogWarning("Festa with ID {Id} not found.", id);
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        var result = new FestaModel
        {
            Id = query.Id,
            DataInizio = query.DataInizio,
            DataFine = query.DataFine,
        };

        return result;
    }

    public async Task<Result<FestaModel>> CreateFestaAsync(FestaCreateModel model, CancellationToken cancellationToken)
    {
        var entity = new Festa
        {
            DataInizio = model.DataInizio,
            DataFine = model.DataFine,
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
            logger.LogWarning(ex, "Error creating Festa");

            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Fail(CustomFailureReasons.InvalidRequest);
        }

        var createdModel = new FestaModel
        {
            Id = entity.Id,
            DataInizio = entity.DataInizio,
            DataFine = entity.DataFine,
        };

        return createdModel;
    }

    public async Task<Result<FestaModel>> UpdateFestaAsync(int id, FestaUpdateModel model, CancellationToken cancellationToken)
    {
        var query = await repository.GetByIdAsync(id, cancellationToken);

        if (query is null)
        {
            logger.LogWarning("Festa with ID {Id} not found for update.", id);
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        query.DataInizio = model.DataInizio;
        query.DataFine = model.DataFine;

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            repository.Update(query);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error updating Festa with id {Id}", id);

            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Fail(CustomFailureReasons.InvalidRequest);
        }

        var updatedModel = new FestaModel
        {
            Id = query.Id,
            DataInizio = query.DataInizio,
            DataFine = query.DataFine,
        };

        return updatedModel;
    }

    public async Task<Result<bool>> DeleteFestaAsync(int id, CancellationToken cancellationToken)
    {
        var query = await repository.GetByIdAsync(id, cancellationToken);

        if (query is null)
        {
            logger.LogWarning("Festa with ID {Id} not found for deletion.", id);
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
            logger.LogWarning(ex, "Error deleting Festa with id {Id}", id);

            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Fail(FailureReasons.InvalidContent);
        }

        return true;
    }
}