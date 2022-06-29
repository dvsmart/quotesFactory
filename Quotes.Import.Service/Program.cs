

using MassTransit;
using Quotes.Import.Service.Attributes;
using Quotes.Import.Service.OperationFilters;
using Quotes.Import.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add basic authentication
builder.Services.AddControllers(opt =>
            {
    opt.Filters.Add<ApiKeyAttribute>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add basic authentication to swagger.
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<ApiKeyHeaderParameterFilter>(); 
});
var config = new ServiceAuthenticationConfig
{
    ApiKey = builder.Configuration.GetValue<string>("Api.Key")
};
builder.Services.AddSingleton<ServiceAuthenticationConfig>(config);
builder.Services.AddScoped<IQuoteGroupsService, QuoteGroupsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

 