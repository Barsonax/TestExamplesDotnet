using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor.PostgreSql.Sut.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BloggingContext _bloggingContext;
    public IEnumerable<Blog> Blogs => _bloggingContext.Blogs;

    public IndexModel(ILogger<IndexModel> logger, BloggingContext bloggingContext)
    {
        _logger = logger;
        _bloggingContext = bloggingContext;
    }

    public void OnGet()
    {
    }
}
