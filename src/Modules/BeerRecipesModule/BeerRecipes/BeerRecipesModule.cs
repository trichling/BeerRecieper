using System.Reflection;
using BeerRecipes.Contracts;
using BeerRecipes.Contracts.Commands;
using BeerRecipes.Contracts.Requests;
using BeerRecipes.Data;
using BeerRecipes.Endpoints;
using BeerRecipes.Handlers;
using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Api;
using MaltPlans.Contracts.Commands;
using MaltPlans.Contracts.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

namespace BeerRecipes;

public static class BeerRecipesModule
{
    public static IServiceCollection AddBeerRecipeServices(this IServiceCollection services)
    {
        services.AddSingleton<IBeerRecipeRepository, InMemoryBeerRecipeRepository>();
        services.AddScoped<
            IHandler<Unit, IEnumerable<BeerRecipeResponse>>,
            GetAllBeerRecipesHandler
        >();
        services.AddScoped<
            IHandler<GetBeerRecipeByIdCommand, BeerRecipeResponse?>,
            GetBeerRecipeByIdHandler
        >();
        services.AddScoped<
            IHandler<CreateBeerRecipeCommand, BeerRecipeResponse>,
            CreateBeerRecipeHandler
        >();
        services.AddScoped<
            IHandler<UpdateBeerRecipeCommand, BeerRecipeResponse?>,
            UpdateBeerRecipeHandler
        >();
        services.AddScoped<IHandler<DeleteBeerRecipeCommand, Unit>, DeleteBeerRecipeHandler>();
        services.AddScoped<IHandler<SetMaltPlanCommand, BeerRecipeResponse?>, SetMaltPlanHandler>();
        services.AddScoped<
            IHandler<RemoveMaltPlanCommand, BeerRecipeResponse?>,
            RemoveMaltPlanHandler
        >();
        services.AddScoped<
            IHandler<UpdateMaltTotalWeightInKgCommand, Unit>,
            UpdateMaltTotalWeightInKgHandler
        >();
        return services;
    }

    public static void RegisterExternalServicesForHttp(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddHttpClient(
            "MaltPlans",
            client =>
            {
                client.BaseAddress = new Uri("http://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
            }
        );

        services.AddScoped<IMaltPlanHttpApi>(services =>
            RestClient.For<IMaltPlanHttpApi>(
                services.GetRequiredService<IHttpClientFactory>().CreateClient("MaltPlans")
            )
        );
    }

    public static void RegisterExternalServicesForInProcess(this IServiceCollection services)
    {
        services.AddTransient<IMaltPlanHttpApi, InProcessMaltPlanHttpApi>();
    }

    public static void MapBeerRecipeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/beerrecipes").WithTags("Beer Recipes");

        group
            .MapGet(
                "/",
                (
                    [FromServices] IHandler<Unit, IEnumerable<BeerRecipeResponse>> handler
                ) => GetAllRecipesEndpoint.Handle(handler)
            )
            .WithName("GetAllRecipes")
            .WithMetadata(new EndpointHandlerMetadata(typeof(GetAllRecipesEndpoint)));

        group
            .MapGet(
                "/{id}",
                (
                    [FromRoute] Guid id,
                    [FromServices] IHandler<GetBeerRecipeByIdCommand, BeerRecipeResponse?> handler
                ) => GetRecipeByIdEndpoint.Handle(id, handler)
            )
            .WithName("GetRecipeById")
            .WithMetadata(new EndpointHandlerMetadata(typeof(GetRecipeByIdEndpoint)));

        group
            .MapPost(
                "/",
                (
                    [FromBody] CreateBeerRecipeRequest request,
                    [FromServices] IHandler<CreateBeerRecipeCommand, BeerRecipeResponse> handler
                ) => CreateRecipeEndpoint.Handle(request, handler)
            )
            .WithName("CreateRecipe")
            .WithMetadata(new EndpointHandlerMetadata(typeof(CreateRecipeEndpoint)));

        group
            .MapPut(
                "/{id}",
                (
                    [FromRoute] Guid id,
                    [FromBody] UpdateBeerRecipeRequest request,
                    [FromServices] IHandler<UpdateBeerRecipeCommand, BeerRecipeResponse?> handler
                ) => UpdateRecipeEndpoint.Handle(id, request, handler)
            )
            .WithName("UpdateRecipe")
            .WithMetadata(new EndpointHandlerMetadata(typeof(UpdateRecipeEndpoint)));

        group
            .MapDelete(
                "/{id}",
                (
                    [FromRoute] Guid id,
                    [FromServices] IHandler<DeleteBeerRecipeCommand, Unit> handler
                ) => DeleteRecipeEndpoint.Handle(id, handler)
            )
            .WithName("DeleteRecipe")
            .WithMetadata(new EndpointHandlerMetadata(typeof(DeleteRecipeEndpoint)));

        group
            .MapPut(
                "/{id}/maltPlan",
                (
                    [FromRoute] Guid id,
                    [FromBody] AddMaltPlanRequest request,
                    [FromServices] IHandler<SetMaltPlanCommand, BeerRecipeResponse?> handler
                ) => SetMaltPlanEndpoint.Handle(id, request, handler)
            )
            .WithName("SetMaltPlan")
            .WithMetadata(new EndpointHandlerMetadata(typeof(SetMaltPlanEndpoint)));

        group
            .MapDelete(
                "/{id}/maltplan",
                (
                    [FromRoute] Guid id,
                    [FromServices] IHandler<RemoveMaltPlanCommand, BeerRecipeResponse?> handler
                ) => RemoveMaltPlanEndpoint.Handle(id, handler)
            )
            .WithName("RemoveMaltPlan")
            .WithMetadata(new EndpointHandlerMetadata(typeof(RemoveMaltPlanEndpoint)));

        group
            .MapPut(
                "/{id}/malttotalweight/{weight}",
                async (
                    [FromRoute] Guid id,
                    [FromRoute] double weight,
                    [FromServices] IHandler<UpdateMaltTotalWeightInKgCommand, Unit> handler
                ) => await UpdateMaltTotalWeightInKgEndpoint.Handle(id, weight, handler)
            )
            .WithName("UpdateMaltPlanTotalWeight")
            .WithMetadata(new EndpointHandlerMetadata(typeof(UpdateMaltTotalWeightInKgEndpoint)));
    }

}

public class InProcessMaltPlanHttpApi : IMaltPlanHttpApi
{
    private readonly IEndpointInvoker endpointInvoker;

    public InProcessMaltPlanHttpApi(IEndpointInvoker endpointInvoker)
    {
        this.endpointInvoker = endpointInvoker;
    }


    public async Task<IEnumerable<MaltPlanResponse>> GetAllMaltPlansAsync()
    {
        // Get all custom attributes
        var attributes =
            typeof(IMaltPlanHttpApi)
                .GetMethod("GetAllMaltPlansAsync")
                ?.GetCustomAttributes(true)
            ?? [];

        var methodAndPath = GetHttpMethodFromAttributes(attributes);

        var result = await endpointInvoker.InvokeEndpointAsync<IEnumerable<MaltPlanResponse>>(
             methodAndPath.Item1,
             methodAndPath.Item2
         );

        return result;
    }

    private static async Task<T> InvokeAsyncMethod<T>(
        MethodInfo method,
        object instance,
        params object[] parameters
    )
    {
        var task = method.Invoke(instance, parameters) as Task<T>;

        if (task == null)
        {
            throw new InvalidOperationException(
                $"Method did not return expected Task<{typeof(T).Name}>"
            );
        }

        return await task;
    }

    private (string, string) GetHttpMethodFromAttributes(IEnumerable<object> attributes)
    {
        // loop thorugh the attributes and find the one that is a RestEase attribute
        foreach (var attribute in attributes)
        {
            if (attribute is not RequestAttributeBase)
            {
                continue;
            }
            var requesAtAttribute = (RequestAttributeBase)attribute;
            var method = requesAtAttribute.Method.ToString();
            var path = requesAtAttribute.Path ?? string.Empty;

            return (method, path);
        }

        throw new NotImplementedException("No RestEase attribute found");
    }

    public Task<MaltPlanResponse> GetMaltPlanByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<MaltPlanResponse> CreateMaltPlanAsync(CreateMaltPlanRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<MaltPlanResponse> UpdateMaltPlanWeightAsync(
        Guid id,
        UpdateMaltPlanWeightRequest request
    )
    {
        throw new NotImplementedException();
    }

    public Task DeleteMaltPlanAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}

