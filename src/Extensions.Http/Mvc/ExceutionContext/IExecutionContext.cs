namespace Extensions.Http.Mvc
{
    public interface IExecutionContext
    {
        string CurrentUserId { get; }
    }
}
