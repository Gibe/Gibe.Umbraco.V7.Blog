﻿using System.Collections.Generic;
using Examine;
using Examine.SearchCriteria;
using Gibe.Umbraco.Blog.Filters;
using Gibe.Umbraco.Blog.Settings;
using Gibe.Umbraco.Blog.Sort;
using Gibe.Umbraco.Blog.Wrappers;

namespace Gibe.Umbraco.Blog
{
	public class BlogSearch : IBlogSearch
	{
		private readonly ISearchIndex _newsIndex;
		private readonly IBlogSettings _blogSettings;

		public BlogSearch(ISearchIndex newsIndex, IBlogSettings blogSettings)
		{
			_newsIndex = newsIndex;
			_blogSettings = blogSettings;
		}
		
		public ISearchResults Search(IBlogPostFilter filter, ISort sort)
		{
			return Search(new List<IBlogPostFilter> { filter }, sort);
		}
		
		public ISearchResults Search(IEnumerable<IBlogPostFilter> filters, ISort sort)
		{
			return SearchForBlogPosts(GetSearchQuery(filters, sort));
		}
		
		private ISearchCriteria GetSearchQuery(IEnumerable<IBlogPostFilter> filters, ISort sort)
		{
			return sort.GetCriteria(GetQuery(filters)).Compile();
		}

		private IBooleanOperation GetQuery(IEnumerable<IBlogPostFilter> filters)
		{
			var query = _newsIndex.CreateSearchCriteria().NodeTypeAlias(_blogSettings.BlogPostDoctype);
			foreach (var filter in filters)
			{
				query = filter.GetCriteria(query.And());
			}
			return query;
		}

		private ISearchResults SearchForBlogPosts(ISearchCriteria query)
		{
			return _newsIndex.Search(query);
		}
	}
	
}
