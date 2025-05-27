using System;
using System.Collections.Generic;


namespace CodingAssessment.Refactor.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private List<Person> _people;

        public PersonRepository()
        {
            _people = new List<Person>();
        }

        public void AddPerson(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Person cannot be null.");
            }
            _people.Add(person);
        }

        public Person GetPersonById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id cannot be null or empty.", nameof(id));
            }
            return _people.Find(p => p.Id == id) ?? throw new KeyNotFoundException($"Person with Id {id} not found.");
        }

        public List<Person> GetAllPersons()
        {
            return new List<Person>(_people);
        }

        public List<Person> GetPersonByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
            return _people.FindAll(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? new List<Person>([]);
        }

        public void UpdatePerson(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Person cannot be null.");
            }
            var index = _people.FindIndex(p => p.Id == person.Id);
            if (index == -1)
            {
                throw new KeyNotFoundException($"Person with Id {person.Id} not found.");
            }
            _people[index] = person;
        }

        public void DeletePerson(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id can not be null or empty.", nameof(id));
            }
            var index = _people.FindIndex(p => p.Id == id);
            if (index == -1)
            {
                throw new KeyNotFoundException($"Person with Id {id} does not exist");
            }
            _people.Remove(_people[index]);
        } 
    }
}