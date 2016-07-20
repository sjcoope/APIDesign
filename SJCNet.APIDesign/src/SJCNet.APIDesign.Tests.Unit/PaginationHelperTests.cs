using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SJCNet.APIDesign.API.Utility;
using Xunit;

namespace SJCNet.APIDesign.Tests.Unit
{
    public class PaginationHelperTests
    {
        [Fact]
        public void Can_Configure_Paging_With_Success()
        {
            // Arrange - Build the pagination settings
            var recordCount = 100;
            var currentPage = 1;
            var pageSize = 10;
            var expectedTotalPages = 10;

            // Act
            var helper = new PaginationHelper(recordCount, pageSize, currentPage);

            // Assert - Check properties
            Assert.Equal(helper.PaginationIsActive, true);
            Assert.Equal(helper.SkipCount, 0);
            Assert.Equal(helper.PageSize, pageSize);

            // Assert - Check serialized JSON info
            dynamic paginationInfo = JObject.Parse(helper.PaginationInfoJson);
            Assert.NotNull(paginationInfo);

            Assert.NotNull(paginationInfo.currentPage);
            Assert.True(currentPage == (int)paginationInfo.currentPage, "Current page value is not expected");

            Assert.NotNull(paginationInfo.pageSize);
            Assert.True(pageSize == (int)paginationInfo.pageSize, "Page size value is not expected");

            Assert.NotNull(paginationInfo.recordCount);
            Assert.True(recordCount == (int)paginationInfo.recordCount, "Record count value is not expected");

            Assert.NotNull(paginationInfo.totalPages);
            Assert.True(expectedTotalPages == (int)paginationInfo.totalPages, "Total pages value is not expected");
        }

        // Additional Tests:
        // - Test when pagination shouldn't be active
        // - Test when pagesize is greater than max
        // - Test other pages of data
    }
}
