using Examine.SearchCriteria;
using Gibe.Umbraco.Blog.Utilities;

namespace Gibe.Umbraco.Blog.Filters
{
	public class AuthorBlogPostFilter : IBlogPostFilter
	{
		public string Author { get; set; }

		public AuthorBlogPostFilter() { }

		public AuthorBlogPostFilter(string author)
		{
			Author = author;
		}

		public IBooleanOperation GetCriteria(IQuery query)
		{
			return query.Field("postAuthorName", new ExactPhraseExamineValue(Author.ToLower()));
		}
	}
}