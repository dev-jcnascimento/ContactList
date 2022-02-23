using System.ComponentModel.DataAnnotations;
using ContactList.Core.Domain;
using Xunit;
using Xunit.Abstractions;

namespace ContactList.Tests.Domain
{
    public class ContactBookFixture
    {
        public ContactBook contactBook => new ContactBook("Nova Agenda");

    }
    public class ContactBookTests : IClassFixture<ContactBookFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ContactBookFixture _fixture;

        public ContactBookTests(ITestOutputHelper testOutputHelper, ContactBookFixture contactBookFixture)
        {
            _testOutputHelper = testOutputHelper;
            _fixture = contactBookFixture;
            _testOutputHelper.WriteLine("Constructor");
        }

        [Fact]
        public void ContactBookTest_IsNullOrEmpty_ReturnFalse()
        {
            _testOutputHelper.WriteLine("Ckeck_IsNullOrEmpty");
            Assert.True(!string.IsNullOrEmpty(_fixture.contactBook.Name));
        }
        [Fact]
        public void ContactBookTest_BiggerThen3charLess60_ReturnTrue()
        {
            _testOutputHelper.WriteLine("Check_BiggerThen3charLess60");
            Assert.NotNull(_fixture.contactBook.Name);

        }
        [Fact]
        public void ContactBookTest_ExceptionIsNull()
        {
            var nameException = Assert.Throws<ValidationException>(() => new ContactBook(MakeString(0)));
            Assert.Equal("The ContactBook Name cannot be empty!", nameException.Message);

        }
        [Fact]
        public void ContactBookTest_ExceptionIsLess3()
        {
            var nameException = Assert.Throws<ValidationException>(() => new ContactBook(MakeString(2)));
            Assert.Equal("The ContactBook Name must contain more than 3 characters and less than 50", nameException.Message);

        }
        [Fact]
        public void ContactBookTest_ExceptionIsMore50()
        {
            var nameException = Assert.Throws<ValidationException>(() => new ContactBook(MakeString(80)));
            Assert.Equal("The ContactBook Name must contain more than 3 characters and less than 50", nameException.Message);

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
