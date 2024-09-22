using System.Text.Json.Serialization;

namespace FlightService.Models
{
    public class BoolResponseModel
    {
        public bool Result { get; set; }

        [JsonConstructor]
        public BoolResponseModel(bool result = false)
        {
            Result = result;
        }

        public BoolResponseModel()
        { }

        public static BoolResponseModel Create(bool value)
        {
            return new BoolResponseModel
            {
                Result = value
            };
        }
    }
}
