using System;
using Eventing.AddressBook.Contracts;
using Eventing.Common;

namespace Eventing.AddressBook.Application
{
    public class UpdatedPersonNameEventHandler :
        IEventHandler<UpdatedPersonNameEvent>
    {
        readonly Action<string> _log;
        readonly PersonReader _reader;
        readonly UpdatedPersonWriter _writer;

        public UpdatedPersonNameEventHandler(
            Action<string> log,
            PersonReader reader, UpdatedPersonWriter writer)
        {
            _log = log;
            _reader = reader;
            _writer = writer;
        }

        public void Handle(UpdatedPersonNameEvent e)
        {
            var person = _reader.Read(e.UpdatedPerson.Identifier);

            _log($"[UPDATED PERSON] {person.Identifier} {person.Name} => {e.UpdatedPerson.NewName}");

            _writer.Write(e.UpdatedPerson);
        }
    }
}