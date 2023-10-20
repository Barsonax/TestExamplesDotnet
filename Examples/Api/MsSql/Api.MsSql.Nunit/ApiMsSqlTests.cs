using Api.MsSql.Nunit.TestSetup;
using Api.MsSql.Sut;

namespace Api.MsSql.Nunit;

public class ApiMsSqlTests : TestBase
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
