using System;

namespace Portal.Business.Extensions
{
    public class SlidingCollectionWrapper<T> where T : class
    {
        public SlidingCollectionWrapper(SlidingCollection<T> slidingCollection)
        {
            this.Items = slidingCollection ?? throw new ArgumentNullException(nameof(slidingCollection));
            this.Total = slidingCollection.Count;
        }

        public SlidingCollection<T> Items { get; }
        public int Total { get; }
    }
}
