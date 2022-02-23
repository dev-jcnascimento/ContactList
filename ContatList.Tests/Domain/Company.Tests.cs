using System.ComponentModel.DataAnnotations;
using ContactList.Core.Domain;
using Xunit;
using Xunit.Abstractions;

namespace ContactList.Tests.Domain
{
    public class CompanyFixture
    {
        public Company company => new Company(1, "Nova Empresa");

    }
    public class CompanyTests : IClassFixture<CompanyFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly CompanyFixture _fixture;

        public CompanyTests(ITestOutputHelper testOutputHelper, CompanyFixture companyFixture)
        {
            _testOutputHelper = testOutputHelper;
            _fixture = companyFixture;
            _testOutputHelper.WriteLine("Constructor");
        }

        [Fact]
        public void CompanyTest_IsNullOrEmpty_ReturnFalse()
        {
            _testOutputHelper.WriteLine("Ckeck_IsNullOrEmpty");
            Assert.True(!string.IsNullOrEmpty(_fixture.company.Name));
        }
        [Fact]
        public void CompanyTest_BiggerThen3charLess60_ReturnTrue()
        {
            _testOutputHelper.WriteLine("Check_BiggerThen3charLess60");
            var name = _fixture.company.Name.Length;
            Assert.True(name < 60 && name > 3);

        }
        [Fact]
        public void CompanyTest_ExceptionIsNull()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Company(1, MakeString(0)));
            Assert.Equal("The Company Name cannot be empty!", nameException.Message);

        }
        [Fact]
        public void CompanyTest_ExceptionIsLess3()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Company(1, MakeString(2)));
            Assert.Equal("The Company Name must contain more than 3 characters and less than 50", nameException.Message);

        }
        [Fact]
        public void CompanyTest_ExceptionIsMore50()
        {
            var nameException = Assert.Throws<ValidationException>(() => new Company(1, MakeString(80)));
            Assert.Equal("The Company Name must contain more than 3 characters and less than 50", nameException.Message);

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
