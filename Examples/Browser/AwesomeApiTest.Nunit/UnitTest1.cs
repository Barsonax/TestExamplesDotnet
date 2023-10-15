using System.Text.RegularExpressions;
using AwesomeApiTest.Sut;

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
        await Page.GotoAsync($"{Sut.ServerAddress}");

        //Assert
        await Expect(Page).ToHaveTitleAsync(new Regex("Home page"));
    }
}
