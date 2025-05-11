namespace TurboNotes.Web.Models;

public class PagingInfo
{
    public int TotalItems { get; init; }
    public int ItemsPerPage { get; init; }
    public int CurrentPage { get; init; }
    public int TotalPages => ItemsPerPage > 0 ? (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage) : 1;
}