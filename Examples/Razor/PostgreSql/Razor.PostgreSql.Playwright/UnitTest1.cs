using Microsoft.Playwright;
using Razor.PostgreSql.Playwright.TestSetup;
using Razor.PostgreSql.Sut;

namespace Razor.PostgreSql.Playwright;

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
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "https://blog.photogrammer.net/" }))
            .ToBeVisibleAsync();
    }
}
