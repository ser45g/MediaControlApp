using MediaControlApp.Application.Services;
using MediaControlApp.Application.Services.Interfaces;
using MediaControlApp.Commands.Add;
using MediaControlApp.Commands.MediaTypes;
using MediaControlApp.Commands.Run;
using MediaControlApp.Commands.Serve;
using MediaControlApp.Infrastructure.DataAccess.MediaStore;
using MediaControlApp.Infrastructure.DataAccess.MediaStore.Repositories;
using MediaControlApp.Infrasturcture;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

    // Run
    config.AddCommand<RunCommand>("run");

    // Add
    config.AddBranch<AddSettings>("add", add =>
    {
        add.SetDescription("Add a package or reference to a .NET project");
        add.AddCommand<AddPackageCommand>("package");
        add.AddCommand<AddReferenceCommand>("reference");
    });

    config.AddBranch<CommandSettings>("media-type", mediaType =>
    {
        mediaType.SetDescription("Working with media types");
        mediaType.AddCommand<AddMediaTypeCommand>("add");
        mediaType.AddCommand<RemoveMediaTypeCommand>("remove");
        mediaType.AddCommand<ShowMediaTypesCommand>("show");
        mediaType.AddCommand<UpdateMediaTypeCommand>("update");
    });
   

    // Serve
    config.AddCommand<ServeCommand>("serve")
        .WithExample("serve", "-o", "firefox")
        .WithExample("serve", "--port", "80", "-o", "firefox");
});

_host.Start();
await app.RunAsync(args);

