using BradyCodeChallenge.DAL.Model.Output;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Microsoft.Extensions.Configuration;

namespace BradyCodeChallenge.Test
{
    public class UnitTestGenerateOutput : IClassFixture<BaseTest>
    {
        private readonly IHost _host;
        // Constructor that sets up the test environment using the BaseTest fixture
        public UnitTestGenerateOutput(BaseTest baseTest)
        {
            _host = baseTest.TestHost;
        }

        // Test method to validate the CreateOutputFile method in the GenerationOutputRepository
        [Fact]
        public void CreateOutputFile_ShouldSucceed()
        {
            // Arrange
            // Creating a sample GenerationOutput object for testing
            var generationOutput = new GenerationOutput
            {
                Totals = new Totals
                {
                    Generator = new List<Generator>
                    {
                        new Generator
                        {
                            Name = "Wind[Offshore]",
                            Total = 1662.617445705M
                        },
                        new Generator
                        {
                            Name = "Wind[Onshore]",
                            Total = 4869.453917394M
                        },
                        new Generator
                        {
                            Name = "Gas[1]",
                            Total = 8512.254605520M
                        },
                        new Generator
                        {
                            Name = "Coal[1]",
                            Total = 5341.716526632M
                        }
                    }
                },
                MaxEmissionGenerators = new MaxEmissionGenerators
                {
                    Days = new List<Day>
                    {
                        new Day
                        {
                            Name = "Coal[1]",
                            Date = DateTime.Parse("2017-01-01T00:00:00+00:00"),
                            Emission = 137.175004008M
                        },
                        new Day
                        {
                            Name = "Coal[1]",
                            Date = DateTime.Parse("2017-01-02T00:00:00+00:00"),
                            Emission = 136.440767624M
                        },
                        new Day
                        {
                            Name = "Gas[1]",
                            Date = DateTime.Parse("2017-01-03T00:00:00+00:00"),
                            Emission = 5.132380700M
                        }
                    }
                },
                ActualHeatRates = new ActualHeatRates
                {
                    ActualHeatRateList = new List<ActualHeatRate>
                    {
                        new ActualHeatRate
                        {
                            Name = "Coal[1]",
                            HeatRate = 1
                        }
                    }
                }
            };

            // Retrieving configuration and output folder path from the host services
            var configuration = _host.Services.GetService<IConfiguration>();
            var outputFolderPath = configuration.GetRequiredSection("OutputFolderPath");

            // Generating a unique file name for the test output file
            Guid guid = Guid.NewGuid();
            var fileNameWithoutExtension = "Test" + guid.ToString();
            var destinationFilePath = outputFolderPath.Value + "\\" + fileNameWithoutExtension + "-Result.xml";

            // Retrieving GenerationOutputRepository from the host services
            var generationOutputRepository = _host.Services.GetService<IGenerationOutputRepository>();

            try
            {
                // Act
                // Creating the output file using the GenerationOutputRepository
                generationOutputRepository.Create(generationOutput, outputFolderPath.Value, fileNameWithoutExtension);

                // Assert
                // Verifying that the output file is created at the specified path
                Assert.True(File.Exists(destinationFilePath), "Output file is NOT created!");
            }
            finally
            {
                // Cleanup
                // Deleting the created output file after the test
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                    Assert.True(!File.Exists(destinationFilePath), "Output file is NOT removed!");
                }
            }
        }
    }
}
