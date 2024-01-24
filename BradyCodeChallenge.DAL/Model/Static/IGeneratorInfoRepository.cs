// Importing necessary namespaces
using BradyCodeChallenge.DAL.Model.Enums;

namespace BradyCodeChallenge.DAL.Model.Static
{
    // Definition of the IGeneratorInfoRepository interface
    public interface IGeneratorInfoRepository
    {
        // Method signature for retrieving a GeneratorInfo entity based on generatorType
        public GeneratorInfo Get(GeneratorType generatorType);

        // Method signature for adding a range of GeneratorInfo entities to the repository
        public void CreateRange(List<GeneratorInfo> generatorInfos);
    }
}
