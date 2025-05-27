using System;
using System.ComponentModel.DataAnnotations;


namespace CodingAssessment.Refactor
{

    // Added Person class to encapsulate person-related properties
    // Renamed the class from People to Person for clarity.
    public class Person
    {

        // Added Id property to uniquely identify each person
        public string Id { get; private set; }

        [MaxLength(255)]
        public string Name { get; set; }
        public DateTimeOffset DateOfBirth { get; private set; }

        /// <summary>
        /// Constructor for creating a person with a name and date of birth.
        /// </summary>
        /// <param name="name">The name of the person</param>
        /// <param name="dateOfBirth">The date of birth of the person</param>
        /// <exception cref="ArgumentException"></exception>
        public Person(string name, DateTimeOffset dateOfBirth)
        {
            // Added check for null or empty name 
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            }
            // Added check for date of birth in the future
            if (dateOfBirth > DateTimeOffset.UtcNow)
            {
                throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));
            }
            Id = Guid.NewGuid().ToString();
            Name = name;
            DateOfBirth = dateOfBirth;
        }

        /// <summary>
        /// Constructor for creating a a person with an ID, name, and date of birth.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="dateOfBirth"></param>

         public Person(string id, string name, DateTimeOffset dateOfBirth)
        {
            Id = id;
            Name = name;
            DateOfBirth = dateOfBirth;
        }

        /// <summary>
        /// Constructor for creating a person with a name, defaulting the date of birth to 15 years ago.
        /// kept for backward compatibility, but should be avoided in new code. 
        /// </summary>
        /// <param name="name">name of the person</param>

        public Person(string name) : this(name, DateTimeOffset.UtcNow.AddYears(-15))
        {
        }

    }

}