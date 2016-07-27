using System;
using System.Collections.Generic;

namespace FluentDataObfuscator.Infrastructure
{
    public class Obfuscator
    {
        public string Table { get; }

        public IReadOnlyDictionary<string, IObfuscation> Obfuscations { get; }

        public Obfuscator(string table, IReadOnlyDictionary<string, IObfuscation> obfuscations)
        {
            Table = table;
            Obfuscations = obfuscations;
        }

        public IDictionary<string, object> Obfuscate(object input)
        {
            var output = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var prop in input.GetType().GetProperties())
            {
                var name = prop.Name;
                var value = prop.GetValue(input);

                if (Obfuscations.ContainsKey(name))
                    value = Obfuscations[name].Obfuscate();

                output.Add(name, value);
            }

            return output;
        }
    }
}