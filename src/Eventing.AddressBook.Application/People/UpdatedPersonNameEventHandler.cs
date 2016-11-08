using System;
using Eventing.AddressBook.Contracts.People;
using Eventing.Common;

namespace Eventing.AddressBook.Application.People
{
    public class UpdatedPersonNameEventHandler :
        IEventHandler<UpdatedPersonNameModel>
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

        public void Handle(Event<UpdatedPersonNameModel> e)
        {
            var person = _reader.Read(e.Data.Identifier);

            _log($"[UPDATED PERSON] {person.Identifier} {person.Name} => {e.Data.NewName}");

            _writer.Write(e.Data);
        }
    }
}