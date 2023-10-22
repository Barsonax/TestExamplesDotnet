using Microsoft.Playwright;
using Vue.Backend.Sut;
using Vue.Playwright.TestSetup;

namespace Vue.Playwright;

public class VueTests : TestBase
{
    [Test]
    public async Task Open_Index_Page_Contains_Link_To_Photogrammer_Blog()
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
