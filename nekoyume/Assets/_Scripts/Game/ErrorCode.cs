using System;
using Nekoyume.L10n;

namespace Nekoyume.Game
{
    public static class ErrorCode
    {
        public static (string, string, string) GetErrorCode(Exception exc)
        {
            var key = "ERROR_UNKNOWN";
            var code = "99";
            var errorMsg = string.Empty;

            errorMsg = errorMsg == string.Empty
                ? string.Format(
                    L10nManager.Localize("UI_ERROR_RETRY_FORMAT"),
                    L10nManager.Localize(key),
                    code)
                : errorMsg;
            return (key, code, errorMsg);
        }
    }
}
