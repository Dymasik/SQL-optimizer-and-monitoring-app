﻿SELECT TOP 10
		TableName = OBJECT_NAME(s.[object_id])
        ,IndexName = i.name
		,[Maintenance cost]  = (user_updates + system_updates)
INTO #TempMaintenanceCost
FROM   sys.dm_db_index_usage_stats s
INNER JOIN sys.indexes i ON  s.[object_id] = i.[object_id]
    AND s.index_id = i.index_id
WHERE s.database_id = DB_ID()
    AND OBJECTPROPERTY(s.[object_id], 'IsMsShipped') = 0
    AND (user_updates + system_updates) > 0 -- Only report on active rows.
    AND s.[object_id] = -999  -- Dummy value to get table structure.
;
-- Loop around all the databases on the server.
EXEC sp_MSForEachDB    'USE [?];
-- Table already exists.
INSERT INTO #TempMaintenanceCost
SELECT TOP 10
		TableName = OBJECT_NAME(s.[object_id])
        ,IndexName = i.name
		,[Maintenance cost]  = (user_updates + system_updates)
FROM   sys.dm_db_index_usage_stats s
INNER JOIN sys.indexes i ON  s.[object_id] = i.[object_id]
    AND s.index_id = i.index_id
WHERE s.database_id = DB_ID()
    AND i.name IS NOT NULL    -- Ignore HEAP indexes.
    AND OBJECTPROPERTY(s.[object_id], ''IsMsShipped'') = 0
    AND (user_updates + system_updates) > 0 -- Only report on active rows.
ORDER BY [Maintenance cost]  DESC;'
SELECT TOP 100 * FROM #TempMaintenanceCost
ORDER BY [Maintenance cost]  DESC
DROP TABLE #TempMaintenanceCost
