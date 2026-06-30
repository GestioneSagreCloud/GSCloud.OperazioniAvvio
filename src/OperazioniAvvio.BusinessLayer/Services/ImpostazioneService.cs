namespace OperazioniAvvio.BusinessLayer.Services;

public class ImpostazioneService(ILogger<ImpostazioneService> logger, IRepository<Impostazione, int> repository, IUnitOfWork unitOfWork) : IImpostazioneService
{
    public async Task<Result<IEnumerable<ImpostazioneModel>>> GetImpostazioniFesteAsync(CancellationToken cancellationToken)
    {
        var query = await repository.GetAllAsync(cancellationToken);

        if (query is null)
        {
            logger.LogWarning("No Impostazioni found in the database.");
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        var result = query.Select(x => new ImpostazioneModel
        {
            Id = x.Id,
            IdFesta = x.IdFesta,
            GestioneCategorie = x.GestioneCategorie,
            GestioneCoperti = x.GestioneCoperti,
            GestioneMenu = x.GestioneMenu,
            StampaCarta = x.StampaCarta,
            StampaRicevuta = x.StampaRicevuta,
            StampaLogo = x.StampaLogo
        }).ToList();

        return result;
    }

    public async Task<Result<ImpostazioneModel>> GetImpostazioneFestaByIdAsync(int id, CancellationToken cancellationToken)
    {
        var query = await repository.GetByIdAsync(id, cancellationToken);

        if (query is null)
        {
            logger.LogWarning("Impostazione with ID {Id} not found.", id);
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        var result = new ImpostazioneModel
        {
            Id = query.Id,
            IdFesta = query.IdFesta,
            GestioneCategorie = query.GestioneCategorie,
            GestioneCoperti = query.GestioneCoperti,
            GestioneMenu = query.GestioneMenu,
            StampaCarta = query.StampaCarta,
            StampaRicevuta = query.StampaRicevuta,
            StampaLogo = query.StampaLogo
        };

        return result;
    }

    public async Task<Result<ImpostazioneModel>> CreateImpostazioneFestaAsync(ImpostazioneCreateModel model, CancellationToken cancellationToken)
    {
        var entity = new Impostazione
        {
            IdFesta = model.IdFesta,
            GestioneCategorie = model.GestioneCategorie,
            GestioneCoperti = model.GestioneCoperti,
            GestioneMenu = model.GestioneMenu,
            StampaCarta = model.StampaCarta,
            StampaRicevuta = model.StampaRicevuta,
            StampaLogo = model.StampaLogo
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
            logger.LogWarning(ex, "Error creating Impostazione");

            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Fail(CustomFailureReasons.InvalidRequest);
        }

        var createdModel = new ImpostazioneModel
        {
            Id = entity.Id,
            IdFesta = entity.IdFesta,
            GestioneCategorie = entity.GestioneCategorie,
            GestioneCoperti = entity.GestioneCoperti,
            GestioneMenu = entity.GestioneMenu,
            StampaCarta = entity.StampaCarta,
            StampaRicevuta = entity.StampaRicevuta,
            StampaLogo = entity.StampaLogo
        };

        return createdModel;
    }

    public async Task<Result<ImpostazioneModel>> UpdateImpostazioneFestaAsync(int id, ImpostazioneUpdateModel model, CancellationToken cancellationToken)
    {
        var query = await repository.GetByIdAsync(id, cancellationToken);

        if (query is null)
        {
            logger.LogWarning("Impostazione with ID {Id} not found for update.", id);
            return Result.Fail(FailureReasons.ItemNotFound);
        }

        query.IdFesta = model.IdFesta;
        query.GestioneCategorie = model.GestioneCategorie;
        query.GestioneCoperti = model.GestioneCoperti;
        query.GestioneMenu = model.GestioneMenu;
        query.StampaCarta = model.StampaCarta;
        query.StampaRicevuta = model.StampaRicevuta;
        query.StampaLogo = model.StampaLogo;

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            repository.Update(query);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error updating Impostazione with id {Id}", id);

            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Fail(CustomFailureReasons.InvalidRequest);
        }

        var updatedModel = new ImpostazioneModel
        {
            Id = query.Id,
            IdFesta = query.IdFesta,
            GestioneCategorie = query.GestioneCategorie,
            GestioneCoperti = query.GestioneCoperti,
            GestioneMenu = query.GestioneMenu,
            StampaCarta = query.StampaCarta,
            StampaRicevuta = query.StampaRicevuta,
            StampaLogo = query.StampaLogo
        };

        return updatedModel;
    }

    public async Task<Result<bool>> DeleteImpostazioneFestaAsync(int id, CancellationToken cancellationToken)
    {
        var query = await repository.GetByIdAsync(id, cancellationToken);

        if (query is null)
        {
            logger.LogWarning("Impostazione with ID {Id} not found for deletion.", id);
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
            logger.LogWarning(ex, "Error deleting Impostazione with id {Id}", id);

            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return Result.Fail(FailureReasons.InvalidContent);
        }

        return true;
    }
}