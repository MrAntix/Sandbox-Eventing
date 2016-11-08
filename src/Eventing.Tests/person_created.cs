using System;
using System.Collections.Generic;
using System.Threading;
using Eventing.AddressBook.Application.People;
using Eventing.AddressBook.Contracts.People;
using Eventing.AddressBook.Domain.People;
using Eventing.Common;
using Xunit;
using Xunit.Abstractions;

namespace Eventing.Tests
{
    public class person_created
    {
        readonly ITestOutputHelper _output;

        public person_created(
            ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void create_update_replay()
        {
            var are = new AutoResetEvent(false);
            var log = (Action<string>) _output.WriteLine;

            var people = new List<Person>();
            var eventStore = TestHelper.GetEventStore();
            var handlers = new EventDispatcherRepository(() => eventStore.NextSequenceNumber);
            RegisterHandlers(handlers, are, log, people);

            var events = TestHelper.GetEvents(eventStore, handlers);

            var bob = ExecuteCreatePerson(events, "Bob");

            Assert.True(are.WaitOne(100));

            ExecuteUpdatePersonName(events, bob.FriendIdentifier, "Bob's Friend");

            Assert.Equal("Bob", people[0].Name);
            Assert.Equal("Bob's Friend", people[1].Name);

            foreach (var person in people)
                _output.WriteLine($"{person.Identifier} {person.Name}");
        }

        static void RegisterHandlers(IEventDispatcherRepository dispatchers,
            AutoResetEvent are, Action<string> log,
            ICollection<Person> people)
        {
            var personReader = new PersonReader(people);
            var createdPersonWriter = new CreatedPersonWriter(people);
            var updatedPersonWriter = new UpdatedPersonWriter(people);

            dispatchers.Register(new CreatedPersonEventHandler(log, createdPersonWriter));
            dispatchers.Register(new CreatedPersonFriendEventHandler(log, are, createdPersonWriter));
            dispatchers.Register(new UpdatedPersonNameEventHandler(log, personReader, updatedPersonWriter));
        }

        static CreatedPersonModel ExecuteCreatePerson(
            IEvents events,
            string name)
        {
            var handler = new CreatePersonCommandHandler(events, new CreatePersonFriendCommandHandler(events));
            var command = new CreatePersonCommand(name);

            return handler.Execute(command);
        }

        static void ExecuteUpdatePersonName(
            IEvents events,
            Guid personIdentifier, string newName)
        {
            var handler = new UpdatePersonNameCommandHandler(events);
            var command = new UpdatePersonNameCommand(personIdentifier, newName);

            handler.Execute(command);
        }
    }
}