namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Models;

public class ItemsResult<T>(IEnumerable<T> items, PageResult? pages)
{
    public ItemsResult(IEnumerable<T> items)
        : this(items, null) { }

    /// <summary>
    /// Gets the items.
    /// </summary>
    public IEnumerable<T> Items { get; } = items;

    /// <summary>
    /// Gets the pagination result.
    /// </summary>
    public PageResult? Pages { get; } = pages;
}
