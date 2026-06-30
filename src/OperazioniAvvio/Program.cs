namespace OperazioniAvvio.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

        builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            loggerConfiguration.ReadFrom.Services(services);
        });

        var appSettings = builder.Services.ConfigureAndGet<AppSettings>(builder.Configuration, nameof(AppSettings)) ?? new();
        var sqlServerConfiguration = builder.Configuration.GetConnectionString("SqlConnection")!;

        var toolDocumentation = appSettings.ApiDocumentationTool;

        var swaggerSettings = new SwaggerSettings();
        var scalarSettings = new ScalarSettings();

        if (toolDocumentation == ApiDocumentationTool.SwaggerUI)
        {
            swaggerSettings = builder.Services.ConfigureAndGet<SwaggerSettings>(builder.Configuration, nameof(SwaggerSettings)) ?? new();
        }
        else if (toolDocumentation == ApiDocumentationTool.Scalar)
        {
            scalarSettings = new ScalarSettings()
            {
                Title = $"{builder.Environment.ApplicationName} API Reference",
                Theme = ScalarTheme.BluePlanet,
            };
        }

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<ILogEventEnricher, HttpContextEnricher>();

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddSingleton<ClientTimeProvider>();

        builder.Services.AddSingleton<ITimeZoneService, TimeZoneService>();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(sqlServerConfiguration, opt =>
            {
                opt.CommandTimeout(60);
                opt.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);

                opt.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                opt.UseCompatibilityLevel(160);
            });

            if (builder.Environment.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }

            options.LogTo(Console.WriteLine, LogLevel.Information);
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

            options.UseExceptionProcessor();
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        builder.Services.AddPersistenceUoW<Festa, int, ApplicationDbContext>();
        builder.Services.AddPersistenceUoW<Intestazione, int, ApplicationDbContext>();

        builder.Services.AddPersistenceUoW<Impostazione, int, ApplicationDbContext>();
        builder.Services.AddTransient<IFestaService, FestaService>();

        builder.Services.AddTransient<IIntestazioneService, IntestazioneService>();
        builder.Services.AddTransient<IImpostazioneService, ImpostazioneService>();

        builder.Services.AddOperationResult(options =>
        {
            options.ErrorResponseFormat = ErrorResposeFormatOR.List;

            options.StatusCodesMapping.Add(CustomFailureReasons.NotAvailable, StatusCodes.Status501NotImplemented);
            options.StatusCodesMapping.Add(CustomFailureReasons.InvalidRequest, StatusCodes.Status400BadRequest);
        });

        builder.Services.AddHttpJsonOptions();
        //builder.Services.AddSimpleAuthentication(builder.Configuration, "JwtSettings");

        builder.Services.AddRequestLocalization(appSettings.SupportedCultures.Distinct().ToArray());
        builder.Services.AddResponseCompression();

        ValidatorOptions.Global.LanguageManager.Enabled = false;

        builder.Services.AddValidatorsFromAssemblyContaining<FestaCreateValidation>();
        builder.Services.ConfigureValidation(options =>
        {
            options.ErrorResponseFormat = ErrorResponseFormat.List;
            options.ValidationErrorTitleMessageFactory = (context, errors)
                => $"There was {errors.Values.Sum(v => v.Length)} validation error(s) occurred";
        });

        builder.Services.AddOpenApiOperationParameters(options =>
        {
            options.Parameters.Add(new()
            {
                Name = TimeZoneService.HeaderKey,
                In = ParameterLocation.Header,
                Required = false,
                Schema = OpenApiSchemaHelper.CreateStringSchema()
            });
        });

        var apiPolicyOptions = new List<ApiPoliciesSettings>();
        var apiOptionSettings = new OpenApiOptionSettings()
        {
            AddAcceptLanguageHeader = true,
            AddOperationParameters = true
        };

        builder.Services.AddVersioningApi(appSettings.ApiVersions, builder.Configuration, apiOptionSettings, apiPolicyOptions, null!);
        builder.Services.AddDefaultProblemDetails();

        builder.Services.AddDefaultExceptionHandler();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyHeader()
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials().WithExposedHeaders(HeaderNames.ContentDisposition);
            });
        });

        var app = builder.Build();
        EFCoreDJ.ApplyMigrations<ApplicationDbContext>(app);

        app.UseForwardedHeaders(new()
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
            KnownProxies = { }
        });

        app.UseHttpsRedirection();
        app.UseExceptionHandler();

        app.MapOpenApi()
            //.AllowAnonymous()
            .WithDocumentPerVersion();

        AEToolDJ.MapDocumentationTool(appSettings, toolDocumentation, swaggerSettings, scalarSettings, app);

        // Enable serving default files like index.html from wwwroot folder
        //app.UseDefaultFiles();

        //app.UseDefaultFiles(); // Enable serving default files like index.html from wwwroot folder
        //app.UseStaticFiles(); // Enable serving static files from wwwroot folder

        app.UseRouting();
        app.UseCors();

        app.UseRequestLocalization();
        //app.UseAuthentication();

        app.UseSerilogRequestLogging(options =>
        {
            options.IncludeQueryInRequestPath = true;
        });

        //app.UseAuthorization();
        app.UseResponseCompression();

        app.UseResponseCaching();
        app.MapEndpoints();

        app.Run();
    }
}