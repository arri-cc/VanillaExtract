using Dapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace FluentDataObfuscator.Infrastructure
{
    public class SqlTableRepository : ITableRepository
    {
        private readonly string _connectionString;

        public SqlTableRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Table> GetSelected(IEnumerable<string> tables)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return conn
                    .Query(@"select table_name as [Table], column_name as [Column]
                            from information_schema.columns 
                            where table_name in @tables", new { tables })
                    .ToLookup(x => x.Table, x => x.Column)
                    .Select(x => new Table
                    {
                        Name = x.Key,
                        Columns = x.Select(c => new Column { Name = c }).ToList()
                    })
                    .ToList();
            }
        }
    }
}