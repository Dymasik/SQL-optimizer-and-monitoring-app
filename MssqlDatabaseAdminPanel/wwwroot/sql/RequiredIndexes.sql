SELECT TOP 100
       TableName = statement,
       [EqualityUsage] = equality_columns,
       [InequalityUsage] = inequality_columns,
       [Include Cloumns] = included_columns,
	   [Total Cost] = ROUND(avg_total_user_cost * avg_user_impact * (user_seeks + user_scans),0)
  FROM sys.dm_db_missing_index_groups g
  INNER JOIN sys.dm_db_missing_index_group_stats s ON s.group_handle = g.index_group_handle
  INNER JOIN sys.dm_db_missing_index_details d ON d.index_handle = g.index_handle
  WHERE database_id = DB_ID()
  ORDER BY [Total Cost] DESC