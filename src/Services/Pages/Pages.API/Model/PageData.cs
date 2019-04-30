using System;
namespace Pages.API.Model
{
    public class PageData
    {
        public string Id { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
    }
}
