using Examine.SearchCriteria;
using Gibe.Umbraco.Blog.Utilities;

namespace Gibe.Umbraco.Blog.Filters
{
	public class TagBlogPostFilter : IBlogPostFilter
	{
		public string Tag { get; set; }

		public TagBlogPostFilter(string tag)
		{
			Tag = tag;
		}
		
		public IBooleanOperation GetCriteria(IQuery query)
		{
			return query.Field("tag", new ExactPhraseExamineValue(Tag.ToLower()));
		}
	}
}