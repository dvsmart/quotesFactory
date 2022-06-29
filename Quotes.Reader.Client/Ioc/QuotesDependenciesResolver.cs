using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quotes.Reader.Client.Models;
using Quotes.Reader.Client.Services;
using Quotes.Reader.Client.Validators;

namespace Quotes.Import.Client.Ioc
{
    public class QuotesDependenciesResolver
    {
        public static IContainer RegisterDependencies()
        {
            var serviceCollection = new ServiceCollection();
            var configurationBuilder = new ConfigurationBuilder()
           .AddJsonFile("appSettings.json");
            var Configuration = configurationBuilder.Build();
            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection); 
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<IConfiguration>(Configuration);
           
            builder.RegisterType<QuotesGroupReaderService>().As<IQuotesGroupReaderService>();
            builder.RegisterType<QuotesGroupReaderRequestValidator>().As<IValidator<QuotesGroupReaderRequest>>().InstancePerLifetimeScope();
            return builder.Build();
        }
    }
}
