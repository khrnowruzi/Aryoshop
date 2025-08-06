namespace Application.RequestHelpers;

public class Pagination<TResult>(
    int pageIndex,
    int pageSize,
    int count,
    IReadOnlyList<TResult> data
    )
{
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public int Count { get; set; } = count;
    public IReadOnlyList<TResult> Data { get; set; } = data;
}
