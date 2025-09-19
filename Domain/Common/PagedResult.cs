using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int CurrentPageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int) Math.Ceiling((double) TotalCount / PageSize);
        public PagedResult() { }
        public PagedResult(IReadOnlyList<T> items, int totalCount, int currentPageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            CurrentPageNumber = currentPageNumber;
            PageSize = pageSize;
        }
    }
}
