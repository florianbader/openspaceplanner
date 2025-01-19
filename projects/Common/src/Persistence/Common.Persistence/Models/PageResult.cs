namespace RioScaffolding.OpenSpacePlanner.Common.Persistence.Models;

public record PageResult(int Size, int TotalItems, int TotalPages, int CurrentPage)
{
    /// <summary>
    /// Gets the size of the paginated result.
    /// </summary>
    public int Size { get; init; } = Size;

    /// <summary>
    /// Gets the total items of the paginated result.
    /// </summary>
    public int TotalItems { get; init; } = TotalItems;

    /// <summary>
    /// Gets the total pages of the paginated result.
    /// </summary>
    public int TotalPages { get; init; } = TotalPages;

    /// <summary>
    /// Gets the current page of the paginated result.
    /// </summary>
    public int CurrentPage { get; init; } = CurrentPage;
}
