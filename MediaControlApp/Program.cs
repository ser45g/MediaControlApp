using MediaControlApp.Application.Services;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Commands.Ganres;
using MediaControlApp.Commands.Medias;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Infrastructure.DataAccess.MediaStore;
using MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories;
using MediaControlApp.Infrasturcture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;

using Spectre.Console.Cli;


IServiceCollection services = null!;

IHost _host = Host.CreateDefaultBuilder().ConfigureServices(s =>
{
   
    s.AddDbContext<MediaDbContext>(builder => {
        builder.UseSqlite("Data Source=db.db");
    },contextLifetime:ServiceLifetime.Transient);

    s.AddTransient<IAuthorRepo,AuthorRepo>();
    s.AddTransient<IGanreRepo,GanreRepo>();
    s.AddTransient<IMediaRepo,MediaRepo>();
    s.AddTransient<IMediaTypeRepo,MediaTypeRepo>();

    s.AddTransient<AuthorService>();
    s.AddTransient<GanreService>();
    s.AddTransient<MediaService>();
    s.AddTransient<MediaTypeService>();

    services = s;
}).ConfigureLogging(builder =>
{
    builder.ClearProviders();
}).Build();


var registrar = new DITypeRegistar(services);

var app = new CommandApp(registrar);

DbContextOptions<MediaDbContext> options = new DbContextOptionsBuilder<MediaDbContext>().UseSqlite("Data Source=db.db").Options;

using (var context = new MediaDbContext(options))
{
    context.Database.Migrate();
}

app.Configure(config =>
{
    config.SetApplicationName("favorite-media");
    config.ValidateExamples();
    config.AddExample("media-type","add","MUSIC");
    


    config.AddBranch<CommandSettings>("media-type", mediaType =>
    {
        mediaType.SetDescription("Working with media types");
        mediaType.AddCommand<AddMediaTypeCommand>("add");
        mediaType.AddCommand<RemoveMediaTypeCommand>("remove");
        mediaType.AddCommand<ShowMediaTypesCommand>("show");
        mediaType.AddCommand<UpdateMediaTypeCommand>("update");
    });

    config.AddBranch<CommandSettings>("author", author=>
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
    });

    app.Configure(config =>
    {
        config.SetExceptionHandler((ex, resolver) =>
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.NoStackTrace|ExceptionFormats.ShortenEverything);

            // Return specific exit codes based on exception type
            return 1;
        });
    });


});

_host.Start();


Console.CancelKeyPress+= (sender, e) =>
{
    e.Cancel = true; // Prevent default behavior of terminating immediately
    AnsiConsole.MarkupLine("[yellow]\nCaught CTRL+C. Shutting down gracefully.[/]");

    // Exit the application
    Environment.Exit(0); // Pass appropriate exit code (0 means success).
};



await app.RunAsync(args);



