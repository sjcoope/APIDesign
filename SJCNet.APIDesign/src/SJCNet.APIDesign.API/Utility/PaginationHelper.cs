using System;
using System.Collections;
using Newtonsoft.Json;

namespace SJCNet.APIDesign.API.Utility
{
    public class PaginationHelper
    {
        private readonly int _recordCount;
        private readonly int _page;

        // Would usually be stored in config.
        private const int _maxPageSize = 10;

        public int SkipCount { get; internal set; }

        public int PageSize { get; internal set; }
        public bool PaginationInUse { get; internal set; }

        public PaginationHelper(int recordCount, int pageSize, int page)
        {
            this.PaginationInUse = (pageSize != 0 && page != 0);
            if (!PaginationInUse) return;

            _recordCount = recordCount;
            _page = page;
            this.PageSize = pageSize;
            this.SkipCount = (PageSize * (page - 1));

            // Ensure pagesize isn't out of range
            if (pageSize > _maxPageSize)
            {
                pageSize = _maxPageSize;
            }
        }

        public string GetInfo()
        {
            var totalPages = (int)Math.Ceiling((double)_recordCount / this.PageSize);

            var paginationHeader = new
            {
                currentPage = _page,
                pageSize = this.PageSize,
                totalCount = _recordCount,
                totalPages = totalPages,
            };

            return JsonConvert.SerializeObject(paginationHeader);
        }
    }
}