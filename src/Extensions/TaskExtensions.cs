using System.Diagnostics;
using System.Threading.Tasks;

namespace Extensions
{
    public static class TaskExtensions
    {
        public static async void Forget(this Task source)
        {
            Debug.Assert(source != null);

            try
            {
                await source;
            }
            catch
            {
            }
        }
    }
}
