using System;
using X3MCMS.EventBus.Events;

namespace X3MCMS.EventBus.IntegrationEvents.Events
{
    public class FeatureAddedIntegrationEvent : IntegrationEvent
    {
        public string FeatureId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string ZoneId { get; set; }

        public FeatureAddedIntegrationEvent(string id, string name, string type, string zoneId)
        {
            FeatureId = id;
            Name = name;
            Type = type;
            ZoneId = zoneId;
        }
    }
}
