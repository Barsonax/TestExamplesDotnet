using CosmosdbApi.Nunit.TestSetup;
using CosmosdbApi.Sut;

namespace CosmosdbApi.Nunit;

public class CosmosdbApiTests : TestBase
{
    [Test]
    public async Task GetBlogs_ShouldReturnExpectedBlogs()
    {
        //Arrange
        await Sut.SeedDataAsync(async database =>
        {
            var container = database.GetContainer("Blogs");
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
