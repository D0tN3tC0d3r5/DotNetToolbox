namespace System.Security;

public class PasswordPolicyTests {
    [Fact]
    public void TryVerify_ReturnsSuccess() {
        var subject = new PasswordPolicy();

        var result = subject.Enforce("password");

        result.IsSuccess.Should().BeTrue();
    }
}
