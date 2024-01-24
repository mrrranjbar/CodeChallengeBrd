using BradyCodeChallenge.DAL.Model.Inputs;
using BradyCodeChallenge.DAL.Model.Static;
using BradyCodeChallenge.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BradyCodeChallenge.Test
{
    public class UnitTestGetInput : IClassFixture<BaseTest>
    {
        private readonly IHost _host;
        // Constructor that sets up the test environment using the BaseTest fixture
        public UnitTestGetInput(BaseTest baseTest)
        {
            _host = baseTest.TestHost;
        }

        // Test method to check if the input folder path is valid
        [Fact]
        public void GetInputAddress_ShouldReturnValidAddress()
        {
            // Arrange
            // Retrieving configuration and input folder path from the host services
            var configuration = _host.Services.GetService<IConfiguration>();
            var inputFolderPath = configuration.GetRequiredSection("InputFolderPath");
            var inputFolderPathValue = inputFolderPath.Value;

            // Act
            // Verifying that configuration and input folder path are not null or empty
            Assert.NotNull(configuration);
            Assert.NotNull(inputFolderPath);
            Assert.False(string.IsNullOrEmpty(inputFolderPathValue));

            // Assert
            // Verifying that the folder exists
            Assert.True(Directory.Exists(inputFolderPathValue), $"Folder does not exist at: {inputFolderPathValue}");
        }

        // Test method to check if the output folder path is valid
        [Fact]
        public void GetOutputAddress_ShouldReturnValidAddress()
        {
            // Arrange
            // Retrieving configuration and output folder path from the host services
            var configuration = _host.Services.GetService<IConfiguration>();
            var outputFolderPath = configuration.GetRequiredSection("OutputFolderPath");
            var outputFolderPathValue = outputFolderPath.Value;

            // Act
            // Verifying that configuration and output folder path are not null or empty
            Assert.NotNull(configuration);
            Assert.NotNull(outputFolderPath);
            Assert.False(string.IsNullOrEmpty(outputFolderPathValue));

            // Assert
            // Verifying that the folder exists
            Assert.True(Directory.Exists(outputFolderPathValue), $"Folder does not exist at: {outputFolderPathValue}");
        }

        // Test method to check if data can be read from an XML file
        [Fact]
        public void GetDataFromXMLFile_ShouldReturnNonEmptyData()
        {
            // Arrange
            // Retrieving configuration, input folder path, and GenerationReportRepository from the host services
            var configuration = _host.Services.GetService<IConfiguration>();
            var inputFolderPath = configuration.GetRequiredSection("InputFolderPath");
            var inputFolderPathValue = inputFolderPath.Value;
            var generationReportRepository = _host.Services.GetService<IGenerationReportRepository>();

            // Creating sample test and destination file path
            var assemblyLocation = typeof(BaseTest).Assembly.Location;
            var sampleTestFileFullPath = Path.Combine(Path.GetDirectoryName(assemblyLocation), "Sample Test File", "01-Basic.xml");
            var destinationFilePath = Path.Combine(inputFolderPathValue, Path.GetFileName(sampleTestFileFullPath));

            try
            {
                // Use a ManualResetEvent to synchronize the test
                using (var resetEvent = new ManualResetEventSlim())
                {

                    // Setting up a FileSystemWatcher to monitor file creation in the input folder
                    FileSystemWatcher watcher = new FileSystemWatcher(inputFolderPathValue);
                    watcher.Created += (object sender, FileSystemEventArgs e) =>
                    {
                        // Act
                        // Reading GenerationReport from the created XML file
                        var generationReport = generationReportRepository.Get(e.FullPath);

                        // Assert
                        // Verifying that GenerationReportRepository and GenerationReport are not null
                        Assert.NotNull(generationReportRepository);
                        Assert.NotNull(generationReport);

                        // Clean up
                        if (File.Exists(e.FullPath))
                        {
                            File.Delete(e.FullPath);
                        }

                        // Signal the event to continue the test
                        resetEvent.Set();
                    };
                    watcher.EnableRaisingEvents = true;

                    // Act
                    // Creating a sample XML file in the input folder
                    if (File.Exists(destinationFilePath))
                    {
                        File.Delete(destinationFilePath);
                    }
                    File.Copy(sampleTestFileFullPath, destinationFilePath);

                    // Wait for the event or timeout after a specific duration
                    if (!resetEvent.Wait(TimeSpan.FromSeconds(20)))
                    {
                        Assert.True(false, "Timeout waiting for file creation event");
                    }
                }
            }
            finally
            {
                // Clean up
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
                Assert.True(!File.Exists(destinationFilePath));
            }
        }

        // Test method to check if DataGenerator initializes without throwing an exception
        [Fact]
        public void DataGenerator_Initialize_ShouldNotThrowException()
        {
            // Arrange
            // Retrieving DataGenerator from the host services
            var dataGenerator = _host.Services.GetRequiredService<IDataGenerator>();

            // Act & Assert
            // Verifying that DataGenerator initializes without throwing an exception
            try
            {
                dataGenerator.Initialize();
            }
            catch (System.Exception ex)
            {
                Assert.True(false, $"Unexpected exception: {ex}");
            }
        }

        // Test method to check if Factors configuration is not null
        [Fact]
        public void FactorsConfiguration_ShouldNotBeNull()
        {
            // Arrange
            // Retrieving Factors configuration from the host services
            var factorsConfiguration = _host.Services.GetRequiredService<IOptions<Factors>>();

            // Act & Assert
            // Verifying that Factors configuration is not null
            Assert.NotNull(factorsConfiguration?.Value);
        }
    }
}