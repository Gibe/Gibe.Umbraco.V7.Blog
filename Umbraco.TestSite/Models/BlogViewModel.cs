using Gibe.Pager.Models;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Umbraco.TestSite.Models
{
	public class BlogViewModel : PublishedContentModel
	{
		public BlogViewModel(IPublishedContent content, PageQueryResultModel<BlogPostModel> posts) : base(content)
		{
			BlogPosts = posts;
		}

		public PageQueryResultModel<BlogPostModel> BlogPosts { get; }
	}
}