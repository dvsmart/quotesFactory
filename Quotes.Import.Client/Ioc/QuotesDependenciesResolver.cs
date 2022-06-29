using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quotes.Import.Client.Models;
using Quotes.Import.Client.Services;
using Quotes.Import.Client.Validators;
using System.Net.Http;

namespace Quotes.Import.Client.Ioc
{
    public class  QuotesDependenciesResolver
    {
        public static IContainer RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appSettings.json");
            var Configuration = configurationBuilder.Build();

            // Adding Configuration.
            serviceCollection.AddSingleton<IConfiguration>(Configuration);
            
            // Adding logging support
            serviceCollection.AddLogging();
            
            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);

            // Registering customer dependencies
            builder.Register(
               context =>
                   context.IsRegistered<IHttpClientFactory>()
                       ? context.Resolve<IHttpClientFactory>().CreateClient()
                       : new HttpClient()).As<HttpClient>();
            builder.RegisterType<QuotesImportClient>().As<IQuotesImportClient>().InstancePerLifetimeScope(); 
            builder.RegisterType<QuotesImportService>().As<IQuotesImportService>().InstancePerLifetimeScope();
            builder.RegisterType<QuotesImportRequestValidator>().As<IValidator<QuotesImportRequest>>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
