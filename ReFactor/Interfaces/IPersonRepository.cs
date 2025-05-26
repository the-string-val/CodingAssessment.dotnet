using System;
using System.Collections.Generic;

namespace CodingAssessment.Refactor
{
    public interface IPersonRepository
    {


        /// <summary>
        /// Adds a person to the repository
        /// </summary>
        /// <param name="person"></param>
        void AddPerson(Person person);

        /// <summary>
        /// Gets a person by their Id
        /// </summary>
        /// <param name="id">Person Id</param>
        /// <returns>Return Person with given Id</returns>
        Person GetPersonById(string id);

        /// <summary>
        /// Gets all persons in the repository.
        /// </summary>
        /// <returns></returns>
        List<Person> GetAllPersons();
        
        /// <summary>
        /// Gets a person by their name.
        /// </summary>
        /// <param name="name">name of a person</param>
        /// <returns>Returns a List of Person with given name</returns>

        List<Person> GetPersonByName(string name)


        /// <summary>
        /// Updates a person in the repository.
        /// </summary>
        /// <param name="person"> Person details to update</param>
        void UpdatePerson(Person person);

        /// <summary>
        /// Deletes a person from the repository.
        /// </summary>
        /// <param name="id">Person Id</param>
        void DeletePerson(string id);

    }
}