using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nekoyume.Model.State;

namespace Nekoyume.Model.Mail
{
    public enum MailType
    {
        Workshop = 1,
        Auction,
        System
    }
    [Serializable]
    public abstract class Mail : IState
    {        
        public Guid id;
        public bool New;
        public long blockIndex;
        public virtual MailType MailType => MailType.System;
        public long requiredBlockIndex;

        public abstract void Read(IMail mail);

        protected abstract string TypeId { get; }
    }

    [Serializable]
    public class MailBox : IEnumerable<Mail>, IState
    {
        private List<Mail> _mails = new List<Mail>();

        public int Count => _mails.Count;

        public Mail this[int idx] => _mails[idx];

        public MailBox()
        {
        }

        public IEnumerator<Mail> GetEnumerator()
        {
            return _mails.OrderByDescending(i => i.blockIndex).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Mail mail)
        {
            _mails.Add(mail);
        }

        public void CleanUp()
        {
            if (_mails.Count > 30)
            {
                _mails = _mails
                    .OrderByDescending(m => m.blockIndex)
                    .ThenBy(m => m.id)
                    .Take(30)
                    .ToList();
            }
        }

        [Obsolete("Use CleanUp")]
        public void CleanUp2()
        {
            if (_mails.Count > 30)
            {
                _mails = _mails.OrderByDescending(m => m.blockIndex).Take(30).ToList();
            }
        }

        [Obsolete("No longer in use.")]
        public void CleanUpTemp(long blockIndex)
        {
            _mails = _mails
                .Where(m => m.requiredBlockIndex >= blockIndex)
                .ToList();
        }

        public void Remove(Mail mail)
        {
            _mails.Remove(mail);
        }
    }
}
