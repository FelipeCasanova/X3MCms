using System;
using System.Threading.Tasks;

namespace X3MCMS.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
