using BuisnessLogic.Models;
using BuisnessLogic.Api;
using Xunit;

namespace BuisnessLogic.Tests
{
    public class BuisnessLogicApiTests
    {
        [Fact]
        public void Create_InputIsRequestUserModel_ReturnResponseUserModel()
        {
            // Arrange

            IBuisnessLogicApi api = new BuisnessLogicApi();

            RequestUserModelBuilder builder = new RequestUserModelBuilder();
            builder.SetName("TestUser");
            builder.SetEmail("test@example.com");
            builder.SetId(Guid.NewGuid());

            RequestUserModel model = builder.Build();

            // Act

            ResponseUserModel result = awaapi.Create(model);

            // Assert

            Assert.Equal(model.Name, result.Name);
            Assert.Equal(model.Email, result.Email);
            Assert.Equal(model.Id, result.Id);
        }

        /*[Fact]
        public void Alter_InputIsRequestUserModel_ReturnResponseUserModel()
        {
            // Arrange

            IBuisnessLogicApi api = new BuisnessLogicApi();

            RequestUserModelBuilder builder = new ResponseUserModelBuilder();
            builder.SetName("TestUser");
            builder.SetEmail("test@example.com");
            builder.SetId(Guid.NewGuid());

            RequestUserModel model = builder.Build();

            // Act

            ResponseUserModel result = api.Alter(model);

            // Assert

            Assert.Equal(model.Name, result.Name);
            Assert.Equal(model.Email, result.Email);
            Assert.Equal(model.Id, result.Id);
        }

        [Fact]
        public void Validate_InputIsRequestUserModel_ReturnTrue()
        {
            // Arrange

            BuisnessLogicApi api = new BuisnessLogicApi();

            RequestUserModelBuilder builder = new ResponseUserModelBuilder();
            builder.SetName("TestUser");
            builder.SetEmail("test@example.com");
            builder.SetId(Guid.NewGuid());

            RequestUserModel model = builder.Build();

            // Act

            bool result = api.Validate(model);

            // 
        } */
    }
}
