using System.Collections.Generic;
using System.Linq;

namespace FluentDataObfuscator.Infrastructure
{
    public class ObfuscatorConfiguration
    {
        private readonly IList<Obfuscator> _obfuscators = new List<Obfuscator>();

        public IEnumerable<Obfuscator> Obfuscators => _obfuscators.AsEnumerable();

        public Obfuscator ForTable(string table)
        {
            _obfuscators.Add(new Obfuscator(table));
            return _obfuscators.First(x => x.Table == table);
        }
    }
}