// Importing necessary namespaces
using BradyCodeChallenge.DAL.Model.Enums;
using BradyCodeChallenge.DAL.Model.Static;

namespace BradyCodeChallenge.Services.General
{
    // Definition of the DataGenerator class implementing the IDataGenerator interface
    public class DataGenerator : IDataGenerator
    {
        // Constructor for the DataGenerator class, injecting an IGeneratorInfoRepository instance
        private readonly IGeneratorInfoRepository _generatorInfoRepository;
        public DataGenerator(IGeneratorInfoRepository generatorInfoRepository)
        {
            _generatorInfoRepository = generatorInfoRepository;
        }

        // Method to initialize and create a range of GeneratorInfo instances in the repository
        public void Initialize()
        {
            _generatorInfoRepository.CreateRange(new List<GeneratorInfo>(){
                // Offshore wind generator with low value factor and N/A emission factor
                new GeneratorInfo { GeneratorType = GeneratorType.Offshore, ValueFactor = DAL.Model.Enums.ValueFactor.Low, EmissionFactor = EmissionFactor.N_A },
                // Onshore wind generator with high value factor and N/A emission 
                new GeneratorInfo { GeneratorType = GeneratorType.Onshore, ValueFactor = DAL.Model.Enums.ValueFactor.High, EmissionFactor = EmissionFactor.N_A },
                // Gas generator with medium value factor and medium emission factor
                new GeneratorInfo { GeneratorType = GeneratorType.Gas, ValueFactor = DAL.Model.Enums.ValueFactor.Medium, EmissionFactor = EmissionFactor.Medium },
                // Coal generator with medium value factor and high emission factor
                new GeneratorInfo { GeneratorType = GeneratorType.Coal, ValueFactor = DAL.Model.Enums.ValueFactor.Medium, EmissionFactor = EmissionFactor.High }
            });
        }
    }
}
