using MediaControlApp.Application.Services;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Commands.Ganres;
using MediaControlApp.Commands.Medias;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Commands.Statistics;
using MediaControlApp.Helpers;
using MediaControlApp.Infrastructure.DataAccess.MediaStore;
using MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories;
using MediaControlApp.Infrastructure.DataAccess.MediaStore.Utility;
using MediaControlApp.Interceptors;
using MediaControlApp.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Spectre.Console;
using Spectre.Console.Cli;

var logger = new LoggerConfiguration()
    .MinimumLevel.Fatal().MinimumLevel.Override("MediaControlApp",LogEventLevel.Information)
    .WriteTo.File("logs.txt")
    .CreateLogger();
Log.Logger = logger;

IServiceCollection services = null!;

IHost _host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, s) =>
{
    string? mediaDbConnectionString = hostContext.Configuration.GetConnectionString("MediaDb");
    if (mediaDbConnectionString == null)
    {
        throw new ArgumentNullException(nameof(mediaDbConnectionString));
    }
    s.AddDbContext<MediaDbContext>(builder => {
        builder.UseSqlite("Data Source=db.db");
    });
   
    s.AddScoped<IAuthorRepo,AuthorRepo>();
    s.AddScoped<IGanreRepo,GanreRepo>();
    s.AddScoped<IMediaRepo,MediaRepo>();
    s.AddScoped<IMediaTypeRepo,MediaTypeRepo>();

    s.AddTransient<IGanreValidationUtils, GanreValidationUtils>();
    s.AddTransient<IAuthorValidationUtils, AuthorValidationUtils>();
    s.AddTransient<IMediaTypeValidationUtils, MediaTypeValidationUtils>();
    s.AddTransient<IMediaValidationUtils, MediaValidationUtils>();
    s.AddTransient<ISharedValidatorUtils, SharedValidatorUtils>();

    s.AddScoped<IAuthorService,AuthorService>();
    s.AddScoped<IGanreService, GanreService>();
    s.AddScoped<IMediaService, MediaService>();
    s.AddScoped<IMediaTypeService, MediaTypeService>();

    s.AddScoped<IUtilityDataRepo,UtilityDataRepo>();

    services = s;
}).UseSerilog(logger).Build();

var registrar = new DITypeRegistar(services);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("favorite-media");
    config.ValidateExamples();
    config.AddExample("media-type","add","MUSIC");

    #if DEBUG

    config.SetInterceptor(new TimingInterceptor());
    
    #endif

    var logger = _host.Services.GetService<ILogger<LoggingInterceptor>>();

    if (logger == null)
    {
        throw new ArgumentNullException(nameof(logger));
    }
    config.SetInterceptor(new LoggingInterceptor(logger));

    config.AddBranch<CommandSettings>("media-type", mediaType =>
    {
        mediaType.SetDescription("Working with media types");
        mediaType.AddCommand<AddMediaTypeCommand>("add");
        mediaType.AddCommand<RemoveMediaTypeCommand>("remove");
        mediaType.AddCommand<ShowMediaTypesCommand>("show");
        mediaType.AddCommand<UpdateMediaTypeCommand>("update");
    });

    config.AddBranch<CommandSettings>("author", author =>
    {
        author.SetDescription("Working with authors");
        author.AddCommand<AddAuthorCommand>("add");
        author.AddCommand<RemoveAuthorCommand>("remove");
        author.AddCommand<ShowAuthorsCommand>("show");
        author.AddCommand<UpdateAuthorCommand>("update");
    });

    config.AddBranch<CommandSettings>("ganre", ganre =>
    {
        ganre.SetDescription("Working with ganres");
        ganre.AddCommand<AddGanreCommand>("add");
        ganre.AddCommand<RemoveGanreCommand>("remove");
        ganre.AddCommand<ShowGanresCommand>("show");
        ganre.AddCommand<UpdateGanreCommand>("update");
    });

    config.AddBranch<CommandSettings>("media", media =>
    {
        media.SetDescription("Working with medias");
        media.AddCommand<AddMediaCommand>("add");
        media.AddCommand<RemoveMediaCommand>("remove");
        media.AddCommand<ShowMediasCommand>("show");
        media.AddCommand<UpdateMediaCommand>("update");
        media.AddCommand<SetMediaConsumedCommand>("set-consumed");
    });

    config.AddBranch<CommandSettings>("statistics", statisctics =>
    {
        statisctics.AddCommand<ShowChartOfElementsCommand>("show");
    });
  
    app.Configure(config =>
    {
        config.SetExceptionHandler((ex, resolver) =>
        {
            IAnsiConsole? _ansiConsole = (IAnsiConsole)resolver.Resolve(typeof(IAnsiConsole));
            _ansiConsole?.WriteException(ex, ExceptionFormats.NoStackTrace|ExceptionFormats.ShortenEverything);

            return 1;
        });
    });


});

// Create a cancellation token source to handle Ctrl+C
var cancellationTokenSource = new CancellationTokenSource();

// Wire up Console.CancelKeyPress to trigger cancellation
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true; // Prevent immediate process termination
    cancellationTokenSource.Cancel();
    Console.WriteLine("Cancellation requested...");
};

_host.Start();


using (var context = _host.Services.GetService<MediaDbContext>())
{
    if(context == null)
    {
        throw new ArgumentNullException(nameof(context));
    }
    context.Database.Migrate();
}


await app.RunAsync(args, cancellationToken:cancellationTokenSource.Token);



