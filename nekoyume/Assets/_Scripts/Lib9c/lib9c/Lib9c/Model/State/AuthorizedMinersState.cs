namespace Nekoyume.Model.State
{
    public class AuthorizedMinersState : State
    {
        public long Interval { get; private set; }
        public long ValidUntil { get; private set; }
    }
}
