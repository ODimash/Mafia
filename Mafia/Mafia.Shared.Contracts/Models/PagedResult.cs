namespace Mafia.Shared.Contracts.Models;

public class PagedResult<TData>
{
	public required int Page { get; set; }
	public required int PageSize { get; set; }
	public required int TotalPages { get; set; }
	public required int TotalRecords { get; set; }
	public required List<TData> Data { get; set; }
}

public class PageFilter
{
	public PageFilter()
	{
	}
	
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}
