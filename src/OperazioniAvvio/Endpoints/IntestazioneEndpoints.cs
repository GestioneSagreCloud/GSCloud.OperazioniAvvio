namespace OperazioniAvvio.Api.Endpoints;

public class IntestazioneEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var versionedApi = endpoints.NewVersionedApi().ReportApiVersions();

        var versionNeutralApi = versionedApi.MapGroup("intestazione").IsApiVersionNeutral();
        var intestazioneGroup = versionNeutralApi.WithTags("Intestazioni Endpoints");

        intestazioneGroup.MapGet(string.Empty, async (HttpContext httpContext, IIntestazioneService intestazioneService, CancellationToken cancellationToken) =>
        {
            var result = await intestazioneService.GetIntestazioniFesteAsync(cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<IEnumerable<IntestazioneModel>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("GetIntestazioniFeste")
        .WithDescription("Recupera tutte le intestazioni delle feste disponibili.")
        .WithSummary("Recupera tutte le intestazioni delle feste disponibili.");

        intestazioneGroup.MapGet("{id:int}", async (HttpContext httpContext, IIntestazioneService intestazioneService, int id, CancellationToken cancellationToken) =>
        {
            var result = await intestazioneService.GetIntestazioneFestaByIdAsync(id, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<IntestazioneModel>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("GetIntestazioneFestaById")
        .WithDescription("Recupera un'intestazione della festa specifica per ID.")
        .WithSummary("Recupera un'intestazione della festa specifica per ID.");

        intestazioneGroup.MapPost(string.Empty, async (HttpContext httpContext, IntestazioneCreateModel model, IIntestazioneService intestazioneService, CancellationToken cancellationToken) =>
        {
            var result = await intestazioneService.CreateIntestazioneFestaAsync(model, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<IntestazioneModel>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity, MediaTypeNames.Application.Json)
        .WithName("CreateIntestazioneFesta")
        .WithDescription("Crea una nuova intestazione della festa.")
        .WithSummary("Crea una nuova intestazione della festa.")
        .WithValidation<IntestazioneCreateModel>();

        intestazioneGroup.MapPut("{id:int}", async (HttpContext httpContext, int id, IntestazioneUpdateModel model, IIntestazioneService intestazioneService, CancellationToken cancellationToken) =>
        {
            var result = await intestazioneService.UpdateIntestazioneFestaAsync(id, model, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<IntestazioneModel>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity, MediaTypeNames.Application.Json)
        .WithName("UpdateIntestazioneFesta")
        .WithDescription("Aggiorna un'intestazione della festa esistente.")
        .WithSummary("Aggiorna un'intestazione della festa esistente.")
        .WithValidation<IntestazioneUpdateModel>();

        intestazioneGroup.MapDelete("{id:int}", async (HttpContext httpContext, int id, IIntestazioneService intestazioneService, CancellationToken cancellationToken) =>
        {
            var result = await intestazioneService.DeleteIntestazioneFestaAsync(id, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<IntestazioneModel>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("DeleteIntestazioneFesta")
        .WithDescription("Elimina un'intestazione della festa esistente.")
        .WithSummary("Elimina un'intestazione della festa esistente.");
    }
}