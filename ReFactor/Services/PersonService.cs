using System;
using System.Collections.Generic;
// using Microsoft.Extensions.Logging;


namespace CodingAssessment.Refactor
{
    public class PersonService
    {
        #region Private fields
        private readonly IPersonRepository _personRepository;

        // tried adding logger but package was not getting added to the project
        // private readonly ILogger<PersonService> _logger;
        #endregion

        #region Constructor 
        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository), "Person repository cannot be null.");
        }

        #endregion

        #region Public Methods
        public void AddPerson(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Person cannot be null.");
            }
            try
            {
                 
                _personRepository.AddPerson(person);
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
               // _logger.LogError(ex, "An error occurred while adding a person.");
                throw;
            }
        }

        public Person GetPersonById(string id)
        {
            try
            {
                return _personRepository.GetPersonById(id);
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                // _logger.LogError(ex, "An error occurred while retrieving persons by id.", id);
                throw;
            }
        }

        public List<Person> GetAllPersons()
        {
            try
            {
                return _personRepository.GetAllPersons();
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                // _logger.LogError(ex, "An error occurred while retrieving all persons");
                throw;
            }
        }

        public List<Person> GetPersonByName(string name)
        {
            try
            {
                return _personRepository.GetPersonByName(name);
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                // _logger.LogError(ex, "An error occurred while retrieving persons by name.", name);
                throw;
            }
        }

        public void UpdatePerson(Person person)
        {

            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Person cannot be null.");
            }
            try
            {

                _personRepository.UpdatePerson(person);
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                // _logger.LogError(ex, "An error occurred while updating the person with ID {Id}.", person.Id);
                throw;
            }
        }

        public void DeletePerson(string id)
        {
            try
            {
                _personRepository.DeletePerson(id);
            }
            catch (Exception ex)
            {
                // Log exception or handle accordingly
                // _logger.LogError(ex, "An error occurred while deleting the person with ID {Id}.", id);
                throw; // Rethrow the same exception to preserve the original stack trace
            }
        }

        /// <summary>
        /// Generated and adds a specified number of people to the repository.
        /// </summary>
        /// <param name="count"></param>
        /// <returns>Added People</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public List<Person> GenerateAddAndRetrieveRandomPersons(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            var persons = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    var person = GenerateRandomPerson();
                    _personRepository.AddPerson(person);
                    persons.Add(person);
                }
                catch (Exception ex)
                {
                    // _logger.LogError(ex, "An error occurred while generating and adding a random person.");
                    throw;
                }
            }
            return persons;
        }
        
        /// <summary>
        /// Retrieves a list of persons by their name and age
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public List<Person> GetPersonsByNameAndAge(string name, int age)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
            if (age < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(age), "Age cannot be negative.");
            }

            try
            {
                var persons = _personRepository.GetPersonByName(name);
                return persons.FindAll(p => p.DateOfBirth.AddYears(age) <= DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An error occurred while retrieving persons by name and age.", name, age);
                throw;
            }
        }


        /// <summary>
        /// Updates the marriage record for a person, not practical use but for demonstration purpose, in real world it could be different Entity to hold Marriage details
        /// and also optional to update the name of the person to include the partner's name.
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="partnerId"></param>
        /// <returns>returns updated person details</returns>
        /// <exception cref="ArgumentException"></exception>
        public Person UpdateMarriageRecord(string personId, string partnerId)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                throw new ArgumentException("Person ID cannot be null or empty.", nameof(personId));
            }
            if (string.IsNullOrWhiteSpace(partnerId))
            {
                throw new ArgumentException("Partner ID cannot be null or empty.", nameof(partnerId));
            }

            try
            {
                var person = _personRepository.GetPersonById(personId);
                var partner = _personRepository.GetPersonById(partnerId);

                if (person == null || partner == null)
                {
                    throw new KeyNotFoundException("Either person or partner not found.");
                }

                 person.Name = $"{person.Name} {partner.Name}";
                _personRepository.UpdatePerson(person);
                return person;
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An error occurred while updating the marriage record for person with ID {Id}.", personId);
                throw;
            }
        }

        #endregion

        #region Private methods
        private Person GenerateRandomPerson()
        {
            // get random name from a predefined list
            var names = new List<string> { "Alice", "Bob", "Charlie", "Diana", "Ethan" };
            var random = new Random();
            string name = names[random.Next(names.Count)];
            // generate a random date of birth
            DateTimeOffset dob = DateTimeOffset.UtcNow.Subtract(new TimeSpan(random.Next(18, 85) * 365, 0, 0, 0));
            return new Person(name, dob);
        }

        #endregion

    }
}