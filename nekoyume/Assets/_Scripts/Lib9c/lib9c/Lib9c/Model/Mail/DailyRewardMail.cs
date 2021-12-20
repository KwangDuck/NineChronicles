using System;

namespace Nekoyume.Model.Mail
{
    [Serializable]
    public class DailyRewardMail : AttachmentMail
    {
        protected override string TypeId => "dailyRewardMail";
        public override MailType MailType => MailType.System;

        public override void Read(IMail mail)
        {
            mail.Read(this);
        }
    }
}
