using System;
using System.Threading;
using Eventing.AddressBook.Contracts;
using Eventing.Common;

namespace Eventing.AddressBook.Application
{
    public class CreatedPersonFriendEventHandler :
        IEventHandler<CreatedPersonFriendEvent>
    {
        readonly Action<string> _log;
        readonly CreatedPersonWriter _writer;
        readonly AutoResetEvent _are;

        public CreatedPersonFriendEventHandler(
            Action<string> log,
            AutoResetEvent are,
            CreatedPersonWriter writer)
        {
            _log = log;
            _writer = writer;
            _are = are;
        }


        public void Handle(CreatedPersonFriendEvent e)
        {
            _writer.Write(e.CreatedPerson);

            _log($"[CREATED PERSON FRIEND] {e.CreatedPerson.Identifier} for {e.FriendIdentifier}");
            _are.Set();
        }
    }
}