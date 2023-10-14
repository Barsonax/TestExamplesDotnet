namespace AwesomeApiTest.Nunit;

public class UnitTest1 : AwesomeApiTests
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
    }
}
