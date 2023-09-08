using System.Collections.Generic;
using System.Linq;
using Examine.SearchCriteria;

namespace Gibe.Umbraco.Blog.Filters
{
	public class AtLeastOneMatchingTagFilter : IBlogPostFilter
	{
		public IEnumerable<string> Tags { get; }

		public AtLeastOneMatchingTagFilter() { }

		public AtLeastOneMatchingTagFilter(IEnumerable<string> tags)
		{
			Tags = tags;
		}
		
		public IBooleanOperation GetCriteria(IQuery query)
		{
			return query.GroupedOr(new []{"tag"}, Tags.ToArray());
		}
	}
}