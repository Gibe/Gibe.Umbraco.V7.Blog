﻿using System;
using System.Globalization;
using System.Web.Mvc;
using Examine.LuceneEngine;
using Gibe.Umbraco.Blog.Settings;
using Gibe.Umbraco.Blog.Wrappers;
using Lucene.Net.Documents;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Gibe.Umbraco.Blog
{
	public class UmbracoEvents : IApplicationEventHandler
	{
		private readonly IBlogSettings _blogSettings = DependencyResolver.Current.GetService<IBlogSettings>();

		public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
		}

		public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
		}

		/// <summary>
		/// Wire up events.
		/// </summary>
		/// <param name="umbracoApplication"></param>
		/// <param name="applicationContext"></param>
		public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			if (applicationContext.IsConfigured && applicationContext.DatabaseContext.IsDatabaseConfigured)
			{
				ContentService.Saving += ContentService_Saving;
				// TODO : Nicer than this
				new NewsIndex().GetIndexer().DocumentWriting += IndexerOnDocumentWriting;
			}
		}

		private void IndexerOnDocumentWriting(object sender, DocumentWritingEventArgs documentWritingEventArgs)
		{
			var document = documentWritingEventArgs.Document;
			if (document.Get("nodeTypeAlias") == _blogSettings.BlogPostDoctype)
			{
				var postDate = DateTime.ParseExact(document.Get("postDate").Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture);
				document.Add(new Field("postDateYear", postDate.Year.ToString("0000"), Field.Store.YES, Field.Index.NOT_ANALYZED));
				document.Add(new Field("postDateMonth", postDate.Month.ToString("00"), Field.Store.YES, Field.Index.NOT_ANALYZED));
				document.Add(new Field("postDateDay", postDate.Day.ToString("00"), Field.Store.YES, Field.Index.NOT_ANALYZED));

				var tags = document.Get("settingsNewsTags");
				if (tags != null)
				{
					foreach (var tag in tags.Split(','))
					{
						document.Add(new Field("tag", tag.ToLower(), Field.Store.YES, Field.Index.NOT_ANALYZED));
					}
				}

				var path = document.Get("path");
				if (path != null)
				{
					foreach (var id in path.Split(','))
					{
						document.Add(new Field("path", id, Field.Store.YES, Field.Index.NOT_ANALYZED));
					}
				}
			}
		}

		/// <summary>
		/// Update the URL alias for blog posts when they are saved
		/// </summary>
		void ContentService_Saving(IContentService sender, SaveEventArgs<IContent> e)
		{
			foreach (var entity in e.SavedEntities)
			{
				try
				{
					if (entity.ContentType.Alias.ToLower() == _blogSettings.BlogPostDoctype.ToLower() && entity.ParentId != -20)
					{
						// TODO : Move code to somewhere better
						var parentContent = sender.GetById(entity.ParentId);
						if (parentContent.HasPublishedVersion)
						{
							//if the date hasn't been set, default it to today
							var postDate = DateTime.Now.Date;
							var postDateString = entity.GetValue<string>("postDate");
							if (String.IsNullOrEmpty(postDateString))
							{
								entity.SetValue("postDate", postDate);
							}
						}
					}
				}
				catch (InvalidOperationException)
				{
					// This happens if you try to get ParentId during install of a package with content
				}
			}
		}

		private string GetUserName(int userId)
		{
			var userService = ApplicationContext.Current.Services.UserService;
			return userService.GetUserById(userId).Name;
		}
	}
}