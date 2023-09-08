using Examine.SearchCriteria;

namespace Gibe.Umbraco.Blog.Sort
{
	public class DateSort : ISort
	{
		public bool Descending { get; set; }

		public DateSort(bool descending = true)
		{
			Descending = descending;
		}

		public IBooleanOperation GetCriteria(IBooleanOperation query)
		{
			if (Descending)
			{
				return query.And().OrderByDescending("postDate");
			}
			return query.And().OrderBy("postDate");
		}
	}
}
