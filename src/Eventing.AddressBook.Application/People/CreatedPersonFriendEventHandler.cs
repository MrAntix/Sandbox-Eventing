using System;
using System.Threading;
using Eventing.AddressBook.Contracts.People;
using Eventing.Common;

namespace Eventing.AddressBook.Application.People
{
    public class CreatedPersonFriendEventHandler :
        IEventHandler<CreatedPersonFriendModel>
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


        public void Handle(Event<CreatedPersonFriendModel> e)
        {
            _writer.Write(e.Data.CreatedPerson);

            _log($"[CREATED PERSON FRIEND] {e.Data.CreatedPerson.Identifier} for {e.Data.FriendIdentifier}");
            _are.Set();
        }
    }
}