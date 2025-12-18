using MediaControlApp.Commands.Add;
using MediaControlApp.Commands.Run;
using MediaControlApp.Commands.Serve;
using Spectre.Console.Cli;

var app = new Spectre.Console.Cli.CommandApp();
app.Configure(config =>
{

    config.SetApplicationName("favorite-media");
    config.ValidateExamples();
    config.AddExample("add","album", "");

    // Run
    config.AddCommand<RunCommand>("run");

    // Add
    config.AddBranch<AddSettings>("add", add =>
    {
        add.SetDescription("Add a package or reference to a .NET project");
        add.AddCommand<AddPackageCommand>("package");
        add.AddCommand<AddReferenceCommand>("reference");
    });

    // Serve
    config.AddCommand<ServeCommand>("serve")
        .WithExample("serve", "-o", "firefox")
        .WithExample("serve", "--port", "80", "-o", "firefox");
});


await app.RunAsync(args);

