using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Quotes.Import.Client.Integration.Test
{
    public class QuotesImportClientTest
    {
        public class ApplicationTests
        { 
            [Fact]
            public async Task When_InputDirectoryHasFiles_ShouldStoreQuotesGroupFileIntoStorageLocation()
            {
                // Arrange  
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var storageDirectory = $"{currentDirectory}\\OutputDirectory";
                Directory.Delete(storageDirectory, true); 
                var expectedFileList = new string[] { "Cap Vols", "Eonia", "Eonia" , "Euribor Futures", "Generic Gov Yields", "Swaption Vols" , "Euribor 6M" };
                var quotesImportService = QuotesImportServiceHelper.GetQuotesImportServiceInstance();

                // Act
                await quotesImportService.ProcessAsync(new Models.QuotesImportRequest($"{currentDirectory}\\InputDirectory",storageDirectory ));

                // Assert
                var files = Directory.EnumerateFiles(storageDirectory, "*.json");

                foreach (var file in files)
                {
                    var actualFileName = file
                        .Replace(storageDirectory, "")
                        .Replace("\\", "")
                        .Replace(".json", "");
                    Assert.Contains(actualFileName, expectedFileList);
                } 
            }

            [Fact]
            public async Task When_InValidInputDirectoryNamePassed_ShouldNotStoreAnyFileIntoStorageLocation()
            {
                // Arrange  
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var storageDirectory = $"{currentDirectory}\\OutputDirectory";
                Directory.Delete(storageDirectory, true);
                Directory.CreateDirectory(storageDirectory);
                var quotesImportService = QuotesImportServiceHelper.GetQuotesImportServiceInstance();
                var inputDirectory = $"{currentDirectory}\\InputDirectory1";

                // Act
                await quotesImportService.ProcessAsync(new Models.QuotesImportRequest(inputDirectory, storageDirectory));

                // Assert
                var files = Directory.EnumerateFiles(storageDirectory, "*.json"); 
                Assert.Empty(files);
            }

            [Fact]
            public async Task When_Error_OnCallingQuotesImportService_ShouldNotStoreAnyFileIntoStorageLocation()
            {
                // Arrange  
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var storageDirectory = $"{currentDirectory}\\OutputDirectory";
                Directory.Delete(storageDirectory, true);
                Directory.CreateDirectory(storageDirectory);
                var quotesImportService = QuotesImportServiceHelper.GetQuotesImportServiceInstance(true);
                var inputDirectory = $"{currentDirectory}\\InputDirectory";

                // Act
                await quotesImportService.ProcessAsync(new Models.QuotesImportRequest(inputDirectory, storageDirectory));

                // Assert
                var files = Directory.EnumerateFiles(storageDirectory, "*.json");
                Assert.Empty(files);
            }
        }
    }
}