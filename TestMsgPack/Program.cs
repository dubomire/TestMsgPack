using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using MessagePack;
using MessagePack.Resolvers;
using Newtonsoft.Json;
using TestMsgPack.Models;

namespace TestMsgPack
{
    class Program
    {
        #region private methods
        private static void TryMsgPack(int taskID, List<Person> persons)
        {
            using (var stream = new FileStream(
                $"{taskID}.txt",
                FileMode.Create))
            {
                Console.WriteLine($"Task[{taskID}]: Creating file.");

                MessagePackSerializer.Serialize(
                    stream,
                    persons,
                    CompositeResolver.Instance);

                stream.Flush();
            }

            List<Person> deserializedPersons = null;
            using (var stream = new FileStream($"{taskID}.txt", FileMode.Open))
            {
                Console.WriteLine($"Task[{taskID}]: Starting deserialization");

                deserializedPersons =
                    MessagePackSerializer.Deserialize<List<Person>>(
                        stream,
                        CompositeResolver.Instance);

                stream.Flush();
            }

            Console.WriteLine(JsonConvert.SerializeObject(deserializedPersons));
            Console.ReadKey();

            foreach (var person in deserializedPersons)
            {
                var originPerson =
                    persons.FirstOrDefault(p => p.ID == person.ID);

                OutputPerson(person, originPerson);
            }
        }

        private static void OutputPerson(Person person, Person originPerson)
        {
            Console.WriteLine("******************");
            Console.WriteLine(
                $"Person '{person.Name}': origin date '{originPerson.Date}' and deserialized date '{person.Date}' is {(DateTime.Equals(originPerson.Date, person.Date) ? "" : "not")} equal; ");
            Console.WriteLine($"Person '{person.Name}':");
            Console.WriteLine(
                $"    Origin date: \n\t{GetDateDescription(originPerson.Date)}");
            Console.WriteLine(
                $"    Deserialized date: \n\t{GetDateDescription(person.Date)}");
            Console.WriteLine("******************");

            Console.ReadKey();
        }

        private static string GetDateDescription(DateTime date)
        {
            return $"Date '{date}' [Ticks: {date.Ticks}, Kind: {date.Kind}]";
        }

        private static List<Person> GetPersonList()
        {
            return new List<Person>
            {
                new Person
                {
                    ID = 0,
                    Name = "Georg",
                    Age = 22,
                    Date = DateTime.Today.AddYears(-5).AddMonths(-6).AddHours(11).AddMinutes(51).AddSeconds(1)
                },
                new Person
                {
                    ID = 1,
                    Name = "Anna",
                    Age = 21,
                    Date = DateTime.Today.AddYears(-11).AddMonths(-2).AddHours(12).AddMinutes(17).AddSeconds(39)
                },
                new Person
                {
                    ID = 2,
                    Name = "Ludvig",
                    Age = 25,
                    Date = DateTime.Today.AddYears(1).AddMonths(4).AddHours(15).AddMinutes(5).AddSeconds(15)
                },
                new Person
                {
                    ID = 3,
                    Name = "Jesper",
                    Age = 28,
                    Date = DateTime.Today.AddYears(2).AddMonths(-3).AddHours(2).AddMinutes(33).AddSeconds(41)
                },
            };
        }
        #endregion

        #region public methods
        static void Main(string[] args)
        {
            //Random rnd = new Random();
            //int taskID = rnd.Next(3541, 91311);
            var persons = GetPersonList();

            CompositeResolver.RegisterAndSetAsDefault(
                ContractlessStandardResolver.Instance,
                NativeDateTimeResolver.Instance,
                StandardResolver.Instance
            );

            TryMsgPack(111, persons);
        }
        #endregion
    }
}
