using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pages.API.Model
{
    public enum ZoneEnum : byte
    {
        HEADER = 0,
        BANNER_LEFT = 1,
        CENTRE = 2,
        BANNER_RIGHT = 3,
        FOOTER = 4
    }

    public class Zone
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ZoneEnum Type { get; set; }
        public string PageId { get; set; }
    }
}
