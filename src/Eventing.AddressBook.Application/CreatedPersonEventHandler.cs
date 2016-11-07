using System;
using Eventing.AddressBook.Contracts;
using Eventing.Common;

namespace Eventing.AddressBook.Application
{
    public class CreatedPersonEventHandler :
        IEventHandler<CreatedPersonEvent>
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

        public void Handle(CreatedPersonEvent e)
        {
            _writer.Write(e.CreatedPerson);

            _log($"[CREATED PERSON] {e.CreatedPerson.Identifier}");
        }
    }
}