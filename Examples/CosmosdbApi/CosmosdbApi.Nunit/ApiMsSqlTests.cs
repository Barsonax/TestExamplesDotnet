using Api.MsSql.Nunit.TestSetup;

namespace CosmosdbApi;

public class CosmosdbApi : TestBase
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

    [Test]
    public void EmptyTest_ShouldPass()
    {

    }
}
