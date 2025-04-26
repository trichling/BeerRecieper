using BeerRecieper.Api.Features.BeerRecipes;
using BeerRecieper.Api.Features.MaltPlans;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Add this line to configure Swagger generation
builder.Services.AddOpenApi();
builder.Services.AddBeerRecipeServices();
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
