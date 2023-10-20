using Api.PostgreSql.Nunit.TestSetup;
using Api.PostgreSql.Sut;

namespace Api.PostgreSql.Nunit;

public class ApiPostgreSqlTests : TestBase
{
    [Test]
    public async Task GetBlogs_ShouldReturnExpectedBlogs()
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
    }
}
