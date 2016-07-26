using System.Collections.Generic;

namespace FluentDataObfuscator.Infrastructure
{
    public class Obfuscator
    {
        public string Table { get; }
        private readonly IDictionary<string, IObfuscation> _obfuscations;

        public IReadOnlyDictionary<string, IObfuscation> Obfuscations => (IReadOnlyDictionary<string, IObfuscation>)_obfuscations;

        public Obfuscator(string table)
        {
            Table = table;
            _obfuscations = new Dictionary<string, IObfuscation>();
        }

        public Obfuscator WithField(string field, ObfuscationType type)
        {
            _obfuscations.Add(field, ObfuscationFactory.Create(type));
            return this;
        }
    }
}