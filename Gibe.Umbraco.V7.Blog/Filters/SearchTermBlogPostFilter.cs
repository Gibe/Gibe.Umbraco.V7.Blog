using Examine.SearchCriteria;

namespace Gibe.Umbraco.Blog.Filters
{
	public class SearchTermBlogPostFilter : IBlogPostFilter
	{
		public string SearchTerm { get; }

		public SearchTermBlogPostFilter(string searchTerm)
		{
			SearchTerm = searchTerm;
		}

		public IBooleanOperation GetCriteria(IQuery query)
		{
			return query.Field("bodyText", SearchTerm);
		}
	}
}
