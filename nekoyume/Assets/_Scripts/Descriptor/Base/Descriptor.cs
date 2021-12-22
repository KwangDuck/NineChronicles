namespace Gateway.Domain.GameContext.Descriptor
{
    public class Descriptor<T>
    {
        public T Id { get; private set; }

        protected Descriptor(T id)
        {
            Id = id;
        }
    }
}
