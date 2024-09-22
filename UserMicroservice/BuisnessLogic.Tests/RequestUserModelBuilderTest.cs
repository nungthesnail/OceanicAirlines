using Xunit;
using BuisnessLogic.Models;
using BuisnessLogic.Models.Exceptions;

namespace BuisnessLogic.Tests
{
    public class RequestUserModelBuilderTest
    {
        private const string CORRECT_JSON = "{\"id\": \"1ab0161f-a3e9-4405-ab94-eda61931cb6d\", \"name\": \"test-user\", \"email\": \"test@example.com\"}";
        private const string INCORRECT_JSON = "{\"id\": \"1ab0161f-a3e9-4405-ab94-eda61931cb6d\", \"name\": \"test-user\" \"123\": 123}";

        [Theory]
        [InlineData(CORRECT_JSON)]
        public void BuildFromJson_InputIsCorrectJson_ReturnRequestModel(string json)
        {
            // Arrange

            RequestUserModelBuilder builder = new RequestUserModelBuilder();

            // Act

            RequestUserModel model = builder.BuildFromJson(json);

            //Assert

            Assert.True(model is RequestUserModel);
        }

        [Theory]
        [InlineData(INCORRECT_JSON)]
        public void BuildFromJson_InputIsIncorrectJson_ThrowsException(string json)
        {
            // Arrange

            RequestUserModelBuilder builder = new RequestUserModelBuilder();

            // Act

            Action act = () => builder.BuildFromJson(json);

            //Assert

            Assert.Throws<ModelBuildingException>(act);
        }

        [Theory]
        [InlineData("1ab0161f-a3e9-4405-ab94-eda61931cb6d", "test-user", "test@example.com")]
        public void BuildByProperties_InputIsPropeties_ReturnCorrect(string guid_string, string name, string email)
        {
            // Arrange

            Guid guid = Guid.Parse(guid_string);
            RequestUserModelBuilder builder = new RequestUserModelBuilder();

            // Act

            builder.SetId(guid);
            builder.SetName(name);
            builder.SetEmail(email);
            RequestUserModel model = builder.Build();

            // Assert

            Assert.Equal(guid_string, model.Id.ToString());
            Assert.Equal(name, model.Name);
            Assert.Equal(email, model.Email);
        }

        [Fact]
        public void BuildByProperties_NoInput_ThrowsException()
        {
            // Arrange

            RequestUserModelBuilder builder = new RequestUserModelBuilder();

            // Act

            Action act = () => builder.Build();

            // Assert

            Assert.Throws<ModelBuildingException>(act);
        }
    }
}