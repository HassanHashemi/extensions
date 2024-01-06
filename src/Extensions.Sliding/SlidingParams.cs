using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Extensions.Sliding
{
    public class SlidingParams : IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (Take < 1)
                result.Add(new ValidationResult("Invalid take", new[] { nameof(Take) }));

            if (Skip < 0)
                result.Add(new ValidationResult("Invalid skip", new[] { nameof(Skip) }));

            return result;
        }
    }
}
