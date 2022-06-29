using Autofac;
using Quotes.Reader.Client.Ioc;
using Quotes.Reader.Client.Models;
using Quotes.Reader.Client.Services;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace Quotes.Import.Client.Integration.Test
{
    public class QuotesReaderClientTest
    {
        public class ApplicationTests
        {
            [Fact]
            public async Task When_StorageDirectoryHasFiles_ShouldStoreQuotesGroupResultIntoOutputDirectoryLocation()
            {
                // Arrange  
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var outputDirectory = $"{currentDirectory}\\OutputDirectory";
                var storageDirectory = $"{currentDirectory}\\StorageDirectory";
                Directory.Delete(outputDirectory, true);
                var container = QuotesDependenciesResolver.RegisterDependencies().Build();
                var quotesGroupReaderService = container.Resolve<IQuotesGroupReaderService>();
                var expectedQuotesDateString = "31-12-2019";
                var expectedQuoteGroup = "Cap Vols";
                var expectedFilePath = $"{outputDirectory}//{expectedQuoteGroup}-2019-12-31.json";

                // Act
                await quotesGroupReaderService.ProcessAsync(
                    new QuotesGroupReaderRequest(storageDirectory, outputDirectory, expectedQuotesDateString, expectedQuoteGroup));

                // Assert
                Assert.True(File.Exists(expectedFilePath));
            }

            [Fact]
            public async Task When_GroupNameDoesntExist_ShouldNotStoreAnyFile()
            {
                // Arrange  
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var outputDirectory = $"{currentDirectory}\\OutputDirectory";
                var storageDirectory = $"{currentDirectory}\\StorageDirectory";
                Directory.Delete(outputDirectory, true);
                var container = QuotesDependenciesResolver.RegisterDependencies().Build();
                var quotesGroupReaderService = container.Resolve<IQuotesGroupReaderService>();
                var expectedQuotesDateString = "31-12-2019";
                var expectedQuoteGroup = "invalidgroupname";
                var expectedFilePath = $"{outputDirectory}//{expectedQuoteGroup}-2019-12-31.json";

                // Act
                await quotesGroupReaderService.ProcessAsync(
                    new QuotesGroupReaderRequest(storageDirectory, outputDirectory, expectedQuotesDateString, expectedQuoteGroup));

                // Assert
                Assert.False(File.Exists(expectedFilePath));
            }

            [Fact]
            public async Task When_GroupNameExistButNoQuotesExists_ShouldStoreAFileWithEmptyQuotes()
            {
                // Arrange  
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var outputDirectory = $"{currentDirectory}\\OutputDirectory";
                var storageDirectory = $"{currentDirectory}\\StorageDirectory";
                Directory.Delete(outputDirectory, true);
                var container = QuotesDependenciesResolver.RegisterDependencies().Build();
                var quotesGroupReaderService = container.Resolve<IQuotesGroupReaderService>();
                var expectedQuotesInvalidDateString = "31-12-2078";
                var expectedQuoteGroup = "Cap Vols";
                var expectedFilePath = $"{outputDirectory}//{expectedQuoteGroup}-2078-12-31.json";

                // Act
                await quotesGroupReaderService.ProcessAsync(
                    new QuotesGroupReaderRequest(storageDirectory, outputDirectory, expectedQuotesInvalidDateString, expectedQuoteGroup));

                // Assert
                Assert.True(File.Exists(expectedFilePath));
            }

        }
    }
}