namespace Warehouse.Contracts.Api.Response;

/// <summary>
/// Класс для пагинации и работы с ней
/// </summary>
public class PageView
{
    /// <summary>
    /// Page number, first page is 1, not 0
    /// </summary>
    public int PageNumber { get; set; }
    /// <summary>
    /// Page size, count elements in one page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total count elements
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// Total count pages
    /// </summary>
    public long TotalPages { get; set; }

    /// <summary>
    /// 
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

        PageNumber = pageNumber;
        PageSize = pageSize;

        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling((float)totalCount / PageSize);
    }


    /// <summary>
    /// Перевод пагинации в take
    /// </summary>
    public int GetTake()
    {
        return PageSize;
    }

    /// <summary>
    /// Перевод пагинации в skip
    /// </summary>
    public int GetSkip()
    {
        return (PageNumber - 1) * PageSize;
    }
}
