using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Nekoyume.Model.State
{
    [Serializable]
    public class CreditsState : State
    {
        public IImmutableList<string> Names { get; }

        public CreditsState(IEnumerable<string> names)
        {
            Names = names.ToImmutableList();
        }        
    }
}
