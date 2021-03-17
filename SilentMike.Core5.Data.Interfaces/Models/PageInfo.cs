using SilentMike.Core5.Data.Interfaces.Interfaces;

namespace SilentMike.Core5.Data.Interfaces.Models
{
    public class PageInfo : IPageInfo
    {
        public int ItemsPerPage { get; set; }
        public int CurrentPageNumber { get; set; }
        public string OrderByPropertyName { get; set; }
        public bool IsDescending { get; set; }
    }
}
