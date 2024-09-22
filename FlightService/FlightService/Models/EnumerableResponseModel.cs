using System.Collections;

namespace FlightService.Models
{
    public class EnumerableResponseModel<TValue>
    {
        public IEnumerable<TValue> Result { get; set; } = [];

        public static EnumerableResponseModel<TValue> Create(IEnumerable<TValue> enumerable)
        {
            return new EnumerableResponseModel<TValue>
            {
                Result = enumerable
            };
        }
    }
}
