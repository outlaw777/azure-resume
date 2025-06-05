using Newtonsoft.Json;

namespace Company.Function
{
    public class getResumeCounter
    {
        [JsonProperty(PropertyName ="id")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        [JsonProperty(PropertyName ="count")]
        public int Count { get; set; }
    }
}