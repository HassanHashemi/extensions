using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;

namespace Extensions.Sliding
{
    public class SlidingCollection<T> : IEnumerable<T> where T : class
    {
        private readonly SlidingParams _slidingParams;
        private IQueryable<T> _collection;
        private bool _collectionInitialized = false;
        private int _count = 0;
        private List<T> _result;

        public SlidingCollection(IQueryable<T> collection) : this(collection, null) { }

        public SlidingCollection(IQueryable<T> collection, SlidingParams slidingParams)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _slidingParams = slidingParams ?? SlidingParams.Default;
        }

        public async Task<SlidingCollection<T>> InitializeAsync()
        {
            await SetCountAsync();
            SortAndSkip();

            if (_collection is IMongoQueryable<T> mongoQueryable)
            {
                _result = await IAsyncCursorSourceExtensions.ToListAsync(mongoQueryable);
            }
            else
            {
                _result = await EntityFrameworkQueryableExtensions.ToListAsync(_collection);
            }

            return this;
        }

        public SlidingCollection<T> Initialize()
        {
            SetCount();
            SortAndSkip();
            _result = _collection.ToList();

            return this;
        }

        private SlidingCollection<T> SortAndSkip()
        {
            _collection = Sort()
                    .Skip(_slidingParams.Skip)
                    .Take(_slidingParams.Take);

            _collectionInitialized = true;

            return this;
        }

        public int Count
        {
            get
            {
                ThrowIfNotInit();

                return _count;
            }
            set
            {
                _count = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            ThrowIfNotInit();
            return _result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            ThrowIfNotInit();
            return GetEnumerator();
        }

        private IQueryable<T> Sort()
        {
            var sortExpression = string.Empty;

            foreach (var kvp in _slidingParams.SortProperties)
            {
                sortExpression += sortExpression += $"{kvp.Key} {kvp.Value.ToString().ToLower()},";
            }

            return _collection.OrderBy(sortExpression.TrimEnd(','));
        }

        private SlidingCollection<T> SetCount()
        {
            Count = _collection.Count();
            return this;
        }

        private async Task<SlidingCollection<T>> SetCountAsync()
        {
            if (_collection is IMongoQueryable<T> mongoQueryable)
            {
                Count = await mongoQueryable.CountAsync();
            }
            else
            {
                Count = await EntityFrameworkQueryableExtensions.CountAsync(_collection);
            }

            return this;
        }

        private void ThrowIfNotInit()
        {
            if (!_collectionInitialized)
            {
                throw new InvalidOperationException("Collection has not been initialized");
            }
        }
    }
}
