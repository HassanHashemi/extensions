using System.Collections.Generic;

namespace Extensions.Sliding
{
    public class SlidingParams
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public Dictionary<string, OrderType> SortProperties { get; set; }

        public static SlidingParams Default
        {
            get
            {
                return new SlidingParams()
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    SortProperties = new Dictionary<string, OrderType>()
                    {
                        { "ID", OrderType.Descending }
                    }
                };
            }
        }
    }
}
