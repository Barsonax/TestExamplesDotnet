using System.Net;
using System.Security.Claims;
using ApiJwtAuth.Nunit.TestSetup;
using ApiJwtAuth.Sut;

namespace ApiJwtAuth.Nunit;

public class ApiJwtAuthTests : TestBase
{
    [Test]
    public async Task Unauthenticated_Client_ShouldRespondWith401()
    {
        //Arrange
        Sut.SeedData(context =>
        {
            context.Blogs.Add(new Blog
            {
                Url = "https://blog.photogrammer.net/"
            });
        });

        //Act
        var result = await Sut.CreateClient().GetAsync("blogs");

        //Assert
        result.Should().HaveStatusCode(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task Authenticated_Client_ShouldRespondWith403()
    {
        //Arrange
        Sut.SeedData(context =>
        {
            context.Blogs.Add(new Blog
            {
                Url = "https://blog.photogrammer.net/"
            });
        });

        //Act
        var result = await Sut.CreateAuthorizedClient().GetAsync("blogs");

        //Assert
        result.Should().HaveStatusCode(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task Authenticated_And_Authorized_Client_ShouldRespondWith200()
    {
        //Arrange
        Sut.SeedData(context =>
        {
            context.Blogs.Add(new Blog
            {
                Url = "https://blog.photogrammer.net/"
            });
        });

        //Act
        var result = await Sut.CreateAuthorizedClient(new Claim("SpecialPermission", string.Empty)).GetAsync("blogs");

        //Assert
        result.Should().HaveStatusCode(HttpStatusCode.OK);
        var blogs = await result.Content.ReadFromJsonAsync<Blog[]>();

        blogs.Should().BeEquivalentTo(new[]
        {
            new
            {
                Url = "https://blog.photogrammer.net/"
            }
        });
    }
}
