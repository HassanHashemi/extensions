using System.Threading.Tasks;

namespace Extensions.Http.Mvc
{
    public interface IExecutionContext
    {
        string CurrentUserId { get; }
        Task<string> GetTokenAsync();
        string GetByClaimType(string claimType);
    }
}
