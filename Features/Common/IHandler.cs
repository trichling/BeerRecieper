namespace Lab.BeerRecieper.Features.Common;

public interface IHandler<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request);
}