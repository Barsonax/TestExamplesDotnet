using Api.PostgreSql.Sut;
using Api.PostgreSql.Xunit.TestSetup;
using Xunit.Abstractions;

namespace Api.PostgreSql.Xunit;

public class ApiPostgreSqlTests : XunitContextBase
{
    private readonly ApiPostgreSqlSut _sut;

    public ApiPostgreSqlTests(ApiPostgreSqlSut sut, ITestOutputHelper output) : base(output)
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

public class ApiPostgreSqlTests2 : XunitContextBase
{
    private readonly ApiPostgreSqlSut _sut;

    public ApiPostgreSqlTests2(ApiPostgreSqlSut sut, ITestOutputHelper output) : base(output)
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

public class ApiPostgreSqlTests3 : XunitContextBase
{
    private readonly ApiPostgreSqlSut _sut;

    public ApiPostgreSqlTests3(ApiPostgreSqlSut sut, ITestOutputHelper output) : base(output)
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

public class ApiPostgreSqlTests4 : XunitContextBase
{
    private readonly ApiPostgreSqlSut _sut;

    public ApiPostgreSqlTests4(ApiPostgreSqlSut sut, ITestOutputHelper output) : base(output)
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

public class ApiPostgreSqlTests5 : XunitContextBase
{
    private readonly ApiPostgreSqlSut _sut;

    public ApiPostgreSqlTests5(ApiPostgreSqlSut sut, ITestOutputHelper output) : base(output)
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




