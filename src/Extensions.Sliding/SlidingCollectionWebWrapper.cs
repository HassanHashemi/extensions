using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions.Sliding
{
    public class SlidingCollectionWrapper<T> where T : class
    {
        public SlidingCollectionWrapper(SlidingCollection<T> slidingCollection)
        {
            this.Items = slidingCollection ?? throw new ArgumentNullException(nameof(slidingCollection));
            this.Total = slidingCollection.Count;
        }

        public SlidingCollectionWrapper(IEnumerable<T> items, int total)
        {
            Items = items;
            Total = total;
        }

        public IEnumerable<T> Items { get; }
        public int Total { get; }

        public static SlidingCollectionWrapper<T> Empty()
        {
            return new SlidingCollectionWrapper<T>(Enumerable.Empty<T>(), 0);
        }
    }
}
