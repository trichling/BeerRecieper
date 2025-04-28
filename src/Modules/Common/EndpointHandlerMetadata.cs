using System.Reflection;

namespace Common;

public class EndpointHandlerMetadata
{
    public MethodInfo MethodInfo { get; }
    public Type HandlerType { get; }

    public EndpointHandlerMetadata(Type handlerType)
    {
        HandlerType = handlerType;
    }

    public EndpointHandlerMetadata(MethodInfo methodInfo, Type handlerType)
    {
        MethodInfo = methodInfo;
        HandlerType = handlerType;
    }
    
}