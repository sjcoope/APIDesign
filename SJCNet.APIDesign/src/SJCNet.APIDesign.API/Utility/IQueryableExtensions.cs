using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace SJCNet.APIDesign.API.Utility
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sort)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (sort == null)
            {
                return source;
            }

            var sortExpression = new StringBuilder();
            foreach(var sortOption in sort.Split(','))
            {
                if(sortOption.StartsWith("-"))
                {
                    sortExpression.Append($"{sortOption.Remove(0, 1)} decending,");
                }
                else
                {
                    sortExpression.Append($"{sortOption},");
                }
            }

            if(sortExpression.Length > 0)
            {
                var sortString = sortExpression.ToString().Remove(sortExpression.Length - 1); // Remove last comma
                source = source.OrderBy(sortString);
            }

            return source;
        }
    }
}
