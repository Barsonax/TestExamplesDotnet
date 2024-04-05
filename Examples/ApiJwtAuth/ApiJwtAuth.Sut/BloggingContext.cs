using Microsoft.EntityFrameworkCore;

namespace ApiJwtAuth.Sut;

public class BloggingContext : DbContext
{
    private readonly string? _connectionString;
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<Post> Posts => Set<Post>();

    public BloggingContext() { }

    public BloggingContext(IConfiguration configuration)
    {
        _connectionString = configuration["DbConnectionString"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public required string Url { get; set; }

    public List<Post>? Posts { get; } = new();
}

public class Post
{
    public int PostId { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }

    public required int BlogId { get; set; }
    public Blog? Blog { get; set; }
}
