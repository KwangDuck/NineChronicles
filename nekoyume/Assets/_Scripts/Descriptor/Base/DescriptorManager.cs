using System;
using System.Collections.Generic;

namespace Gateway.Domain.GameContext.Descriptor
{
    public interface IDescriptorManager
    {
        void PreLink();
        void Link();
        void OnComplete();
        void OnFailed();
    }

    public abstract class DescriptorManager<ID, D> : IDescriptorManager
        where D : Descriptor<ID>
    {
        protected Dictionary<ID, D> Map { get; set; } = new Dictionary<ID, D>();
        protected Dictionary<ID, D> VerifiedMap { get; private set; } = null;

        public bool Has(ID id)
        {
            return Map.ContainsKey(id);
        }

        public D Get(ID id)
        {
            if (!Has(id))
            {
                throw new Exception($"{id} not found");
            }
            return Map[id];
        }

        public D Put(ID id, D value)
        {
            if (Has(id))
            {
                throw new Exception($"{id} already exists");
            }
            return Map[id] = value;
        }

        public Dictionary<ID, D>.KeyCollection Keys()
        {
            return Map.Keys;
        }

        public Dictionary<ID, D>.ValueCollection Values()
        {
            return Map.Values;
        }

        public virtual void PreLink() { }

        public virtual void Link() { }

        public virtual void PutComplete() { }

        public void Reset()
        {
            Map = new Dictionary<ID, D>();
        }

        public void OnComplete()
        {
            VerifiedMap = Map;
        }

        public void OnFailed()
        {
            if (VerifiedMap != null)
            {
                Map = VerifiedMap;
            }
        }
    }
}
