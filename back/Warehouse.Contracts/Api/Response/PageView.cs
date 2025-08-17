namespace Warehouse.Contracts.Api.Response;

/// <summary>
/// Класс для пагинации и работы с ней
/// </summary>
public class PageView
{
    /// <summary>
    /// Page number, first page is 1, not 0
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Page size, count elements in one page
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// Total count elements
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// Total count pages
    /// </summary>
    public long TotalPages { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    public PageView(int pageNumber, int pageSize, long totalCount)
    {
        if (pageNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageNumber));
        }
        if (pageSize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize));
        }
        if (totalCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalCount));
        }

        Page = pageNumber;
        Size = pageSize;

        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling((float)totalCount / Size);
    }


    /// <summary>
    /// Перевод пагинации в take
    /// </summary>
    public int GetTake()
    {
        return Size;
    }

    /// <summary>
    /// Перевод пагинации в skip
    /// </summary>
    public int GetSkip()
    {
        return (Page - 1) * Size;
    }
}
