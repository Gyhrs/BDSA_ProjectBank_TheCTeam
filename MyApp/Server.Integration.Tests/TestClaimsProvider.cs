//This class has been taken from https://gunnarpeipman.com/aspnet-core-integration-tests-users-roles/ and inspired from group TeamLitExplorer

namespace MyApp.Server.Integration.Tests;

public class TestClaimsProvider
{
    public IList<Claim> Claims { get; }
 
    public TestClaimsProvider(IList<Claim> claims)
    {
        Claims = claims;
    }
 
    public TestClaimsProvider()
    {
        Claims = new List<Claim>();
    }
 
    public static TestClaimsProvider WithUserClaims()
    {
        var _provider = new TestClaimsProvider();
        _provider.Claims.Add(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
        _provider.Claims.Add(new Claim(ClaimTypes.Name, "User"));
        _provider.Claims.Add(new Claim("http://schemas.microsoft.com/identity/claims/scope", "API.Access"));
 
        return _provider;
    }
}
