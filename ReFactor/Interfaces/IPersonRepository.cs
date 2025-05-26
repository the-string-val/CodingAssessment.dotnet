namespace CodingAssessment.Refactor
{
    public interface IPersonRepository
    {
        /// <summary>
        /// Retrieves a list of people.
        /// </summary>
        /// <param name="count">The number of people to retrieve.</param>
        /// <returns>A list of people.</returns>
        List<Person> GetPeople(int count);

        /// <summary>
        /// Gets a person by their name.
        /// </summary>
        /// <param name="name">name of the person</param>
        /// <returns></returns>
        Person GetPersonByName(string name, bool olderThanThirty = false);
        
    }
}