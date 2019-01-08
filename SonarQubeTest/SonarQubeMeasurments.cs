using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SonarQubeTest
{
    public partial class SonarQubeMeasurments
    {
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        [JsonProperty("measures")]
        public List<Measure> Measures { get; set; }
    }

    public partial class Measure
    {
        [JsonProperty("metric")]
        public string Metric { get; set; }

        [JsonProperty("history")]
        public List<History> History { get; set; }
    }

    public partial class History
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class Paging
    {
        [JsonProperty("pageIndex")]
        public long PageIndex { get; set; }

        [JsonProperty("pageSize")]
        public long PageSize { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

}