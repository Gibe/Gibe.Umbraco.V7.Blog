using Gibe.Pager.Interfaces;
using Gibe.Pager.Services;
using Gibe.UmbracoWrappers;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using Site.App_Start;
using System;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Umbraco.TestSite.Models;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]
namespace Site.App_Start
{
	public static class NinjectWebCommon
	{
		private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

		/// <summary>
		///   Starts the application
		/// </summary>
		public static void Start()
		{
			DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
			DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
			Bootstrapper.Initialize(CreateKernel);
		}

		/// <summary>
		///   Stops the application.
		/// </summary>
		public static void Stop()
		{
			Bootstrapper.ShutDown();
		}

		/// <summary>
		///   Creates the kernel that will manage your application.
		/// </summary>
		/// <returns>The created kernel.</returns>
		private static IKernel CreateKernel()
		{
			var kernel = new StandardKernel();
			try
			{
				kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
				kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
								
				RegisterServices(kernel);

				return kernel;
			}
			catch
			{
				kernel.Dispose();
				throw;
			}
		}

		/// <summary>
		///   Load your modules or register your services here!
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		private static void RegisterServices(IKernel kernel)
		{
			kernel.Bind<IUmbracoWrapper>().To<DefaultUmbracoWrapper>();
			kernel.Bind<IPagerService>().To<PagerService>();
			kernel.Load<Gibe.Umbraco.Blog.Ninject.GibeUmbracoBlogModule<BlogPostModel, BlogSectionModel>>();
		}
	}
}