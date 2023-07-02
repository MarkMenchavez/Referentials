namespace Referentials.ViewModels;

/// <summary>
/// A paged collection of items.
/// </summary>
/// <typeparam name="T">The type of the items.</typeparam>
public class Connection<T>
{
    public Connection() => this.Items = new List<T>();

    /// <summary>
    /// Gets or sets the total count of items.
    /// </summary>
    /// <example>100</example>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the page information.
    /// </summary>
    public PageInfo PageInfo { get; set; } = default!;

    /// <summary>
    /// Gets the items.
    /// </summary>
#pragma warning disable CA1002 // Do not expose generic lists
    public List<T> Items { get; }
#pragma warning restore CA1002 // Do not expose generic lists
}
