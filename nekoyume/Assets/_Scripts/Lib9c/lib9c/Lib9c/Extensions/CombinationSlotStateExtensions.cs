using System;
using Nekoyume.Model.Mail;
using Nekoyume.Model.State;

namespace Nekoyume.Extensions
{
    public static class CombinationSlotStateExtensions
    {
        public static bool TryGetResultId(this CombinationSlotState state, out Guid resultId)
        {            
            return true;
        }

        public static bool TryGetMail(
            this CombinationSlotState state,
            long blockIndex,
            long requiredBlockIndex,
            out CombinationMail combinationMail,
            out ItemEnhanceMail itemEnhanceMail)
        {
            combinationMail = null;
            itemEnhanceMail = null;

            if (!state.TryGetResultId(out var resultId))
            {
                return false;
            }

            return true;
        }
    }
}
