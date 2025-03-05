using Microsoft.VisualStudio.TestTools.UnitTesting;
using LinkedInTester;
using System.Collections.Generic;

namespace LinkedInTester.Tests
{
    [TestClass]
    public class PersonTests
    {
        [TestMethod]
        public void Person_ToString_ReturnsCorrectFormat()
        {
            // Arrange
            var person = new Person
            {
                Name = "John Doe",
                Position = "Developer",
                Validated = true
            };

            // Act
            var result = person.ToString();

            // Assert
            Assert.AreEqual("John Doe,Developer,True", result);
        }

        [TestMethod]
        public void Person_NameProperty_SetAndGet()
        {
            // Arrange
            var person = new Person();

            // Act
            person.Name = "Alice";

            // Assert
            Assert.AreEqual("Alice", person.Name);
        }

        [TestMethod]
        public void Person_PositionProperty_SetAndGet()
        {
            // Arrange
            var person = new Person();

            // Act
            person.Position = "Engineer";

            // Assert
            Assert.AreEqual("Engineer", person.Position);
        }

        [TestMethod]
        public void Person_ValidatedProperty_SetAndGet()
        {
            // Arrange
            var person = new Person();

            // Act
            person.Validated = true;

            // Assert
            Assert.AreEqual(true, person.Validated);
        }
    }

    [TestClass]
    public class CompanyTests
    {
        [TestMethod]
        public void Company_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var company = new Company();

            // Assert
            Assert.AreEqual(string.Empty, company.Name);
            Assert.IsNotNull(company.People);
            Assert.AreEqual(0, company.People.Count);
        }

        [TestMethod]
        public void Company_AddPerson_IncreasesPeopleCount()
        {
            // Arrange
            var company = new Company();
            var person = new Person
            {
                Name = "Jane Doe",
                Position = "Manager",
                Validated = false
            };

            // Act
            company.People.Add(person);

            // Assert
            Assert.AreEqual(1, company.People.Count);
            Assert.AreEqual(person, company.People[0]);
        }
    }
}

