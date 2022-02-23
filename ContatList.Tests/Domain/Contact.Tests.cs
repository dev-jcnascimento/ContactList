using System.ComponentModel.DataAnnotations;
using ContactList.Core.Domain;
using Xunit;
using Xunit.Abstractions;

namespace ContactList.Tests.Domain
{
    public class ContactFixture
    {
        public Contact contactValid => new Contact(1, 1, "First Test", "Last Test", "67359487245",
           "bbbbbbbbT", "test@test.com", "Street Leopoldina, Number 45, Brazil");
    }
    public class ContactTests : IClassFixture<ContactFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ContactFixture _fixture;

        public ContactTests(ITestOutputHelper testOutputHelper, ContactFixture contactFixture)
        {
            _testOutputHelper = testOutputHelper;
            _fixture = contactFixture;
            _testOutputHelper.WriteLine("Constructor");
        }

        [Fact]
        public void ContactTest_FirstName_IsNullOrEmpty_ReturnFalse()
        {
            _testOutputHelper.WriteLine("Ckeck_IsNullOrEmpty");
            Assert.True(!string.IsNullOrEmpty(_fixture.contactValid.FirstName));
        }
        [Fact]
        public void ContactTest_NameFirstName_BiggerThen3charLess60_ReturnTrue()
        {
            _testOutputHelper.WriteLine("Check_BiggerThen3charLess60");
            var name = _fixture.contactValid.FirstName.Length;
            Assert.True(name < 60 && name > 3);
        }
        [Fact]
        public void ContactTest_LastName_IsNullOrEmpty_ReturnFalse()
        {
            _testOutputHelper.WriteLine("Ckeck_IsNullOrEmpty");
            Assert.True(!string.IsNullOrEmpty(_fixture.contactValid.LastName));
        }
        [Fact]
        public void ContactTest_LastFirst_BiggerThen3charLess60_ReturnTrue()
        {
            _testOutputHelper.WriteLine("Check_BiggerThen3charLess60");
            var name = _fixture.contactValid.LastName.Length;
            Assert.True(name < 60 && name > 3);
        }
        [Fact]
        public void Validate_GivenShorterThan8Characters_Password()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Test", "Last Test", "6735942465",
           MakeString(4), "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("Password must be longer than 8 characters", nameException.Message);
        }
        [Fact]
        public void Validate_GivenOneUpperCase_Password()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Test", "Last Test", "6735942465",
            MakeString(9), "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("Password must contain at least one uppercase character", nameException.Message);
        }
        [Fact]
        public void Validate_IsNullOrEmpty_Password()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Test", "Last Test", "6735942465",
            "", "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("Password cannot be empty", nameException.Message);
        }
        [Fact]
        public void Validate_Email_IsNullOrEmpty()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Test", "Last Test", "6735942465",
           MakeString(9) + "A", "", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("Email cannot be empty", nameException.Message);
        }
        [Fact]
        public void Validate_Email_ReturnFalse()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Test", "Last Test", "6735942465",
           MakeString(9) + "A", "testtestcom", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("The email provided is not valid", nameException.Message);
        }
        [Fact]
        public void ContactTest_ExceptionIsNull_FirstName()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "", "Last Test", "6735942465",
           MakeString(9) + "A", "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("The Contact FirstName cannot be empty!", nameException.Message);

        }
        [Fact]
        public void ContactTest_ExceptionIsLess3_FirstName()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, MakeString(2), "Last Test", "6735942465",
           MakeString(9) + "A", "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("The Contact FirstName must contain more than 3 characters and less than 50", nameException.Message);

        }
        [Fact]
        public void ContactTest_ExceptionIsMore50_FirstName()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, MakeString(80), "Last Test", "6735942465",
           MakeString(9) + "A", "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("The Contact FirstName must contain more than 3 characters and less than 50", nameException.Message);
        }
        [Fact]
        public void ContactTest_ExceptionIsNull_LastName()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Name", "", "6735942465",
           MakeString(9) + "A", "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("The Contact LastName cannot be empty!", nameException.Message);

        }
        [Fact]
        public void ContactTest_ExceptionIsLess3_LastName()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Name", MakeString(2), "6735942465",
           MakeString(9) + "A", "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("The Contact LastName must contain more than 3 characters and less than 50", nameException.Message);

        }
        [Fact]
        public void ContactTest_ExceptionIsMore50_LastName()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Name", MakeString(80), "6735942465",
           MakeString(9) + "A", "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("The Contact LastName must contain more than 3 characters and less than 50", nameException.Message);
        }
        [Fact]
        public void ContactTest_ExceptionIsMore10_Phone()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Name", "Last Test", "67 3594",
           MakeString(9) + "A", "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("The phone number must contain more than 10 digits!", nameException.Message);
        }
        [Fact]
        public void ContactTest_ExceptionOnlyNumber_Phone()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Contact(1, 1, "First Name", "Last Test", "67 3594as.asdf",
           MakeString(9) + "A", "test@test.com", "Street Leopoldina, Number 45, Brazil"));
            Assert.Equal("The phone number cannot contain letters and special characters", nameException.Message);
        }

        public static string MakeString(int length)
        {
            var result = "";
            for (int i = 1; i <= length; i++)
            {
                result += "a";
            }
            return result;
        }
    }
}
