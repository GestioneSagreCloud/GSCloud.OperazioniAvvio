namespace OperazioniAvvio.Api.Endpoints;

public class FestaEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var versionedApi = endpoints.NewVersionedApi().ReportApiVersions();

        var versionNeutralApi = versionedApi.MapGroup("festa").IsApiVersionNeutral();
        var festaGroup = versionNeutralApi.WithTags("Feste Endpoints");

        festaGroup.MapGet(string.Empty, async (HttpContext httpContext, IFestaService festaService, CancellationToken cancellationToken) =>
        {
            var result = await festaService.GetFesteAsync(cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<IEnumerable<FestaModel>>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("GetFeste")
        .WithDescription("Recupera tutte le feste disponibili.")
        .WithSummary("Recupera tutte le feste disponibili.");

        festaGroup.MapGet("{id:int}", async (HttpContext httpContext, int id, IFestaService festaService, CancellationToken cancellationToken) =>
        {
            var result = await festaService.GetFestaByIdAsync(id, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<FestaModel>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("GetFestaById")
        .WithDescription("Recupera una festa specifica in base all'ID.")
        .WithSummary("Recupera una festa specifica in base all'ID.");

        festaGroup.MapPost(string.Empty, async (HttpContext httpContext, FestaCreateModel model, IFestaService festaService, CancellationToken cancellationToken) =>
        {
            var result = await festaService.CreateFestaAsync(model, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<FestaModel>(StatusCodes.Status201Created, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity, MediaTypeNames.Application.Json)
        .WithName("CreateFesta")
        .WithDescription("Crea una nuova festa.")
        .WithSummary("Crea una nuova festa.")
        .WithValidation<FestaCreateValidation>();

        festaGroup.MapPut("{id:int}", async (HttpContext httpContext, int id, FestaUpdateModel model, IFestaService festaService, CancellationToken cancellationToken) =>
        {
            var result = await festaService.UpdateFestaAsync(id, model, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces<FestaModel>(StatusCodes.Status200OK, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status400BadRequest, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .ProducesProblem(StatusCodes.Status422UnprocessableEntity, MediaTypeNames.Application.Json)
        .WithName("UpdateFesta")
        .WithDescription("Aggiorna una festa esistente.")
        .WithSummary("Aggiorna una festa esistente.")
        .WithValidation<FestaUpdateValidation>();

        festaGroup.MapDelete("{id:int}", async (HttpContext httpContext, int id, IFestaService festaService, CancellationToken cancellationToken) =>
        {
            var result = await festaService.DeleteFestaAsync(id, cancellationToken);
            var response = httpContext.CreateResponse(result);

            return response;
        })
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound, MediaTypeNames.Application.Json)
        .WithName("DeleteFesta")
        .WithDescription("Elimina una festa esistente.")
        .WithSummary("Elimina una festa esistente.");
    }
}