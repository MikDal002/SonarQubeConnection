using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SonarQubeTest
{
    public partial class SonarQubeMeasurments
    {
        [JsonProperty("component")]
        public Component Component { get; set; }
    }

    public partial class Component
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("qualifier")]
        public string Qualifier { get; set; }

        [JsonProperty("measures")]
        public List<Measure> Measures { get; set; }
    }

    public partial class Measure
    {
        [JsonProperty("metric")]
        public string Metric { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("periods")]
        public List<Period> Periods { get; set; }

        [JsonProperty("bestValue", NullValueHandling = NullValueHandling.Ignore)]
        public bool? BestValue { get; set; }
    }

    public partial class Period
    {
        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("bestValue", NullValueHandling = NullValueHandling.Ignore)]
        public bool? BestValue { get; set; }
    }
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}