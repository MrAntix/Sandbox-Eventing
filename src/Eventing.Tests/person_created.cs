using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Eventing.AddressBook.Application;
using Eventing.AddressBook.Contracts;
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
            var log = (Action<string>)_output.WriteLine;

            var people = new List<Person>();
            var eventStore = new TestEventStore();
            var events = GetEvents(eventStore);
            RegisterEvents(events, are, log, people);
            
            var bob = ExecuteCreatePerson(events, "Bob");

            Assert.True(are.WaitOne(100));

            ExecuteUpdatePersonName(events, bob.FriendIdentifier, "Bob's Friend");

            _output.WriteLine("\r\nReplay");

            var newEventStore = new TestEventStore();
            var newEvents = GetEvents(newEventStore);
            var newPeople = new List<Person>();
            RegisterEvents(newEvents, are, log, newPeople);

            foreach (var e in eventStore.All())
                newEvents.Replay(e);

            Assert.Equal(people[0].Name, newPeople[0].Name);

            foreach(var person in newPeople) 
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

        class TestEventStore : IEventStore
        {
            readonly IList<object> _list;

            public TestEventStore(
                IList<object> list = null)
            {
                _list = list ?? new List<object>();
            }

            public IEnumerable<object> All()
            {
                return _list.ToArray();
            }

            public void Add(object e)
            {
                _list.Add(e);
            }
        }
    }
}