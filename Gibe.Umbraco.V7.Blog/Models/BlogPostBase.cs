﻿using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Gibe.Umbraco.Blog.Models
{
	public abstract class BlogPostBase : PublishedContentModel, IBlogPostModel
	{
		protected BlogPostBase(IPublishedContent content) : base(content)
		{
		}

		public DateTime PostDate => Content.GetPropertyValue<DateTime>("postDate");
		public IEnumerable<string> Tags => Content.GetPropertyValue<IEnumerable<string>>("settingsNewsTags");
		public bool HasTags => Tags != null && Tags.Any();
	}
}