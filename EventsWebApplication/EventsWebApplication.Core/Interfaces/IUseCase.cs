namespace EventsWebApplication.Core.Interfaces
{
    public interface IUseCase<in TRequest, TResult>
    {
        Task<TResult> ExecuteAsync(TRequest request);
    }

}
