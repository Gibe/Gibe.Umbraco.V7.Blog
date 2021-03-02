using System.Collections.Generic;
using System.Linq;
using Examine;
using Gibe.Umbraco.Blog.Models;
using Gibe.Umbraco.Blog.Settings;
using Gibe.Umbraco.Blog.Wrappers;
using Gibe.UmbracoWrappers;

namespace Gibe.Umbraco.Blog
{
	public class BlogSections<T> : IBlogSections<T> where T : class
	{
		private readonly IBlogSettings _blogSettings;
		private readonly IUmbracoWrapper _umbracoWrapper;
		private readonly ISearchIndex _searchIndex;

		public BlogSections(IUmbracoWrapper umbracoWrapper, ISearchIndex searchIndex, IBlogSettings blogSettings)
		{
			_umbracoWrapper = umbracoWrapper;
			_searchIndex = searchIndex;
			_blogSettings = blogSettings;
		}

		public IEnumerable<T> All()
		{
			var results = SearchForBlogSections();
			return results.Select(r => Activator.Activate<T>(_umbracoWrapper.TypedContent(r.Id)));
		}
		
		private ISearchResults SearchForBlogSections()
		{
			var query = _searchIndex.CreateSearchCriteria().NodeTypeAlias(_blogSettings.BlogSectionDoctype).Compile();
			
			return _searchIndex.Search(query);
		}

	}
}
