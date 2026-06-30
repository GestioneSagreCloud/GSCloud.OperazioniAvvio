namespace OperazioniAvvio.Api.Endpoints;

public class ImpostazioneEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var versionedApi = endpoints.NewVersionedApi().ReportApiVersions();

        var versionNeutralApi = versionedApi.MapGroup("impostazione").IsApiVersionNeutral();
        var impostazioneGroup = versionNeutralApi.WithTags("Impostazioni Endpoints");

        impostazioneGroup.MapGet(string.Empty, async (HttpContext httpContext, IImpostazioneService impostazioneService, CancellationToken cancellationToken) =>
        {
            var result = await impostazioneService.GetImpostazioniFesteAsync(cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<IEnumerable<ImpostazioneModel>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("GetImpostazioniFeste")
        .WithDescription("Recupera tutte le impostazioni delle feste disponibili.")
        .WithSummary("Recupera tutte le impostazioni delle feste disponibili.");

        impostazioneGroup.MapGet("{id:int}", async (HttpContext httpContext, IImpostazioneService impostazioneService, int id, CancellationToken cancellationToken) =>
        {
            var result = await impostazioneService.GetImpostazioneFestaByIdAsync(id, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<ImpostazioneModel>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("GetImpostazioneFestaById")
        .WithDescription("Recupera un'impostazione della festa specifica per ID.")
        .WithSummary("Recupera un'impostazione della festa specifica per ID.");

        impostazioneGroup.MapPost(string.Empty, async (HttpContext httpContext, ImpostazioneCreateModel model, IImpostazioneService impostazioneService, CancellationToken cancellationToken) =>
        {
            var result = await impostazioneService.CreateImpostazioneFestaAsync(model, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<ImpostazioneModel>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity, MediaTypeNames.Application.Json)
        .WithName("CreateImpostazioneFesta")
        .WithDescription("Crea una nuova impostazione della festa.")
        .WithSummary("Crea una nuova impostazione della festa.")
        .WithValidation<ImpostazioneCreateModel>();

        impostazioneGroup.MapPut("{id:int}", async (HttpContext httpContext, int id, ImpostazioneUpdateModel model, IImpostazioneService impostazioneService, CancellationToken cancellationToken) =>
        {
            var result = await impostazioneService.UpdateImpostazioneFestaAsync(id, model, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<ImpostazioneModel>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity, MediaTypeNames.Application.Json)
        .WithName("UpdateImpostazioneFesta")
        .WithDescription("Aggiorna un'impostazione della festa esistente.")
        .WithSummary("Aggiorna un'impostazione della festa esistente.")
        .WithValidation<ImpostazioneUpdateModel>();

        impostazioneGroup.MapDelete("{id:int}", async (HttpContext httpContext, int id, IImpostazioneService impostazioneService, CancellationToken cancellationToken) =>
        {
            var result = await impostazioneService.DeleteImpostazioneFestaAsync(id, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<ImpostazioneModel>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("DeleteImpostazioneFesta")
        .WithDescription("Elimina un'impostazione della festa esistente.")
        .WithSummary("Elimina un'impostazione della festa esistente.");
    }
}