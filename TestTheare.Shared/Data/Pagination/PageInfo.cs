using System.ComponentModel.DataAnnotations;

namespace TestTheare.Shared.Data.Pagination
{
    public class PageInfo
    {
        
        public int? PageNumber { get; set; }

        public int? PageLength { get; set; }

        public int TotalRecords { get; set; }

        public PageInfo() { }
        public PageInfo(int? pageNumber, int? pageLength) {
            PageNumber = pageNumber;
            PageLength = pageLength;
        }
        public static PageInfo Get(int? pageLength = null, int? pageNumber = null)
        {
            PageInfo pageInfo = null;

            if (pageLength != null)
            {
                pageInfo = new PageInfo()
                {
                    PageLength = pageLength.Value,
                    PageNumber = pageNumber ?? 1,
                };
            }
            return pageInfo;
        }
    }
}
