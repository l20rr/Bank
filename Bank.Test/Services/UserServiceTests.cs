using Bank.Client.Services;
using Bank.Shared;
using Xunit;
using FakeItEasy;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Bank.Test.Services
{

    public class UserDataServiceTests
    {
        private UserDataService userDataService;
        private HttpClient fakeHttpClient;

        public string UPassword { get; private set; }

        public UserDataServiceTests()
        {
            fakeHttpClient = A.Fake<HttpClient>();
            userDataService = new UserDataService(fakeHttpClient);
        }

        [Fact]
        public void ValidatePasswords_PasswordsAreEqual_ReturnsTrue()
        {
            // Arrange
            var faker = new Faker();
            string password = faker.Internet.Password();
            string confirmPassword = password;

            // Act
            bool result = userDataService.ValidatePasswords(password, confirmPassword);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateUserAreEqual_ReturnsTrue()
        {
            // Arrange
            var faker = new Faker();
            int userId = faker.Random.Int(1, 1000);
            string firstName = faker.Name.FirstName();
            string lastName = faker.Name.LastName();
            string email = faker.Internet.Email();
            string password = faker.Internet.Password();
            string confirmPassword = password;
            DateTime joinedDate = faker.Date.Past();

            // Act
            bool result = userDataService.ValidateUsers(userId, firstName, lastName, email, password, confirmPassword, joinedDate);

            // Assert
            Assert.True(result);
        }
   

        [Fact]
        public void ValidatePasswords_PasswordsAreDifferent_ReturnsFalse()
        {
            // Arrange
            var faker = new Faker();
            string password = faker.Internet.Password();
            string confirmPassword = faker.Internet.Password();

            // Ensure that the generated passwords are different
            while (password == confirmPassword)
            {
                confirmPassword = faker.Internet.Password();
            }

            // Act
            bool result = userDataService.ValidatePasswords(password, confirmPassword);

            // Assert
            Assert.False(result);
        }


      
    }
}
