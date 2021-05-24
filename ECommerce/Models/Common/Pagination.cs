using System.Collections.Generic;

namespace ECommerce.Models.Common
{
    public class Pagination<T>
    {
        public List<T> Items { get; set; }

        public int TotalRecords { get; set; }

        public int PageSize { get; set; } = 10;

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; } = 1;
    }
}