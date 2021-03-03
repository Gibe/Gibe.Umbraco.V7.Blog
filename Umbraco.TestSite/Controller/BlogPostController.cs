using System.Linq;
using System.Web.Mvc;
using Gibe.Umbraco.Blog;
using Gibe.Umbraco.Blog.Filters;
using Gibe.Umbraco.Blog.Sort;
using Umbraco.TestSite.Models;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Umbraco.TestSite.Controller
{
	public class BlogPostController : RenderMvcController
	{

		private readonly IBlogService<BlogPostModel> _blogService;

		public BlogPostController(IBlogService<BlogPostModel> blogService)
		{
			_blogService = blogService;
		}

		public override ActionResult Index(RenderModel model)
		{
			var post = new BlogPostModel(model.Content);
			post.Next = _blogService.GetNextPost(post, Enumerable.Empty<IBlogPostFilter>(), new DateSort());
			post.Previous = _blogService.GetPreviousPost(post, Enumerable.Empty<IBlogPostFilter>(), new DateSort());
			return View(post);
		}
	}
}