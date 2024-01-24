using BradyCodeChallenge.DAL.Model.Enums;
using BradyCodeChallenge.DAL.Model.Inputs;
using BradyCodeChallenge.DAL.Model.Static;
using BradyCodeChallenge.Services.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;

namespace BradyCodeChallenge.Test
{
    public class UnitTestCalculator : IClassFixture<BaseTest>
    {
        private readonly IHost _host;
        // Constructor that sets up the test environment using the BaseTest fixture
        public UnitTestCalculator(BaseTest baseTest)
        {
            _host = baseTest.TestHost;
        }

        // Test method to validate the Calculate method in the Calculation class
        [Fact]
        public void Calculate_ShouldReturnGenerationOutput()
        {
            // Arrange
            // Setting up mock options for Factors
            var factorsOptions = new Factors
            {
                ValueFactor = new DAL.Model.Static.ValueFactor
                {
                    Low = 0.265M,
                    Medium = 0.696M,
                    High = 0.946M
                },
                EmissionsFactor = new EmissionsFactor
                {
                    Low = 0.312M,
                    Medium = 0.562M,
                    High = 0.812M
                }
            };
            var optionsMock = new Mock<IOptions<Factors>>();
            optionsMock.Setup(opt => opt.Value).Returns(factorsOptions);

            // Setting up mock GeneratorInfoRepository
            var generatorInfoRepositoryMock = new Mock<IGeneratorInfoRepository>();
            // Setup the mock to return appropriate GeneratorInfo objects
            generatorInfoRepositoryMock.Setup(repo => repo.Get(GeneratorType.Offshore)).Returns(new GeneratorInfo { GeneratorType = GeneratorType.Offshore, ValueFactor = DAL.Model.Enums.ValueFactor.Low, EmissionFactor = EmissionFactor.N_A });
            generatorInfoRepositoryMock.Setup(repo => repo.Get(GeneratorType.Onshore)).Returns(new GeneratorInfo { GeneratorType = GeneratorType.Onshore, ValueFactor = DAL.Model.Enums.ValueFactor.High, EmissionFactor = EmissionFactor.N_A });
            generatorInfoRepositoryMock.Setup(repo => repo.Get(GeneratorType.Gas)).Returns(new GeneratorInfo { GeneratorType = GeneratorType.Gas, ValueFactor = DAL.Model.Enums.ValueFactor.Medium, EmissionFactor = EmissionFactor.Medium });
            generatorInfoRepositoryMock.Setup(repo => repo.Get(GeneratorType.Coal)).Returns(new GeneratorInfo { GeneratorType = GeneratorType.Coal, ValueFactor = DAL.Model.Enums.ValueFactor.Medium, EmissionFactor = EmissionFactor.High });

            // Creating an instance of the Calculation class with the mock options and repository
            var calculation = new Calculation(optionsMock.Object, generatorInfoRepositoryMock.Object);

            // Creating a sample generation report for testing
            var generationReport = new GenerationReport
            {
                Wind = new Wind
                {
                    WindGenerators = new List<WindGenerator>
                    {
                        new WindGenerator
                        {
                            Name = "Wind[Offshore]",
                            Generations = new List<Generation>
                            {
                                new Generation()
                                {
                                   Days = new List<Day>
                                    {
                                        new Day
                                        {
                                            Date = DateTime.Parse("2017-01-01T00:00:00+00:00"),
                                            Energy = 100.368M,
                                            Price = 20.148M
                                        }
                                    }
                                }
                            },
                            Location = "Offshore"
                        },
                        new WindGenerator
                        {
                            Name = "Wind[Onshore]",
                            Generations = new List<Generation>
                            {
                                new Generation(){
                                    Days = new List<Day>
                                    {
                                        new Day
                                        {
                                            Date = DateTime.Parse("2017-01-01T00:00:00+00:00"),
                                            Energy = 56.578M,
                                            Price = 29.542M
                                        }
                                    }
                                }
                            },
                            Location = "Onshore"
                        }
                    }
                },
                Gas = new Gas
                {
                    GasGenerator = new List<GasGenerator>
                    {
                        new GasGenerator
                        {
                            Name = "Gas[1]",
                            Generations = new List<Generation>
                            {
                                new Generation(){
                                    Days = new List<Day>
                                    {
                                        new Day
                                        {
                                            Date = DateTime.Parse("2017-01-01T00:00:00+00:00"),
                                            Energy = 259.235M,
                                            Price = 15.837M
                                        }
                                    }
                                }
                            },
                            EmissionsRating = 0.038M
                        }
                    }
                },
                Coal = new Coal
                {
                    CoalGenerator = new List<CoalGenerator>
                    {
                        new CoalGenerator
                        {
                            Name = "Coal[1]",
                            Generations = new List<Generation>
                            {
                                new Generation()
                                {
                                     Days = new List<Day>
                                {
                                    new Day
                                    {
                                        Date = DateTime.Parse("2017-01-01T00:00:00+00:00"),
                                        Energy = 350.487M,
                                        Price = 10.146M
                                    }
                                }
                                }
                            },
                            TotalHeatInput = 11.815M,
                            ActualNetGeneration = 11.815M,
                            EmissionsRating = 0.482M
                        }
                    }
                }
            };

            // Act
            // Calling the Calculate method with the sample generation report
            var result = calculation.Calculate(generationReport);

            // Assert
            // Verifying that the result is not null
            Assert.NotNull(result);

            // more verifying can be here...
        }
    }
}
