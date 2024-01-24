// Importing necessary namespaces
using BradyCodeChallenge.DAL.Model.Enums;

namespace BradyCodeChallenge.DAL.Model.Static
{
    // Definition of the GeneratorInfoRepository class implementing the IGeneratorInfoRepository interface
    public class GeneratorInfoRepository : IGeneratorInfoRepository
    {
        // Private readonly field to store the GeneratorInfoDbContext dependency
        private readonly GeneratorInfoDbContext _dbContext;

        // Constructor for the GeneratorInfoRepository class, injecting a GeneratorInfoDbContext instance
        public GeneratorInfoRepository(GeneratorInfoDbContext generatorInfoDbContext) 
        {
            _dbContext = generatorInfoDbContext;
        }

        // Method for retrieving a GeneratorInfo entity based on generatorType
        public GeneratorInfo Get(GeneratorType generatorType)
        {
            var generatorInfo = _dbContext.GeneratorInfos.FirstOrDefault(generator => generator.GeneratorType == generatorType);

            return generatorInfo;
        }

        // Method for adding a range of GeneratorInfo entities to the repository
        public void CreateRange(List<GeneratorInfo> generatorInfos)
        {
            // Adding a range of GeneratorInfo entities to the DbSet in the database context
            _dbContext.GeneratorInfos.AddRange(generatorInfos);

            // Save changes to the database
            _dbContext.SaveChanges();
        }
    }
}
