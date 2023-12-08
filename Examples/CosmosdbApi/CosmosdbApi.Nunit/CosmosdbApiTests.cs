using CosmosdbApi.Nunit.TestSetup;

namespace CosmosdbApi.Nunit;

public class CosmosdbApiTests : TestBase
{
    [Test]
    public async Task GetBlogs_ShouldReturnExpectedBlogs()
    {
        //Arrange
        await Sut.SeedDataAsync(async context =>
        {
            var container = context.GetContainer("CosmosdbApi", "Blogs");
            await container.UpsertItemAsync(new Blog
            {
                Id = "1",
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
                Id = "1",
                Url = "https://blog.photogrammer.net/"
            }
        });
    }

    [Test]
    public void EmptyTest_ShouldPass()
    {

    }
}
