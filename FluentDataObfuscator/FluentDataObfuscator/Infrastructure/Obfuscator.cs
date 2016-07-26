using System;
using System.Collections.Generic;

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
            var output = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var prop in input.GetType().GetProperties())
            {
                var name = prop.Name;
                var value = prop.GetValue(input);

                if (_obfuscations.ContainsKey(name))
                    value = _obfuscations[name].Obfuscate();

                output.Add(name, value);
            }

            return output;
        }
    }
}