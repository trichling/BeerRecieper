namespace BeerRecieper.Api.Features.Common;

public interface IInvoke<TCommand>
{
    Task<Unit> InvokeAsync(TCommand request);
}

public interface IHandler<TCommand, TResult> : IInvoke<TCommand>
{
    async Task<Unit> IInvoke<TCommand>.InvokeAsync(TCommand request)
    {
        await HandleAsync(request);
        return Unit.Value;
    }

    Task<TResult> HandleAsync(TCommand request);
}
