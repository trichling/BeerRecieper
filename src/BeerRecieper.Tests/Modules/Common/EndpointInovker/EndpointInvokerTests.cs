using System.Runtime.CompilerServices;
using Common;
using MaltPlans.Contracts;
using MaltPlans.Contracts.Api;
using MaltPlans.Contracts.Requests;
using RestEase;

namespace BeerRecieper.Tests.Modules.Common.EndpointInovker;

[TestClass]
public class EndpointInvokerTests { 

    
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
        var methodAndPath = GetHttpMethodFromAttributes();

        var result = await endpointInvoker.InvokeEndpointAsync<IEnumerable<MaltPlanResponse>>(
            methodAndPath.Item1,
            methodAndPath.Item2
        );

        return result;
    }

    public async Task<MaltPlanResponse> GetMaltPlanByIdAsync(Guid id, bool includeMalt = false)
    {
        var methodAndPath = GetHttpMethodFromAttributes();

        var result = await endpointInvoker.InvokeEndpointAsync<MaltPlanResponse>(
            methodAndPath.Item1,
            methodAndPath.Item2,
            new Dictionary<string, object> { { "id", id.ToString() } }
        );

        return result;
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

    private (string, string) GetHttpMethodFromAttributes([CallerMemberName] string callerName = "")
    {
        // Get all interfaces implemented by this class
        var interfaces = this.GetType().GetInterfaces();

        foreach (var interfaceType in interfaces)
        {
            var methodInfo = interfaceType.GetMethod(callerName);
            if (methodInfo == null)
                continue;

            var attributes = methodInfo.GetCustomAttributes(true) ?? [];

            // Find RestEase attribute
            foreach (var attribute in attributes)
            {
                if (attribute is not RequestAttributeBase requestAttr)
                    continue;
                return (requestAttr.Method.ToString(), requestAttr.Path ?? string.Empty);
            }
        }

        throw new NotImplementedException("No RestEase attribute found for method " + callerName);
    }
}
