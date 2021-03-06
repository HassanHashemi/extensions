using System.Linq;
using System.Threading.Tasks;

namespace Extensions.Sliding
{
    public static class SlidingCollectionExtensions
    {
        public async static Task<SlidingCollectionWrapper<T>> ToSlidingCollectionWrapperAsync<T>(this IQueryable<T> source, SlidingParams slidingParams)
            where T : class
        {
            var result = await source.ToSlidingCollectionAsync(slidingParams);

            return new SlidingCollectionWrapper<T>(result);
        }

        public static SlidingCollectionWrapper<T> ToSlidingCollectionWrapper<T>(this IQueryable<T> source, SlidingParams slidingParams)
            where T : class
        {
            return new SlidingCollectionWrapper<T>(source.ToSlidingCollection(slidingParams));
        }

        public static Task<SlidingCollection<T>> ToSlidingCollectionAsync<T>(this IQueryable<T> source, SlidingParams slidingParams)
            where T : class
        {
            return new SlidingCollection<T>(source, slidingParams).InitializeAsync();
        }

        public static SlidingCollection<T> ToSlidingCollection<T>(this IQueryable<T> source, SlidingParams slidingParams)
            where T : class
        {
            return new SlidingCollection<T>(source, slidingParams).Initialize();
        }
    }
}
