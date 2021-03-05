using System.Collections.Generic;
using Examine;
using Gibe.Umbraco.Blog.Models;

namespace Gibe.Umbraco.Blog
{
	public interface IBlogPostMapper<T> where T : IBlogPostModel
	{
		IEnumerable<T> ToBlogPosts(IEnumerable<SearchResult> searchResults);
		T ToBlogPost(SearchResult searchResult);
	}
}
