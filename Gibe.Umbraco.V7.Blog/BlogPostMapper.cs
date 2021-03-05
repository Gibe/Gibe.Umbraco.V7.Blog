using System.Collections.Generic;
using System.Linq;
using Examine;
using Gibe.Umbraco.Blog.Models;
using Gibe.UmbracoWrappers;
using Umbraco.Core.Models;

namespace Gibe.Umbraco.Blog
{
	public class BlogPostMapper<T> : IBlogPostMapper<T> where T : class, IBlogPostModel
	{
		private readonly IUmbracoWrapper _umbracoWrapper;

		public BlogPostMapper(IUmbracoWrapper umbracoWrapper)
		{
			_umbracoWrapper = umbracoWrapper;
		}

		public IEnumerable<T> ToBlogPosts(IEnumerable<SearchResult> searchResults)
			=> ToBlogPosts(searchResults.Select(r => _umbracoWrapper.TypedContent(r.Id)));

		public T ToBlogPost(SearchResult searchResult) =>
			ToBlogPost(_umbracoWrapper.TypedContent(searchResult.Id));
		
		private T ToBlogPost(IPublishedContent content)
			=> Activator.Activate<T>(content);
		
		private IEnumerable<T> ToBlogPosts(IEnumerable<IPublishedContent> content)
			=> content.Select(ToBlogPost);
	}
}
