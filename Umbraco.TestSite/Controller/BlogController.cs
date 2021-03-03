using Gibe.Umbraco.Blog;
using System.Web.Mvc;
using Gibe.Umbraco.Blog.Sort;
using Umbraco.TestSite.Models;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace Umbraco.TestSite.Controller
{
	public class BlogController : RenderMvcController
	{
		private readonly IBlogService<BlogPostModel> _blogService;

		public BlogController(IBlogService<BlogPostModel> blogService)
		{
			_blogService = blogService;
		}

		public override ActionResult Index(RenderModel model)
		{
			var results = _blogService.GetPosts(10, 1, new DateSort());

			return View(new BlogViewModel(model.Content, results));
		}
	}
}