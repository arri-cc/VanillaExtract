using System.Collections.Generic;

namespace FluentDataObfuscator.Infrastructure
{
    public class Table
    {
        public string Name { get; set; }
        public IEnumerable<Column> Columns { get; set; }
    }
}