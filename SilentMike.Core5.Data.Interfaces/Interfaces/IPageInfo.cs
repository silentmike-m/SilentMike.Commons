namespace SilentMike.Core5.Data.Interfaces.Interfaces
{
    public interface IPageInfo
    {
         int ItemsPerPage { get; set; }
         int CurrentPageNumber { get; set; }
         string OrderByPropertyName { get; set; }
         bool IsDescending { get; set; }
    }
}
