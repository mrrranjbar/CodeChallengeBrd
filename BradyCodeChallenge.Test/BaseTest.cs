using BradyCodeChallenge.DAL.Model.Inputs;
using BradyCodeChallenge.DAL.Model.Output;
using BradyCodeChallenge.DAL.Model.Static;
using BradyCodeChallenge.Services.Core;
using BradyCodeChallenge.Services.General;
using BradyCodeChallenge.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BradyCodeChallenge.Test
{
    public class BaseTest
    {
        // The test host for running and testing the application
        public IHost TestHost { get; }

        // Constructor that initializes the test host
        public BaseTest()
        {
            TestHost = CreateHostBuilder().Build();
            Task.Run(() => TestHost.RunAsync());
        }

        // Creates a default host builder for the application
        public static IHostBuilder CreateHostBuilder(string[] args = null) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // Configures the application's configuration sources, adding JSON and XML files
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddXmlFile("ReferenceData.xml", optional: false, reloadOnChange: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Configures services for dependency injection
                    services.AddOptions();
                    services.Configure<Factors>(hostContext.Configuration.GetRequiredSection(key: nameof(Factors)));

                    // Configures an in-memory database for the GeneratorInfoDbContext
                    services.AddDbContext<GeneratorInfoDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("GeneratorInfoDB");
                    });

                    // Registers various repositories and services for dependency injection
                    services.AddTransient<IGenerationReportRepository, GenerationReportRepository>();
                    services.AddTransient<IGenerationOutputRepository, GenerationOutputRepository>();
                    services.AddTransient<IGeneratorInfoRepository, GeneratorInfoRepository>();
                    services.AddTransient<ICalculation, Calculation>();
                    services.AddSingleton<IWatcher, Watcher>();
                    services.AddTransient<IDataGenerator, DataGenerator>();
                });
    }
}
