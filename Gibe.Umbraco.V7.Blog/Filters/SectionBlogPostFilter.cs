using Examine.SearchCriteria;

namespace Gibe.Umbraco.Blog.Filters
{
	public class SectionBlogPostFilter : IBlogPostFilter
	{
		public int SectionNodeId { get; set; }

		public SectionBlogPostFilter(int sectionNodeId)
		{
			SectionNodeId = sectionNodeId;
		}

		public IBooleanOperation GetCriteria(IQuery query)
		{
			return query.Field("path", SectionNodeId.ToString());
		}
	}
}