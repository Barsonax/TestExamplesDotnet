namespace AwesomeApiTest.Xunit;

public class UnitTest1
{
    private readonly AwesomeApiTestSut _sut;

    public UnitTest1(AwesomeApiTestSut sut)
    {
        _sut = sut;
    }

    [Fact]
    public async Task Test1()
    {
        //Arrange
        _sut.SeedData(context =>
        {
            context.Blogs.Add(new Blog
            {
                Url = "https://blog.photogrammer.net/"
            });
        });

        //Act
        var result = await _sut.CreateClient().GetFromJsonAsync<Blog[]>("blogs");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            new
            {
                Url = "https://blog.photogrammer.net/"
            }
        });

        _sut.AssertDatabase(context =>
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
