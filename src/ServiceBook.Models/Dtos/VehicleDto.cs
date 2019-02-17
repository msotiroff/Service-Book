using Newtonsoft.Json;

namespace ServiceBook.Models.Dtos
{
    public class VehicleDto
    {
        [JsonProperty("name")]
        public string Make { get; set; }

        [JsonProperty("models")]
        public string[] Models { get; set; }
    }
}
