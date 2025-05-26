using System;
using CodingAssessment.Refactor;
using FluentAssertions;
using Xunit;

namespace Tests
{
    public class PeopleTest
    {

        [Fact]
        public void People_ConstructorWithNameOnly_SetsDOBCorrectlyTo15YearsAgo()
        {
            // Arrange
            string name = "TestPerson";
            var expectedDob = DateTimeOffset.UtcNow.AddYears(-15);

            // Act
            var person = new People(name);

            // Assert
            Assert.Equal(name, person.Name);
            Assert.Equal(person.DOB.Date, expectedDob.Date);
        }

        [Fact]
        public void People_ConstructorWithNameAndDOB_SetsNameAndDOBCorrectly()
        {
            // Arrange
            string name = "TestPerson";
            var dateOfBirth = DateTimeOffset.UtcNow.AddYears(-30).AddDays(new Random().Next(-1000, 1000));

            // Act
            var person = new People(name, dateOfBirth);

            // Assert
            Assert.Equal(name, person.Name);
            Assert.Equal(person.DOB.Date, dateOfBirth.Date);
        }

    }

    public class BirthingUnitTest
    {
        [Fact]
        public void GetPeople_ReturnsCorrectNumberOfPeople()
        {
            // Arrange
            var birthingUnit = new BirthingUnit();
            int numberOfPeopleToRetrieve = 5;

            // Act
            var people = birthingUnit.GetPeople(numberOfPeopleToRetrieve);

            // Assert
            Assert.Equal(numberOfPeopleToRetrieve, people.Count);
        }

        [Fact]
        public void GetPeople_ReturnsListWithUniqueNames()
        {
            // Arrange
            var birthingUnit = new BirthingUnit();
            int numberOfPeopleToRetrieve = 10;

            // Act
            var people = birthingUnit.GetPeople(numberOfPeopleToRetrieve);

            // Assert
            var names = new HashSet<string>();
            foreach (var person in people)
            {
                Assert.True(names.Add(person.Name), "Duplicate name found: " + person.Name);
            }
        }
    }
}
