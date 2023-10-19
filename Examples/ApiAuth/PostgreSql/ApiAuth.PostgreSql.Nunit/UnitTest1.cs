using ApiAuth.PostgreSql.Nunit.TestSetup;
using ApiAuth.PostgreSql.Sut;

namespace ApiAuth.PostgreSql.Nunit;

public class UnitTest1 : TestBase
{
    [Test]
    public async Task Test1()
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
        var result = await Sut.CreateClient().GetFromJsonAsync<Blog[]>("blogs");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            new
            {
                Url = "https://blog.photogrammer.net/"
            }
        });

        Sut.AssertDatabase(context =>
        {
            context.Blogs.Should().BeEquivalentTo(new[]
            {
                new
                {
                    Url = "https://blog.photogrammer.net/"
                }
            });
        });
    }
}
