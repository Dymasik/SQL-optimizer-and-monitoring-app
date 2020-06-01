DECLARE @database_name AS NVARCHAR(MAX) = DB_NAME()
DECLARE @index_rebuild_threshhold AS NUMERIC(5,2) = 25;
DECLARE @index_defrag_threshhold AS NUMERIC(5,2) = 12;
	   
SELECT IIF(d.avg_fragmentation_in_percent >= @index_rebuild_threshhold, N'ALTER INDEX [' + i.name + N'] ON [' + s.name + N'].['+ OBJECT_NAME(o.[object_id]) + N'] REBUILD' + case
                                                                                                    when p.data_space_id is not null then N' PARTITION = ' + convert(varchar(100),partition_number)
                                                                                                    else N''
                                                                                                    end + N' with(maxdop = 4,  SORT_IN_TEMPDB = on)', IIF(d.avg_fragmentation_in_percent < @index_rebuild_threshhold,
        N'ALTER INDEX [' + i.name + N'] ON [' + s.name + N'].[' + OBJECT_NAME(o.[object_id]) + N'] REORGANIZE' + case
                                                                                                    when p.data_space_id is not null then N' PARTITION = ' + convert(varchar(100),partition_number)
                                                                                                    else N''
                                                                                                    end,'')) AS [sql]
FROM sys.dm_db_index_physical_stats (DB_ID(), NULL, NULL, NULL, NULL) AS d
JOIN sys.objects AS o ON o.object_id = d.object_id
JOIN sys.schemas AS s ON s.schema_id = o.schema_id
JOIN sys.indexes AS i ON i.object_id = d.object_id AND i.index_id = d.index_id
left join sys.partition_schemes as p on i.data_space_id = p.data_space_id
WHERE d.avg_fragmentation_in_percent > @index_defrag_threshhold AND d.index_id > 0