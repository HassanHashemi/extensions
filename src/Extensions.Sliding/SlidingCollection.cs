using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Project.Business.Extensions
{
    public class SlidingCollection<T> : IEnumerable<T> where T : class
    {
        private readonly SlidingParams _slidingParams;
        private IQueryable<T> _collection;
        private bool _collectionInitialized = false;
        private int _count = 0;

        public SlidingCollection(IQueryable<T> collection) : this(collection, null) { }

        public SlidingCollection(IQueryable<T> collection, SlidingParams slidingParams)
        {
            this._collection = collection ?? throw new ArgumentNullException(nameof(collection));
            this._slidingParams = slidingParams ?? SlidingParams.Default;
        }

        public async Task<SlidingCollection<T>> InitializeAsync()
            => (await this.SetCountAsync()).SortAndSkip();

        public SlidingCollection<T> Initialize()
            => this.SetCount().SortAndSkip();

        private SlidingCollection<T> SortAndSkip()
        {
            this._collection = this.Sort()
                    .Skip(this._slidingParams.Skip)
                    .Take(this._slidingParams.Take);

            this._collectionInitialized = true;

            return this;
        }

        public int Count
        {
            get
            {
                this.ThrowIfNotInit();

                return _count;
            }
            set
            {
                this._count = value;
            }
        }

        public IQueryable<T> Collection
        {
            get
            {
                ThrowIfNotInit();

                return this._collection;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            this.ThrowIfNotInit();
            return this._collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            this.ThrowIfNotInit();
            return this.GetEnumerator();
        }

        private IQueryable<T> Sort()
        {
            var sortExpression = string.Empty;

            foreach(var kvp in this._slidingParams.SortProperties)
            {
                sortExpression += sortExpression += $"{kvp.Key} {kvp.Value.ToString().ToLower()},";
            }

            return this._collection.OrderBy(sortExpression.TrimEnd(','));
        }

        private SlidingCollection<T> SetCount()
        {
            this.Count = this._collection.Count();
            return this;
        }

        private async Task<SlidingCollection<T>> SetCountAsync()
        {
            this.Count = await this._collection.CountAsync();
            return this;
        }

        private void ThrowIfNotInit()
        {
            if (!this._collectionInitialized)
            {
                throw new InvalidOperationException("Collection has not been initialized");
            }
        }
    }
}
