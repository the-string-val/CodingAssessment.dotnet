
using CodingAssessment.Refactor;
using Xunit;

namespace Tests
{
    public class PersonTest
    {

        [Fact]
        public void Person_ConstructorWithNameOnly_SetsDOBCorrectlyTo15YearsAgo()
        {
            // Arrange
            string name = "TestPerson";
            var expectedDob = DateTimeOffset.UtcNow.AddYears(-15);

            // Act
            var person = new Person(name);

            // Assert
            Assert.Equal(name, person.Name);
            Assert.Equal(person.DateOfBirth.Date, expectedDob.Date);
        }

        [Fact]
        public void Person_ConstructorWithNameAndDOB_SetsNameAndDOBCorrectly()
        {
            // Arrange
            string name = "TestPerson";
            var dateOfBirth = DateTimeOffset.UtcNow.AddYears(-30).AddDays(new Random().Next(-1000, 1000));

            // Act
            var person = new Person(name, dateOfBirth);

            // Assert
            Assert.Equal(name, person.Name);
            Assert.Equal(person.DateOfBirth.Date, dateOfBirth.Date);
        }

        
        [Fact]
        public void Person_Constructor_ThrowsException_WhenNameIsNullOrEmpty()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => new Person(null));
            Assert.Throws<ArgumentException>(() => new Person(""));
            Assert.Throws<ArgumentException>(() => new Person("   "));
        }

        [Fact]
        public void Person_Constructor_ThrowsException_WhenDateOfBirthIsInFuture()
        {
            // Arrange
            string name = "FuturePerson";
            var futureDob = DateTimeOffset.UtcNow.AddDays(1);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Person(name, futureDob));
        }

        [Fact]
        public void Person_Id_IsGeneratedAndIsNotNullOrEmpty()
        {
            // Arrange
            var person = new Person("TestPerson");

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(person.Id));
            Guid guidOutput;
            Assert.True(Guid.TryParse(person.Id, out guidOutput));
        }

        [Fact]
        public void Person_Name_CanBeChanged()
        {
            // Arrange
            var person = new Person("OriginalName");

            // Act
            person.Name = "NewName";

            // Assert
            Assert.Equal("NewName", person.Name);
        } }
}