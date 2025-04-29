using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Commands;
using MaltPlans.Contracts.Requests;
using MaltPlans.Data;
using MaltPlans.Endpoints;
using MaltPlans.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace MaltPlans;

public static class MaltPlansModule
{
    public static IServiceCollection AddMaltPlanServices(this IServiceCollection services)
    {
        services.AddSingleton<IMaltPlanRepository, InMemoryMaltPlanRepository>();
        services.AddScoped<IHandler<Unit, IEnumerable<MaltPlanResponse>>, GetAllMaltPlansHandler>();
        services.AddScoped<
            IHandler<GetMaltPlanByIdCommand, MaltPlanResponse?>,
            GetMaltPlanByIdHandler
        >();
        services.AddScoped<
            IHandler<CreateMaltPlanCommand, MaltPlanResponse>,
            CreateMaltPlanHandler
        >();
        services.AddScoped<
            IHandler<UpdateMaltPlanTotalWeightCommand, MaltPlanResponse?>,
            UpdateMaltPlanTotalWeightHandler
        >();
        services.AddScoped<IHandler<AddMaltCommand, MaltPlanResponse?>, AddMaltHandler>();
        services.AddScoped<IHandler<RemoveMaltCommand, MaltPlanResponse?>, RemoveMaltHandler>();
        services.AddScoped<
            IHandler<UpdateMaltAmountCommand, MaltPlanResponse?>,
            UpdateMaltAmountHandler
        >();

        services.AddScoped<
            IInvoke<UpdateMaltPlanTotalWeightCommand>,
            UpdateMaltPlanTotalWeightHandler
        >();
        return services;
    }

    public static void MapMaltPlanEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/maltplans").WithTags("MaltPlans");

        group
            .MapGet(
                "/",
                ([FromServices] IHandler<Unit, IEnumerable<MaltPlanResponse>> handler) =>
                    GetAllMaltPlansEndpoint.Handle(handler)
            )
            .WithName("GetAllMaltPlans")
            .WithMetadata(new EndpointHandlerMetadata(typeof(GetAllMaltPlansEndpoint)));

        group
            .MapGet(
                "/{id}",
                (
                    [FromRoute] Guid id,
                    [FromQuery] bool includeMalt,
                    [FromServices] IHandler<GetMaltPlanByIdCommand, MaltPlanResponse?> handler
                ) => GetMaltPlanByIdEndpoint.Handle(id, includeMalt, handler)
            )
            .WithName("GetMaltPlanById");

        group
            .MapPost(
                "/",
                (
                    [FromBody] CreateMaltPlanRequest request,
                    [FromServices] IHandler<CreateMaltPlanCommand, MaltPlanResponse> handler
                ) => CreateMaltPlanEndpoint.Handle(request, handler)
            )
            .WithName("CreateMaltPlan");

        group
            .MapPut(
                "/{id}/weight",
                (
                    [FromRoute] Guid id,
                    [FromBody] UpdateMaltPlanWeightRequest request,
                    [FromServices]
                        IHandler<UpdateMaltPlanTotalWeightCommand, MaltPlanResponse?> handler
                ) => UpdateMaltPlanTotalWeightEndpoint.Handle(id, request, handler)
            )
            .WithName("UpdateMaltPlanWeight");

        group
            .MapPost(
                "/{id}/malts",
                (
                    [FromRoute] Guid id,
                    [FromBody] AddMaltRequest request,
                    [FromServices] IHandler<AddMaltCommand, MaltPlanResponse?> handler
                ) => AddMaltEndpoint.Handle(id, request, handler)
            )
            .WithName("AddMalt");

        group
            .MapDelete(
                "/{id}/malts/{maltName}",
                (
                    [FromRoute] Guid id,
                    [FromRoute] string maltName,
                    [FromServices] IHandler<RemoveMaltCommand, MaltPlanResponse?> handler
                ) => RemoveMaltEndpoint.Handle(id, maltName, handler)
            )
            .WithName("RemoveMalt");

        group
            .MapPut(
                "/{id}/malts/{maltName}",
                (
                    [FromRoute] Guid id,
                    [FromRoute] string maltName,
                    [FromBody] UpdateMaltAmountRequest request,
                    [FromServices] IHandler<UpdateMaltAmountCommand, MaltPlanResponse?> handler
                ) => UpdateMaltAmountEndpoint.Handle(id, maltName, request, handler)
            )
            .WithName("UpdateMaltAmount");
    }
}
