using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentDataObfuscator.Infrastructure
{
    public class Obfuscator
    {
        private readonly IDictionary<string, IObfuscation> _obfuscations;

        public string Table { get; }

        public IReadOnlyDictionary<string, IObfuscation> Obfuscations => (IReadOnlyDictionary<string, IObfuscation>)_obfuscations;

        public Obfuscator(string table)
        {
            Table = table;
            _obfuscations = new Dictionary<string, IObfuscation>(StringComparer.InvariantCultureIgnoreCase);
        }

        public Obfuscator WithField(string field, ObfuscationType type)
        {
            _obfuscations.Add(field, ObfuscationFactory.Create(type));
            return this;
        }

        public IDictionary<string, object> Obfuscate(object input)
        {
            return input.GetType().GetProperties()
                .Select(prop => prop.Name)
                .Where(name => _obfuscations.ContainsKey(name))
                .ToDictionary(
                    name => name,
                    name => _obfuscations[name].Obfuscate(),
                    StringComparer.InvariantCultureIgnoreCase);
        }
    }
}