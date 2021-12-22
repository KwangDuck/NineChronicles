using Gateway.Protocol.Table;

namespace Gateway.Domain.GameContext.Descriptor
{
    public class DummyDescriptor : Descriptor<int>
    {
        public class Loader : DescriptorLoader<DummyDescriptor, int, Manager>
        {            
            public Loader(Manager manager) : base(manager)
            {
            }

            public override void LoadInternal()
            {
                
            }
        }

        public class Manager : DescriptorManager<int, DummyDescriptor>
        {
            public Manager()
            {
            }
        }

        protected DummyDescriptor(int id) : base(id)
        {

        }
    }
}
