namespace Application.Specification.Products;

public class ProductSpecParams
{
    public const int MaxPageSize = 50;

    public int PageIndex { get; set; } = 1;

    private int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;

        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    private List<string> _brands = [];
    public List<string> Brands
    {
        get => _brands;
        set
        {
            _brands = [.. value.SelectMany(x =>
                x.Split(',', StringSplitOptions.RemoveEmptyEntries))];
        }
    }

    private List<string> _models = [];
    public List<string> Models
    {
        get => _models;
        set
        {
            _models = [.. value.SelectMany(x =>
                x.Split(',', StringSplitOptions.RemoveEmptyEntries))];
        }
    }

    public string? Sort { get; set; }

    private string? _search;
    public string? Search
    {
        get => _search ?? "";
        set => _search = value?.ToLower();
    }

}
