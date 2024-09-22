namespace PasswordUtils.Tests
{
    public class PasswordHasherTests
    {
        [Theory]
        [InlineData("123456")]
        [InlineData("qwerty")]
        [InlineData("vdw3t|^&*wHDgw")]
        public void CheckGenerationAndVerification_InputIsPassword_ReturnHashAndVerify(string input)
        {
            // Arrange

            IPasswordHasher hasher = new PasswordHasher();
            string encoded = hasher.Generate(input);

            // Act

            bool correct = hasher.Verify(input, encoded);

            // Assert

            Assert.True(correct);
        }
    }
}
