// Importing necessary namespaces
using BradyCodeChallenge.DAL.Model.Error;
using BradyCodeChallenge.DAL.Model.Inputs;
using BradyCodeChallenge.DAL.Model.Output;
using BradyCodeChallenge.DAL.Model.Static;
using BradyCodeChallenge.Services;
using BradyCodeChallenge.Services.Core;
using BradyCodeChallenge.Services.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Creating a host application builder
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Adding an XML configuration file to the application configuration
builder.Configuration.AddXmlFile("ReferenceData.xml", optional: false, reloadOnChange: false);

// Binding the 'Factors' class and the XML file
builder.Services.Configure<Factors>(builder.Configuration.GetRequiredSection(key: nameof(Factors)));

// Registering DbContext for Entity Framework Core with an in-memory database
builder.Services.AddDbContext<GeneratorInfoDbContext>(options =>
{
    options.UseInMemoryDatabase("GeneratorInfoDB");
});

// Registering various services and repositories for dependency injection
builder.Services.AddTransient<IGenerationReportRepository, GenerationReportRepository>();
builder.Services.AddTransient<IGenerationOutputRepository, GenerationOutputRepository>();
builder.Services.AddTransient<IGeneratorInfoRepository, GeneratorInfoRepository>();
builder.Services.AddTransient<ICalculation, Calculation>();
builder.Services.AddSingleton<IWatcher, Watcher>();
builder.Services.AddTransient<IDataGenerator, DataGenerator>();

// Building the application host
using IHost host = builder.Build();

try
{
    // Call the DataGenerator to create static data
    IDataGenerator dataGenerator = host.Services.GetRequiredService<IDataGenerator>();
    dataGenerator.Initialize();

    // Attempting to start the 'Watcher' service
    IWatcher watcher = host.Services.GetRequiredService<IWatcher>();
    watcher.StartWatching();
}
catch(Exception ex)
{
    // Handling exceptions by printing an error message
    ErrorContent.PrintError(new ErrorContent() { Message = $"{ex.Message}" });
}

// Starting the application host and asynchronously waiting for it to complete
await host.RunAsync();

