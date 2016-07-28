using System.Collections.Generic;

namespace FluentDataObfuscator.Infrastructure
{
    public interface ITableRepository
    {
        IEnumerable<Table> GetSelected(IEnumerable<string> tables);
    }
}