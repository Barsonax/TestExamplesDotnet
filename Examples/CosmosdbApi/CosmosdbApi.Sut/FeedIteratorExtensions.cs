using Microsoft.Azure.Cosmos;

namespace CosmosdbApi.Sut;

public static class FeedIteratorExtensions
{
    public static async IAsyncEnumerable<T> GetAllAsync<T>(this FeedIterator<T> iterator)
    {
        while (iterator.HasMoreResults)
            foreach (var item in await iterator.ReadNextAsync().ConfigureAwait(false))
                yield return item;
    }
}
