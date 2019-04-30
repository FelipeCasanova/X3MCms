using System;
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
        public ZoneEnum Type { get; set; }
        public string PageId { get; set; }
    }
}
