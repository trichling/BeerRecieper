using BeerRecipes;
using Common;
using MaltPlans;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IEndpointInvoker>(sp =>
{
    var endpointDataSource = sp.GetRequiredService<EndpointDataSource>();
    return new InProcessEndpointInvoker(sp, endpointDataSource);
});

builder.Services.AddBeerRecipeServices();
builder.Services.RegisterExternalServicesForInProcess();
builder.Services.AddMaltPlanServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure endpoints
app.MapBeerRecipeEndpoints();
app.MapMaltPlanEndpoints();

await app.RunAsync();
