using Microsoft.Playwright;
using VueAuth.Playwright.TestSetup;
using VueAuth.Sut;

namespace VueAuth.Playwright;

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
        await Page.GotoAsync(Sut.ServerAddress);
        var state = await Page.Context.StorageStateAsync();

        //Assert
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "https://blog.photogrammer.net/" }))
            .ToBeVisibleAsync();
    }
}
