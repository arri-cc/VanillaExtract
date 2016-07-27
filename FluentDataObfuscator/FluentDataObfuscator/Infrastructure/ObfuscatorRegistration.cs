using System;
using System.Collections.Generic;

namespace FluentDataObfuscator.Infrastructure
{
    public class ObfuscatorRegistration : IObfuscatorRegistration
    {
        public string Table { get; }

        private readonly IDictionary<string, IObfuscation> _obfuscations;

        public IReadOnlyDictionary<string, IObfuscation> Obfuscations => (IReadOnlyDictionary<string, IObfuscation>)_obfuscations;

        public ObfuscatorRegistration(string table)
        {
            Table = table.ToLower();
            _obfuscations = new Dictionary<string, IObfuscation>(StringComparer.InvariantCultureIgnoreCase);
        }

        public IObfuscatorRegistration WithField(string field, ObfuscationType type)
        {
            _obfuscations.Add(field, ObfuscationFactory.Create(type));
            return this;
        }
    }
}