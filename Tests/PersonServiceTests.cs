
using CodingAssessment.Refactor;
using Moq;
using Xunit;

namespace Tests
{
    public class PersonServiceTests
    {
       private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly PersonService _personService;

        public PersonServiceTests()
        {
            // Arrange (common setup for all tests)
            _mockPersonRepository = new Mock<IPersonRepository>();
            // Initialize PersonService with the mock repository.
            _personService = new PersonService(_mockPersonRepository.Object);
        }


        [Fact]
        public void AddPerson_ValidPerson_ShouldCallRepositoryAddPerson()
        {
            // Arrange
            var person = new Person("TestName", DateTimeOffset.UtcNow);
            _mockPersonRepository.Setup(r => r.AddPerson(person)); // Setup repository method

            // Act
            _personService.AddPerson(person);

            // Assert
            _mockPersonRepository.Verify(r => r.AddPerson(person), Times.Once); // Verify method was called
        }

        [Fact]
        public void AddPerson_NullPerson_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _personService.AddPerson(null));
            _mockPersonRepository.Verify(r => r.AddPerson(It.IsAny<Person>()), Times.Never); // Ensure repository not called
        }

        [Fact]
        public void AddPerson_RepositoryThrowsException_ShouldLogErrorAndRethrow()
        {
            // Arrange
            var person = new Person("TestName", DateTimeOffset.UtcNow);
            _mockPersonRepository.Setup(r => r.AddPerson(person))
                                 .Throws(new InvalidOperationException("DB connection lost"));

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _personService.AddPerson(person));
            Assert.Equal("DB connection lost", ex.Message);
            _mockPersonRepository.Verify(r => r.AddPerson(person), Times.Once);
     
        }

        // --- Test Cases for GetPersonById method ---

        [Fact]
        public void GetPersonById_ExistingId_ShouldReturnPerson()
        {
            // Arrange
            var personId = Guid.NewGuid().ToString();
            var expectedPerson = new Person(personId, "Alice", DateTimeOffset.UtcNow);
            _mockPersonRepository.Setup(r => r.GetPersonById(personId)).Returns(expectedPerson);

            // Act
            var result = _personService.GetPersonById(personId);

            // Assert
            Assert.Equal(expectedPerson, result);
            _mockPersonRepository.Verify(r => r.GetPersonById(personId), Times.Once);
        }

        [Fact]
        public void GetPersonById_NonExistingId_ShouldReturnNull()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid().ToString();
            _mockPersonRepository.Setup(r => r.GetPersonById(nonExistingId)).Returns((Person)null);

            // Act
            var result = _personService.GetPersonById(nonExistingId);

            // Assert
            Assert.Null(result);
            _mockPersonRepository.Verify(r => r.GetPersonById(nonExistingId), Times.Once);
        }

        [Fact]
        public void GetPersonById_RepositoryThrowsException_ShouldLogErrorAndRethrow()
        {
            // Arrange
            var personId = Guid.NewGuid().ToString();
            _mockPersonRepository.Setup(r => r.GetPersonById(personId))
                                 .Throws(new DataAccessException("Repository error on GetById", new Exception("Inner")));

            // Act & Assert
            var ex = Assert.Throws<DataAccessException>(() => _personService.GetPersonById(personId));
            Assert.Contains("Repository error on GetById", ex.Message);
            _mockPersonRepository.Verify(r => r.GetPersonById(personId), Times.Once);
        }

        // --- Test Cases for GetAllPersons method ---

        [Fact]
        public void GetAllPersons_ShouldReturnAllPersonsFromRepository()
        {
            // Arrange
            var persons = new List<Person>
            {
                new Person("1", "Alice", DateTimeOffset.UtcNow),
                new Person("2", "Bob", DateTimeOffset.UtcNow)
            };
            _mockPersonRepository.Setup(r => r.GetAllPersons()).Returns(persons);

            // Act
            var result = _personService.GetAllPersons();

            // Assert
            Assert.Equal(persons.Count, result.Count);
            Assert.Contains(persons[0], result);
            Assert.Contains(persons[1], result);
            _mockPersonRepository.Verify(r => r.GetAllPersons(), Times.Once);
        }

        [Fact]
        public void GetAllPersons_NoPersonsInRepository_ShouldReturnEmptyList()
        {
            // Arrange
            _mockPersonRepository.Setup(r => r.GetAllPersons()).Returns(new List<Person>());

            // Act
            var result = _personService.GetAllPersons();

            // Assert
            Assert.Empty(result);
            _mockPersonRepository.Verify(r => r.GetAllPersons(), Times.Once);
        }

        [Fact]
        public void GetAllPersons_RepositoryThrowsException_ShouldLogErrorAndRethrow()
        {
            // Arrange
            _mockPersonRepository.Setup(r => r.GetAllPersons())
                                 .Throws(new DataAccessException("Repository error on GetAll", new Exception("Inner")));

            // Act & Assert
            var ex = Assert.Throws<DataAccessException>(() => _personService.GetAllPersons());
            Assert.Contains("Repository error on GetAll", ex.Message);
            _mockPersonRepository.Verify(r => r.GetAllPersons(), Times.Once);
        }

        // --- Test Cases for GetPersonByName method ---

        [Fact]
        public void GetPersonByName_ExistingName_ShouldReturnMatchingPersons()
        {
            // Arrange
            var name = "Alice";
            var persons = new List<Person>
            {
                new Person("1", "Alice", DateTimeOffset.UtcNow),
                new Person("2", "Alice", DateTimeOffset.UtcNow),
                new Person("3", "Bob", DateTimeOffset.UtcNow)
            };
            _mockPersonRepository.Setup(r => r.GetPersonByName(name)).Returns(persons.Where(p => p.Name == name).ToList());

            // Act
            var result = _personService.GetPersonByName(name);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal(name, p.Name));
            _mockPersonRepository.Verify(r => r.GetPersonByName(name), Times.Once);
        }

        [Fact]
        public void GetPersonByName_NonExistingName_ShouldReturnEmptyList()
        {
            // Arrange
            var name = "Zara";
            _mockPersonRepository.Setup(r => r.GetPersonByName(name)).Returns(new List<Person>());

            // Act
            var result = _personService.GetPersonByName(name);

            // Assert
            Assert.Empty(result);
            _mockPersonRepository.Verify(r => r.GetPersonByName(name), Times.Once);
        }

        [Fact]
        public void GetPersonByName_RepositoryThrowsException_ShouldLogErrorAndRethrow()
        {
            // Arrange
            var name = "Alice";
            _mockPersonRepository.Setup(r => r.GetPersonByName(name))
                                 .Throws(new DataAccessException("Repository error on GetByName", new Exception("Inner")));

            // Act & Assert
            var ex = Assert.Throws<DataAccessException>(() => _personService.GetPersonByName(name));
            Assert.Contains("Repository error on GetByName", ex.Message);
            _mockPersonRepository.Verify(r => r.GetPersonByName(name), Times.Once);
        }

        // --- Test Cases for UpdatePerson method ---

        [Fact]
        public void UpdatePerson_ValidPerson_ShouldCallRepositoryUpdatePerson()
        {
            // Arrange
            var person = new Person("1", "UpdatedName", DateTimeOffset.UtcNow);
            _mockPersonRepository.Setup(r => r.UpdatePerson(person));

            // Act
            _personService.UpdatePerson(person);

            // Assert
            _mockPersonRepository.Verify(r => r.UpdatePerson(person), Times.Once);
        }

        [Fact]
        public void UpdatePerson_NullPerson_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _personService.UpdatePerson(null));
            _mockPersonRepository.Verify(r => r.UpdatePerson(It.IsAny<Person>()), Times.Never);
        }


        // --- Test Cases for DeletePerson method ---

        [Fact]
        public void DeletePerson_ValidId_ShouldCallRepositoryDeletePerson()
        {
            // Arrange
            var personId = Guid.NewGuid().ToString();
            _mockPersonRepository.Setup(r => r.DeletePerson(personId));

            // Act
            _personService.DeletePerson(personId);

            // Assert
            _mockPersonRepository.Verify(r => r.DeletePerson(personId), Times.Once);
        }

        [Fact]
        public void DeletePerson_RepositoryThrowsException_ShouldLogErrorAndRethrow()
        {
            // Arrange
            var personId = Guid.NewGuid().ToString();
            _mockPersonRepository.Setup(r => r.DeletePerson(personId))
                                 .Throws(new DataAccessException("Repository error on Delete", new Exception("Inner")));

            // Act & Assert
            var ex = Assert.Throws<DataAccessException>(() => _personService.DeletePerson(personId));
            Assert.Contains("Repository error on Delete", ex.Message);
            _mockPersonRepository.Verify(r => r.DeletePerson(personId), Times.Once);
        }

        // --- Test Cases for GenerateAddAndRetrieveRandomPersons method ---

        [Fact]
        public void GenerateAddAndRetrieveRandomPersons_ValidCount_ShouldAddAndReturnPersons()
        {
            // Arrange
            int count = 3;
            var addedPersons = new List<Person>();
            _mockPersonRepository.Setup(r => r.AddPerson(It.IsAny<Person>()))
                                 .Callback<Person>(p => addedPersons.Add(p));

            // Act
            var result = _personService.GenerateAddAndRetrieveRandomPersons(count);

            // Assert
            Assert.Equal(count, result.Count);
            Assert.All(result, p => Assert.NotNull(p.Id));
            Assert.All(result, p => Assert.True(p.Name == "Alice" || p.Name == "Bob" || p.Name == "Charlie" || p.Name == "Diana" || p.Name == "Ethan"));
            _mockPersonRepository.Verify(r => r.AddPerson(It.IsAny<Person>()), Times.Exactly(count));
            Assert.Equal(result.Count, addedPersons.Count); // Verify that people created internally were passed to repo
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GenerateAddAndRetrieveRandomPersons_InvalidCount_ShouldThrowArgumentOutOfRangeException(int invalidCount)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _personService.GenerateAddAndRetrieveRandomPersons(invalidCount));
            _mockPersonRepository.Verify(r => r.AddPerson(It.IsAny<Person>()), Times.Never);
        }



        // --- Test Cases for GetPersonsByNameAndAge method ---

        [Fact]
        public void GetPersonsByNameAndAge_ValidNameAndAge_ShouldReturnFilteredPersons()
        {
            // Arrange
            var targetName = "Alice";
            var targetAge = 30;
            var persons = new List<Person>
            {
                new Person("1", "Alice", DateTimeOffset.UtcNow.AddYears(-30).AddDays(-1)), // Alice, 30+
                new Person("2", "Bob", DateTimeOffset.UtcNow.AddYears(-35)),           // Bob, 35+
                new Person("3", "Alice", DateTimeOffset.UtcNow.AddYears(-25)),         // Alice, 25 (too young)
                new Person("4", "Alice", DateTimeOffset.UtcNow.AddYears(-40)),         // Alice, 40+
            };
            _mockPersonRepository.Setup(r => r.GetPersonByName(targetName)).Returns(persons.Where(p => p.Name == targetName).ToList());

            // Act
            var result = _personService.GetPersonsByNameAndAge(targetName, targetAge);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal(targetName, p.Name));
            Assert.DoesNotContain(result, p => p.Id == "3"); // Ensure the too-young Alice is excluded
            _mockPersonRepository.Verify(r => r.GetPersonByName(targetName), Times.Once);
        }


        [Fact]
        public void GetPersonsByNameAndAge_RepositoryThrowsException_ShouldLogErrorAndRethrow()
        {
            // Arrange
            _mockPersonRepository.Setup(r => r.GetPersonByName(It.IsAny<string>()))
                                 .Throws(new DataAccessException("Repository error on GetByName for age filter", new Exception("Inner")));

            // Act & Assert
            var ex = Assert.Throws<DataAccessException>(() => _personService.GetPersonsByNameAndAge("Alice", 25));
            Assert.Contains("Repository error on GetByName for age filter", ex.Message);
            _mockPersonRepository.Verify(r => r.GetPersonByName("Alice"), Times.Once);
        }

        // --- Test Cases for UpdateMarriageRecord method ---

        [Fact]
        public void UpdateMarriageRecord_ValidPersons_ShouldUpdateNameAndRepository()
        {
            // Arrange
            var personId = Guid.NewGuid().ToString();
            var partnerId = Guid.NewGuid().ToString();
            var person = new Person(personId, "Alice", DateTimeOffset.UtcNow.AddYears(-25));
            var partner = new Person(partnerId, "Bob", DateTimeOffset.UtcNow.AddYears(-27));
            var expectedNewName = "Alice Bob"; // Alice's name should become "Alice Bob"

            _mockPersonRepository.Setup(r => r.GetPersonById(personId)).Returns(person);
            _mockPersonRepository.Setup(r => r.GetPersonById(partnerId)).Returns(partner);
            _mockPersonRepository.Setup(r => r.UpdatePerson(It.Is<Person>(p => p.Id == personId && p.Name == expectedNewName)));

            // Act
            var updatedPerson = _personService.UpdateMarriageRecord(personId, partnerId);

            // Assert
            Assert.NotNull(updatedPerson);
            Assert.Equal(personId, updatedPerson.Id);
            Assert.Equal(expectedNewName, updatedPerson.Name);
            _mockPersonRepository.Verify(r => r.GetPersonById(personId), Times.Once);
            _mockPersonRepository.Verify(r => r.GetPersonById(partnerId), Times.Once);
            _mockPersonRepository.Verify(r => r.UpdatePerson(It.Is<Person>(p => p.Id == personId && p.Name == expectedNewName)), Times.Once);
        }

        [Theory]
        [InlineData(null, "partnerId")]
        [InlineData("", "partnerId")]
        [InlineData(" ", "partnerId")]
        public void UpdateMarriageRecord_InvalidPersonId_ShouldThrowArgumentException(string invalidPersonId, string partnerId)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _personService.UpdateMarriageRecord(invalidPersonId, partnerId));
            _mockPersonRepository.Verify(r => r.GetPersonById(It.IsAny<string>()), Times.Never);
            _mockPersonRepository.Verify(r => r.UpdatePerson(It.IsAny<Person>()), Times.Never);
        }

        [Theory]
        [InlineData("personId", null)]
        [InlineData("personId", "")]
        [InlineData("personId", " ")]
        public void UpdateMarriageRecord_InvalidPartnerId_ShouldThrowArgumentException(string personId, string invalidPartnerId)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _personService.UpdateMarriageRecord(personId, invalidPartnerId));
            _mockPersonRepository.Verify(r => r.GetPersonById(It.IsAny<string>()), Times.Never);
            _mockPersonRepository.Verify(r => r.UpdatePerson(It.IsAny<Person>()), Times.Never);
        }

        [Fact]
        public void UpdateMarriageRecord_PersonNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var personId = Guid.NewGuid().ToString();
            var partnerId = Guid.NewGuid().ToString();
            _mockPersonRepository.Setup(r => r.GetPersonById(personId)).Returns((Person)null); // Person not found
            _mockPersonRepository.Setup(r => r.GetPersonById(partnerId)).Returns(new Person(partnerId, "Partner", DateTimeOffset.UtcNow));

            // Act & Assert
            var ex = Assert.Throws<KeyNotFoundException>(() => _personService.UpdateMarriageRecord(personId, partnerId));
            Assert.Contains("Either person or partner not found.", ex.Message);
            _mockPersonRepository.Verify(r => r.GetPersonById(personId), Times.Once);
            _mockPersonRepository.Verify(r => r.GetPersonById(partnerId), Times.Once); // Partner search might still happen
            _mockPersonRepository.Verify(r => r.UpdatePerson(It.IsAny<Person>()), Times.Never);
        }

        [Fact]
        public void UpdateMarriageRecord_PartnerNotFound_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var personId = Guid.NewGuid().ToString();
            var partnerId = Guid.NewGuid().ToString();
            _mockPersonRepository.Setup(r => r.GetPersonById(personId)).Returns(new Person(personId, "Person", DateTimeOffset.UtcNow));
            _mockPersonRepository.Setup(r => r.GetPersonById(partnerId)).Returns((Person)null); // Partner not found

            // Act & Assert
            var ex = Assert.Throws<KeyNotFoundException>(() => _personService.UpdateMarriageRecord(personId, partnerId));
            Assert.Contains("Either person or partner not found.", ex.Message);
            _mockPersonRepository.Verify(r => r.GetPersonById(personId), Times.Once);
            _mockPersonRepository.Verify(r => r.GetPersonById(partnerId), Times.Once);
            _mockPersonRepository.Verify(r => r.UpdatePerson(It.IsAny<Person>()), Times.Never);
        }

    }

    // Dummy DataAccessException for testing purposes
    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception innerException = null) : base(message, innerException) { }
    }
}
