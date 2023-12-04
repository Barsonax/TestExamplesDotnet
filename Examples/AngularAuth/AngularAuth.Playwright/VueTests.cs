using System.Text.RegularExpressions;
using AngularAuth.Playwright.TestSetup;
using AngularAuth.Sut;
using Microsoft.Playwright;

namespace AngularAuth.Playwright;

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
        
        await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("Profile", RegexOptions.IgnoreCase) }).ClickAsync();

        //Assert
        await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "https://blog.photogrammer.net/" }))
            .ToBeVisibleAsync();
    }
}
