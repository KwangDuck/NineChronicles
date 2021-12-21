using Gateway.Protocol.Table;

namespace Gateway.Domain.GameContext.Descriptor
{
    public interface Loadable
    {

    }

    public interface IDescriptorLoader : Loadable
    {
        dynamic Manager { get; }
        string TableName { get; }
        ST_Table GetTable();
    }

    public abstract class DescriptorLoader<D, ID, M> : IDescriptorLoader
        where D : Descriptor<ID> 
        where M : DescriptorManager<ID, D>
    {
        public dynamic Manager { get; private set; }
        public virtual string TableName { get; }
        private ST_Table _table;

        protected void SetTable(ST_Table table)
        {
            _table = table;
        }

        public ST_Table GetTable()
        {
            return _table;
        }

        public DescriptorLoader(M manager)
        {
            Manager = manager;
        }

        public void Compile()
        {
            Manager.Reset();
            LoadInternal();
            Manager.PutComplete();
        }

        abstract public void LoadInternal();
    }
}
