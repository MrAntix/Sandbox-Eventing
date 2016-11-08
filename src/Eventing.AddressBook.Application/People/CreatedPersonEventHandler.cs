using System;
using Eventing.AddressBook.Contracts.People;
using Eventing.Common;

namespace Eventing.AddressBook.Application.People
{
    public class CreatedPersonEventHandler :
        IEventHandler<CreatedPersonModel>
    {
        readonly Action<string> _log;
        readonly CreatedPersonWriter _writer;

        public CreatedPersonEventHandler(
            Action<string> log,
            CreatedPersonWriter writer)
        {
            _log = log;
            _writer = writer;
        }

        public void Handle(Event<CreatedPersonModel> e)
        {
            _writer.Write(e.Data);

            _log($"[CREATED PERSON] {e.Data.Identifier}");
        }
    }
}