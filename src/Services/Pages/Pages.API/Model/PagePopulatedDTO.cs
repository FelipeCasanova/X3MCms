using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Pages.API.Model
{
    [XmlType("PopulatedPage")]
    public class PagePopulatedDTO
    {
        public Page Page { get; set; }

        [XmlIgnore]
        public IEnumerable<Page> Children { get; set; }

        [XmlIgnore]
        public IEnumerable<Zone> Zones { get; set; }

        [JsonIgnore]
        [XmlArray(ElementName = "Children"), XmlArrayItem(ElementName = "Child"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public List<Page> ChildrenSurrogate { get { return Children.ToList(); } set { Children = value; } }

        [JsonIgnore]
        [XmlArray(ElementName = "Zones"), XmlArrayItem(ElementName = "Zone"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public List<Zone> ZonesSurrogate { get { return Zones.ToList(); } set { Zones = value; } }

    }
}
