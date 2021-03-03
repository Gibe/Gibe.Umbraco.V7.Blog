using Gibe.Umbraco.Blog.Models;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Umbraco.TestSite.Models
{
	public class BlogPostModel : BlogPostBase
	{

		public BlogPostModel(IPublishedContent content) : base(content)
		{

		}
		public string Excerpt => Content.GetPropertyValue<string>("excerpt");
		public string PageTitle => Content.GetPropertyValue<string>("pageTitle");
		public IEnumerable<string> Categories => Content.GetPropertyValue<IEnumerable<string>>("categories");

		public BlogPostModel Next { get; set; }
		public BlogPostModel Previous { get; set; }
	}

	public class BlogSectionModel : IBlogPostSection
	{
	}
}