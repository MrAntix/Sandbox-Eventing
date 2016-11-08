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
            var eventStore = GetEventStore();
            var events = GetEvents(eventStore);
            RegisterEvents(events, are, log, people);

            var bob = ExecuteCreatePerson(events, "Bob");

            Assert.True(are.WaitOne(100));

            ExecuteUpdatePersonName(events, bob.FriendIdentifier, "Bob's Friend");

            _output.WriteLine("\r\nReplay");

            var newPeople = new List<Person>();
            var newEventStore = GetEventStore();
            var newEvents = GetEvents(newEventStore);
            RegisterEvents(newEvents, are, log, newPeople);

            for (var i = 1; i < eventStore.NextSequenceNumber; i++)
                newEvents.Replay(eventStore.Get(i));

            Assert.Equal(people[0].Name, newPeople[0].Name);

            foreach (var person in newPeople)
                _output.WriteLine($"{person.Identifier} {person.Name}");
        }

        static void RegisterEvents(IEvents events,
            AutoResetEvent are, Action<string> log,
            ICollection<Person> people)
        {
            var personReader = new PersonReader(people);
            var createdPersonWriter = new CreatedPersonWriter(people);
            var updatedPersonWriter = new UpdatedPersonWriter(people);

            events.Register(new CreatedPersonEventHandler(log, createdPersonWriter));
            events.Register(new CreatedPersonFriendEventHandler(log, are, createdPersonWriter));
            events.Register(new UpdatedPersonNameEventHandler(log, personReader, updatedPersonWriter));
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

        static IEvents GetEvents(IEventStore store)
        {
            return new Events(store);
        }

        static IEventStore GetEventStore()
        {
            return new TestEventStore();
        }

        class TestEventStore : IEventStore
        {
            readonly IList<IEvent> _list;

            public TestEventStore()
            {
                _list = new List<IEvent>();
            }

            public long NextSequenceNumber => _list.Count + 1;

            public long Add(IEvent e)
            {
                _list.Add(e);

                return e.SequenceNumber;
            }

            public IEvent Get(long sequenceNumber)
            {
                return _list[(int) sequenceNumber - 1];
            }
        }
    }
}