namespace EventsWebApplication.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IEventRepository Events { get; }
        IPartisipantRepository Participants { get; }
        Task CompleteAsync();
    }
}
