namespace Application.Shared.Grid;

public class GridDataSource<T>
{
	public static string DataSourceQuery(GridOptions options, string query, string orderBy, string condition)
	{
		try
		{
			query = query.Replace(';', ' ');
			string orderby = "";
			string sqlQuery = query;
			if (options != null)
			{
				if (options.pageSize > 0)
				{
					sqlQuery = GridDataBuilder<T>.Query(options, query, orderBy, condition);
				}
				else
				{
					if (orderBy != "")
					{
						if (orderBy.ToLower().Contains("asc") || orderBy.ToLower().Contains("desc"))
						{
							orderby = string.Format(" order by {0}", orderBy);
						}
						else
						{
							orderby = string.Format(" order by {0} asc ", orderBy);
						}
					}
				}
			}
			else
			{
				if (orderBy != "")
				{
					if (orderBy.ToLower().Contains("asc") || orderBy.ToLower().Contains("desc"))
					{
						orderby = string.Format(" order by {0}", orderBy);
					}
					else
					{
						orderby = string.Format(" order by {0} asc ", orderBy);
					}
				}
			}

			if (!string.IsNullOrEmpty(condition))
			{
				condition = " WHERE " + condition;
			}

			var condition1 = "";
			if (options != null)
			{
				if (options.filter != null)
				{
					condition1 = GridDataBuilder<T>.FilterCondition(options.filter).Trim();
				}
			}
			if (!string.IsNullOrEmpty(condition1))
			{
				if (!string.IsNullOrEmpty(condition))
				{
					condition += " And " + condition1;
				}
				else
				{
					condition = " WHERE " + condition1;
				}
			}
			sqlQuery = "SELECT * FROM (" + sqlQuery + " ) As tbl " + condition;

			return sqlQuery + orderby;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

}
