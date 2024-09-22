using EntityFrameworkLogic;

namespace EntityFrameworkLogicTests
{
    public class EFInitTest
    {
        [Fact]
        public void Test1()
        {
            var context = new ApplicationContext();
            context.Database.EnsureCreated();
            context.PasswordHashes.Add(new PasswordHash { Id = Guid.NewGuid(), HashedPassword = "12345", LinkedUserId = Guid.NewGuid() });
            context.SaveChanges();

            Assert.True(5 == 5);
        }
    }
}