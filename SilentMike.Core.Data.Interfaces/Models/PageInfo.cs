namespace SilentMike.Core.Data.Interfaces.Models
{
    public class PageInfo
    {
        public int ItemsPerPage { get; set; }
        public int CurrentPageNumber { get; set; }
        public string OrderByPropertyName { get; set; }
        public bool IsDescending { get; set; }
    }
}
