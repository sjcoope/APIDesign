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

            // Assert - Check header key
            Assert.True(helper.Header.Key == "X-Pagination", "Header key is not as expected");

            // Assert - Check header value
            dynamic headerValue = JObject.Parse(helper.Header.Value);
            Assert.NotNull(headerValue);

            Assert.NotNull(headerValue.currentPage);
            Assert.True(currentPage == (int)headerValue.currentPage, "CurrentPage value in the header is not expected");

            Assert.NotNull(headerValue.pageSize);
            Assert.Equal(pageSize, (int)headerValue.pageSize);

            Assert.NotNull(headerValue.recordCount);
            Assert.Equal(recordCount, (int)headerValue.recordCount);

            Assert.NotNull(headerValue.totalPages);
            Assert.Equal(expectedTotalPages, (int)headerValue.totalPages);
        }

        // Additional Tests:
        // - Test when pagination shouldn't be active
        // - Test when pagesize is greater than max
        // - Test other pages of data
    }
}
