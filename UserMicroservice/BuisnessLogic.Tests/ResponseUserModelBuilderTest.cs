using Xunit;
using BuisnessLogic.Models;
using BuisnessLogic.Models.Exceptions;

namespace BuisnessLogic.Tests
{
    public class ResponseUserModelBuilderTest
    {
        private const string CORRECT_JSON = "{\"id\": \"1ab0161f-a3e9-4405-ab94-eda61931cb6d\", \"name\": \"test-user\", \"email\": \"test@example.com\", \"creationDateTime\": \"2019-07-26T00:00:00\"}";
        private const string INCORRECT_JSON = "{\"id\": \"1ab0161f-a3e9-4405-ab94-eda61931cb6d\", \"name\": \"test-user\" \"123\": 123}";

        [Theory]
        [InlineData(CORRECT_JSON)]
        public void BuildFromJson_InputIsCorrectJson_ReturnResponseModel(string json)
        {
            // Arrange

            ResponseUserModelBuilder builder = new ResponseUserModelBuilder();

            // Act

            ResponseUserModel model = builder.BuildFromJson(json);

            //Assert

            Assert.True(model is ResponseUserModel);
        }

        [Theory]
        [InlineData(INCORRECT_JSON)]
        public void BuildFromJson_InputIsIncorrectJson_ThrowsException(string json)
        {
            // Arrange

            ResponseUserModelBuilder builder = new ResponseUserModelBuilder();

            // Act

            Action act = () => builder.BuildFromJson(json);

            //Assert

            Assert.Throws<ModelBuildingException>(act);
        }

        [Theory]
        [InlineData("1ab0161f-a3e9-4405-ab94-eda61931cb6d", "test-user", "test@example.com", 2024, 7, 3)]
        public void BuildByProperties_InputIsPropeties_ReturnCorrect(
            string guid_string,
            string name,
            string email,
            int year,
            int month,
            int day)
        {
            // Arrange

            Guid guid = Guid.Parse(guid_string);
            DateTime dt = new DateTime(year, month, day);
            ResponseUserModelBuilder builder = new ResponseUserModelBuilder();

            // Act

            builder.SetId(guid);
            builder.SetName(name);
            builder.SetEmail(email);
            builder.SetCreatedAt(dt);
            ResponseUserModel model = builder.Build();

            // Assert

            Assert.Equal(guid_string, model.Id.ToString());
            Assert.Equal(name, model.Name);
            Assert.Equal(email, model.Email);
            Assert.Equal(dt, model.CreatedAt);
        }

        [Fact]
        public void BuildByProperties_NoInput_ThrowsException()
        {
            // Arrange

            ResponseUserModelBuilder builder = new ResponseUserModelBuilder();

            // Act

            Action act = () => builder.Build();

            // Assert

            Assert.Throws<ModelBuildingException>(act);
        }
    }
}