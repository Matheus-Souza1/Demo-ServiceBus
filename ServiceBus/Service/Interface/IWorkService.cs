namespace ServiceBus.Service.Interface
{
    public interface IWorkService
    {
        Task Execute(CancellationToken cancellation);
    }
}
