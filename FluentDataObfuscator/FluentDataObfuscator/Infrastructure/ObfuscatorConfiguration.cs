using System.Collections.Generic;
using System.Linq;

namespace FluentDataObfuscator.Infrastructure
{
    public class ObfuscatorConfiguration
    {
        private readonly IList<ObfuscatorRegistration> _registrations = new List<ObfuscatorRegistration>();

        public IEnumerable<ObfuscatorRegistration> Registrations => _registrations.AsEnumerable();

        public IObfuscatorRegistration ForTable(string table)
        {
            _registrations.Add(new ObfuscatorRegistration(table));
            return _registrations.Last();
        }

        public Obfuscator GetObfuscatorFor(string table)
        {
            return _registrations
                .Where(x => x.Table == table.ToLower())
                .Select(x => new Obfuscator(x.Table, x.Obfuscations))
                .Single();
        }
    }
}