using BradyCodeChallenge.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BradyCodeChallenge.Test
{
    public class UnitTestProcess : IClassFixture<BaseTest>
    {
        private readonly IHost _host;
        // Constructor that sets up the test environment using the BaseTest fixture
        public UnitTestProcess(BaseTest baseTest)
        {
            _host = baseTest.TestHost;
        }

        // Test method to check if input processing and output creation succeed
        [Fact]
        public void GetInputAndCreateOutput_ShouldSucceed()
        {
            // Arrange
            // Retrieving required services and configuration from the host services
            var dataGenerator = _host.Services.GetService<IDataGenerator>();
            var watcher = _host.Services.GetService<IWatcher>();
            var configuration = _host.Services.GetService<IConfiguration>();
            var inputFolderPath = configuration.GetRequiredSection("InputFolderPath");
            var outputFolderPath = configuration.GetRequiredSection("OutputFolderPath");
            var assemblyLocation = typeof(BaseTest).Assembly.Location;
            var fileName = "01-Basic";
            var sampleTestFileFullPath = Path.Combine(Path.GetDirectoryName(assemblyLocation), "Sample Test File", fileName + ".xml");
            var destinationFilePath = Path.Combine(inputFolderPath.Value, Path.GetFileName(sampleTestFileFullPath));
            var outputFilePath = Path.Combine(outputFolderPath.Value, fileName + "-Result.xml");

            try
            {
                // Act
                // Initializing the data generator and starting the watcher
                dataGenerator.Initialize();
                watcher.StartWatching();

                // Creating a sample XML file in the input folder
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
                File.Copy(sampleTestFileFullPath, destinationFilePath);

                // Use Task.Run to run the assertions in a new thread
                var task = Task.Run(() =>
                {
                    var count = 0;
                    while (count < 10)
                    {
                        // Allow some time for the watcher to process the file
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        if (File.Exists(outputFilePath)) break;
                        count++;
                    }

                    // Assert
                    // Verifying that the output report file is created
                    Assert.True(File.Exists(outputFilePath), "Output report file is created!");
                });
            }
            finally
            {
                // Clean up
                // Deleting the created output report file after the test
                if (File.Exists(outputFilePath))
                {
                    File.Delete(outputFilePath);
                }
                Assert.True(!File.Exists(outputFilePath), "Output file is NOT removed!");
            }
        }
    }
}
