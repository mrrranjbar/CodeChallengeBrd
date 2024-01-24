using BradyCodeChallenge.DAL.Model.Enums;
using BradyCodeChallenge.DAL.Model.Error;
using BradyCodeChallenge.DAL.Model.Inputs;
using BradyCodeChallenge.DAL.Model.Output;
using BradyCodeChallenge.DAL.Model.Static;
using Microsoft.Extensions.Options;

namespace BradyCodeChallenge.Services.Core
{
    public class Calculation : ICalculation
    {
        // Constructor that receives IOptions<Factors> for configuration settings and IGeneratorInfoRepository for CRUD operations
        private readonly Factors _options;
        private readonly IGeneratorInfoRepository _generatorInfoRepository;
        public Calculation(IOptions<Factors> options, IGeneratorInfoRepository generatorInfoRepository)
        {
            _options = options.Value;
            _generatorInfoRepository = generatorInfoRepository;
        }

        // Main calculation method to process the GenerationReport and produce GenerationOutput
        public GenerationOutput? Calculate(GenerationReport generationReport)
        {
            // Error handling for invalid input
            if (generationReport == null)
            {
                ErrorContent.PrintError(new ErrorContent() { Message = $"Input file is not correct! method name is: {nameof(Calculate)}" });
                return null;
            }

            // Initialize GenerationOutput and a list to store daily emissions
            GenerationOutput generationOutput = GenerationOutput.GetInstance();
            var daysAndEissions = new List<DAL.Model.Output.Day>();

            // Call methods to calculate values for wind, fuel
            WindGenerator(generationReport.Wind, generationOutput);
            FuelGenerator(generationReport, generationOutput, daysAndEissions);
            // Create report related to find max emission per day
            DaysAndEmissions(generationOutput, daysAndEissions);

            return generationOutput;
        }

        // Method to calculate values for gas and coal generators
        private void FuelGenerator(GenerationReport generationReport, GenerationOutput generationOutput, List<DAL.Model.Output.Day> daysAndEissions)
        {
            // Process gas generators
            var generatorInfo = _generatorInfoRepository.Get(GeneratorType.Gas);
            decimal emissionFactor = GetEmissionFactor(generatorInfo.ValueFactor);
            decimal valueFactor = GetEmissionFactor(generatorInfo.ValueFactor);
            GasGenerator(generationReport.Gas, valueFactor, emissionFactor, generationOutput, daysAndEissions);

            // Process coal generators
            generatorInfo = _generatorInfoRepository.Get(GeneratorType.Coal);
            emissionFactor = GetEmissionFactor(generatorInfo.ValueFactor);
            valueFactor = GetEmissionFactor(generatorInfo.ValueFactor);
            CoalGenerator(generationReport.Coal, valueFactor, emissionFactor, generationOutput, daysAndEissions);
        }

        // Method to process wind generators and calculate total generation values
        private void WindGenerator(Wind Wind, GenerationOutput generationOutput)
        {
            foreach (var windGenerator in Wind.WindGenerators)
            {
                // Determine the generator type based on the generator's name
                var generatorInfo = new GeneratorInfo();
                if (windGenerator.Name.ToLower().Contains(nameof(GeneratorType.Onshore).ToLower()))
                    generatorInfo = _generatorInfoRepository.Get(GeneratorType.Onshore);
                else if (windGenerator.Name.ToLower().Contains(nameof(GeneratorType.Offshore).ToString().ToLower()))
                    generatorInfo = _generatorInfoRepository.Get(GeneratorType.Offshore);

                // Get the value factor and calculate wind total generation value
                decimal valueFactor = GetValueFactor(generatorInfo.ValueFactor);
                WindTotalGenerationValue(windGenerator, valueFactor, generationOutput);
            }
        }

        // Method to calculate the total generation value for a wind generator
        private void WindTotalGenerationValue(WindGenerator windGenerator, decimal valueFactor, GenerationOutput generationOutput)
        {
            // Calculate total generation value based on energy, price, and value factor
            decimal totalGenerationValue = 0;
            foreach (var generation in windGenerator.Generations)
            {
                foreach (var day in generation.Days)
                {
                    totalGenerationValue += day.Energy * day.Price * valueFactor;
                }
            }

            // Add the total generation value to the GenerationOutput
            generationOutput.Totals.Generator.Add(new DAL.Model.Output.Generator()
            {
                Name = windGenerator.Name,
                Total = totalGenerationValue
            });
        }

        // Method to get the value factor based on the specified factor type
        private decimal GetValueFactor(DAL.Model.Enums.ValueFactor factorType)
        {
            decimal valueFactor = 1;

            switch (factorType)
            {
                case DAL.Model.Enums.ValueFactor.Low:
                    valueFactor = _options.ValueFactor.Low;
                    break;
                case DAL.Model.Enums.ValueFactor.Medium:
                    valueFactor = _options.ValueFactor.Medium;
                    break;
                case DAL.Model.Enums.ValueFactor.High:
                    valueFactor = _options.ValueFactor.High;
                    break;
                default: return valueFactor;
            }

            return valueFactor;
        }

        // Method to get the emission factor based on the specified factor type
        private decimal GetEmissionFactor(DAL.Model.Enums.ValueFactor factorType)
        {
            decimal emissionFactor = 1;

            switch (factorType)
            {
                case DAL.Model.Enums.ValueFactor.Low:
                    emissionFactor = _options.EmissionsFactor.Low;
                    break;
                case DAL.Model.Enums.ValueFactor.Medium:
                    emissionFactor = _options.EmissionsFactor.Medium;
                    break;
                case DAL.Model.Enums.ValueFactor.High:
                    emissionFactor = _options.EmissionsFactor.High;
                    break;
                default: return emissionFactor;
            }

            return emissionFactor;
        }

        // Method to process gas generators and calculate total generation values
        private void GasGenerator(Gas gasInput, decimal valueFactor, decimal emissionFactor, GenerationOutput generationOutput, List<DAL.Model.Output.Day> daysAndEissions)
        {
            foreach (var gasGenerator in gasInput.GasGenerator)
            {
                // Calculate total generation value for each gas generator
                decimal totalGenerationValue = 0;
                foreach (var generation in gasGenerator.Generations)
                    totalGenerationValue += TotalGenerationValue(generation, gasGenerator.EmissionsRating, gasGenerator.Name, valueFactor, emissionFactor, daysAndEissions);

                // Add the total generation value to the GenerationOutput
                generationOutput.Totals.Generator.Add(new DAL.Model.Output.Generator()
                {
                    Name = gasGenerator.Name,
                    Total = totalGenerationValue
                });
            }
        }

        // Method to process coal generators and calculate total generation values
        private void CoalGenerator(Coal coalInput, decimal valueFactor, decimal emissionFactor, GenerationOutput generationOutput, List<DAL.Model.Output.Day> daysAndEissions)
        {
            foreach (var coalGenerator in coalInput.CoalGenerator)
            {
                // Calculate total generation value for each coal generator
                decimal totalGenerationValue = 0;
                foreach (var generation in coalGenerator.Generations)
                    totalGenerationValue += TotalGenerationValue(generation, coalGenerator.EmissionsRating, coalGenerator.Name, valueFactor, emissionFactor, daysAndEissions);

                // Add the total generation value to the GenerationOutput
                generationOutput.Totals.Generator.Add(new DAL.Model.Output.Generator()
                {
                    Name = coalGenerator.Name,
                    Total = totalGenerationValue
                });

                // Calculate and add actual heat rates for each coal generator
                generationOutput.ActualHeatRates.ActualHeatRateList.Add(new ActualHeatRate
                {
                    Name = coalGenerator.Name,
                    HeatRate = coalGenerator.TotalHeatInput / coalGenerator.ActualNetGeneration
                });
            }
        }

        // Method to calculate total generation value for a given generation and add emissions to the list
        private decimal TotalGenerationValue(Generation generation, decimal emissionsRating, string generatorName, decimal valueFactor, decimal emissionFactor, List<DAL.Model.Output.Day> daysAndEissions)
        {
            decimal totalGenerationValue = 0;
            foreach (var day in generation.Days)
            {
                // Calculate total generation value based on energy, price, and value factor
                totalGenerationValue += day.Energy * day.Price * valueFactor;

                // Add emissions data to the list
                daysAndEissions.Add(new DAL.Model.Output.Day
                {
                    Emission = day.Energy * emissionsRating * emissionFactor,
                    Date = day.Date,
                    Name = generatorName
                });
            }

            return totalGenerationValue;
        }

        // Method to process days and emissions data and find days with maximum emissions
        private void DaysAndEmissions(GenerationOutput generationOutput, List<DAL.Model.Output.Day> daysAndEissions)
        {
            // Grouping emissions data by day and finding the day with maximum emissions
            var result = daysAndEissions
                            .GroupBy(item => item.Date.Day)
                            .Select(group => new
                            {
                                MaxItem = group.OrderByDescending(item => item.Emission).First()
                            }).ToList();

            // Adding days with maximum emissions to the GenerationOutput
            foreach (var item in result)
            {
                generationOutput.MaxEmissionGenerators.Days.Add(new DAL.Model.Output.Day
                {
                    Date = item.MaxItem.Date,
                    Name = item.MaxItem.Name,
                    Emission = item.MaxItem.Emission
                });
            }
        }
    }
}
