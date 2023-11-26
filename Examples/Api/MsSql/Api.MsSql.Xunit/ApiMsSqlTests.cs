using Api.MsSql.Sut;
using Api.MsSql.Xunit.TestSetup;
using Xunit.Abstractions;

namespace Api.MsSql.Xunit;

public class ApiMsSqlTests : XunitContextBase
{
    private readonly ApiMsSqlSut _sut;

    public ApiMsSqlTests(ApiMsSqlSut sut, ITestOutputHelper output) : base(output)
    {
        _sut = sut;
    }

    [Fact]
    public async Task GetBlogs_ShouldReturnExpectedBlogs()
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
    }
}
