SELECT TOP 20 SUBSTRING(qt.text, (qs.statement_start_offset/2)+1,
        ((CASE qs.statement_end_offset
 WHEN -1 THEN DATALENGTH(qt.text)
         ELSE qs.statement_end_offset
         END - qs.statement_start_offset)/2)+1)
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) qt
WHERE qt.encrypted=0
ORDER BY qs.total_logical_reads DESC
