using Examine.SearchCriteria;

namespace Gibe.Umbraco.Blog.Filters
{
	public class DateBlogPostFilter : IBlogPostFilter
	{
		public int Year { get; }
		public int? Month { get; }
		public int? Day { get; }

		public DateBlogPostFilter(int year, int? month, int? day)
		{
			Year = year;
			Month = month;
			Day = day;
		}
		
		public IBooleanOperation GetCriteria(IQuery query)
		{
			var output = query.Field("postDateYear", Year.ToString("00"));

			if (Month.HasValue)
			{
				output = output.And().Field("postDateMonth", Month.Value.ToString("00"));
			}
			if (Day.HasValue)
			{
				output = output.And().Field("postDateDay", Day.Value.ToString("00"));
			}
			return output;
		}
	}
}