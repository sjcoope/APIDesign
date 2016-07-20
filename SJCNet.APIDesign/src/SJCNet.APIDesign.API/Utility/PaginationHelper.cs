using System;
using System.Collections;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace SJCNet.APIDesign.API.Utility
{
    public class PaginationHelper
    {
        private readonly int _recordCount;
        private readonly int _page;

        // Would usually be stored in config.
        private const int _maxPageSize = 10;

        public PaginationHelper(int recordCount, int pageSize, int currentPage)
        {
            this.PaginationIsActive = (pageSize != 0 && currentPage != 0);
            if (!PaginationIsActive) return;

            this.RecordCount = recordCount;
            this.CurrentPage = currentPage;

            // Ensure pagesize isn't out of range
            this.PageSize = (pageSize > _maxPageSize) ? _maxPageSize : pageSize;

            // Generate the pagination header info.
            SerializePaginationInfo();
        }

        public int SkipCount => (PageSize * (CurrentPage - 1));
            
        public int PageSize { get; internal set; }

        public bool PaginationIsActive { get; internal set; }

        private int RecordCount { get; set; }

        private int CurrentPage { get; set; }

        public string PaginationInfoJson { get; private set; }

        private void SerializePaginationInfo()
        {
            var totalPages = (int)Math.Ceiling((double)this.RecordCount / this.PageSize);

            var paginationHeader = new
            {
                currentPage = this.CurrentPage,
                pageSize = this.PageSize,
                recordCount = this.RecordCount,
                totalPages = totalPages,
            };

            this.PaginationInfoJson = JsonConvert.SerializeObject(paginationHeader);
        }
    }
}