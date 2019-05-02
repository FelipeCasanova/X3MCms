using System;
using System.Threading.Tasks;
using X3MCMS.EventBus.Abstractions;
using X3MCMS.EventBus.IntegrationEvents.Events;

namespace X3MCMS.EventBus.IntegrationEvents.Handlers
{
    public class FeatureAddedIntegrationEventHandler : IIntegrationEventHandler<FeatureAddedIntegrationEvent>
    {
        public FeatureAddedIntegrationEventHandler()
        {
        }

        public Task Handle(FeatureAddedIntegrationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
