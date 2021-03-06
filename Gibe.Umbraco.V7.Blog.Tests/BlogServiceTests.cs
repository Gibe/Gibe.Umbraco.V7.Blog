﻿using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Gibe.Pager.Interfaces;
using Gibe.Umbraco.Blog.Filters;
using Gibe.Umbraco.Blog.Models;
using Gibe.UmbracoWrappers;
using NUnit.Framework;
using Moq;
using Umbraco.Core.Models;

namespace Gibe.Umbraco.Blog.Tests
{
	[TestFixture]
	public class BlogServiceTests
	{
		private IPagerService PagerService()
		{
			var pagerService = new Mock<IPagerService>();
			return pagerService.Object;
		}

		private IBlogPostMapper<BlogModel> BlogPostMapper(int searchResultsCount)
		{
			var blogPostMapper = new Mock<IBlogPostMapper<BlogModel>>();
			blogPostMapper.Setup(m => m.ToBlogPosts(It.IsAny<IEnumerable<SearchResult>>()))
				.Returns(MappedSearchResults(searchResultsCount));

			return blogPostMapper.Object;
		}

		private IEnumerable<BlogModel> MappedSearchResults(int searchResultsCount)
		{
			var mappedSearchResults = new List<BlogModel>();
			for(var i=0 ; i<searchResultsCount ; i++)
			{
				mappedSearchResults.Add(new BlogModel());
			}
			return mappedSearchResults;
		}

		[Test]
		public void GetRelatedPosts_Uses_Correct_Filters()
		{
			var searchResults = GetSearchResults();
			var blogSearch = new FakeBlogSearch(searchResults);
			var blogService = new BlogService<BlogModel>(PagerService(), blogSearch, BlogPostMapper(searchResults.Count()));
			var testPost = new BlogModel
			{
				Tags = new List<string>
				{
					"test",
					"post",
					"1"
				}
			};
			var relatedPosts = blogService.GetRelatedPosts(testPost, 3);

			Assert.That(blogSearch.LastUsedFilters.Any(t => typeof(AtLeastOneMatchingTagFilter) == t.GetType()), Is.EqualTo(true));

			var tagsFilter = blogSearch.LastUsedFilters.First(t => typeof(AtLeastOneMatchingTagFilter) == t.GetType()) as AtLeastOneMatchingTagFilter;
			foreach (var tag in testPost.Tags)
			{
				Assert.That(tagsFilter.Tags.Contains(tag), Is.True);
			}

			Assert.That(relatedPosts.Count(), Is.EqualTo(3));
		}

		private FakeSearchResults GetSearchResults()
		{
			return new FakeSearchResults(new List<SearchResult>
			{
				SearchResult(1),
				SearchResult(2),
				SearchResult(3)
			});
		}

		private SearchResult SearchResult(int id)
		{
			return new SearchResult
			{
				Id = id
			};
		}

		private IEnumerable<BlogModel> GetBlogPosts()
		{
			return new List<BlogModel>
			{
				CreatePost(1, new []
				{
					"test"
				}),
				CreatePost(2, new []
				{
					"test",
					"post"
				}),
				CreatePost(3, new []
				{
					"Test"
				})
			};
		}

		private BlogModel CreatePost(int id, string[] tags)
		{
			return new BlogModel
			{
				Id = id,
				HasTags = true,
				Tags = tags
			};
		}

		public static IPublishedContent Content(int id, string docType = "blogPost", string name = null)
		{
			var content = new Mock<IPublishedContent>();
			content.Setup(c => c.DocumentTypeAlias).Returns(docType);
			content.Setup(c => c.Name).Returns(name ?? docType);
			content.Setup(c => c.Id).Returns(id);

			return content.Object;
		}


		public static IUmbracoWrapper UmbracoWrapper(params IPublishedContent[] content)
		{
			var umbraco = new Mock<IUmbracoWrapper>();
			umbraco.Setup(u => u.TypedContentAtRoot()).Returns(content);

			foreach (var c in content)
			{
				umbraco.Setup(u => u.TypedContent(c.Id)).Returns(c);
			}

			return umbraco.Object;
		}
	}

	public class BlogModel : IBlogPostModel
	{
		public int Id { get; set; }
		public string Url { get; }
		public DateTime PostDate { get; }
		public IEnumerable<string> Tags { get; set; }
		public bool HasTags { get; set; }
	}
}
