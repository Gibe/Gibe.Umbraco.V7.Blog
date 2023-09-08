using Examine.SearchCriteria;
using Gibe.Umbraco.Blog.Utilities;

namespace Gibe.Umbraco.Blog.Filters
{
	public class CategoryBlogPostFilter : IBlogPostFilter
	{
		public string CategoryName { get; set; }

		public CategoryBlogPostFilter() { }

		public CategoryBlogPostFilter(string categoryName)
		{
			CategoryName = categoryName;
		}

		public IBooleanOperation GetCriteria(IQuery query)
		{
			return query.Field("categoryName", new ExactPhraseExamineValue(CategoryName.ToLower()));
		}
	}
}
