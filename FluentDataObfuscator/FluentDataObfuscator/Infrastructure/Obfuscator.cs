using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentDataObfuscator.Infrastructure
{
    public class Obfuscator
    {
        public string Table { get; }

        public IEnumerable<FieldObfuscation> Obfuscations { get; }

        public Obfuscator(string table, IEnumerable<FieldObfuscation> obfuscations)
        {
            Table = table;
            Obfuscations = obfuscations;
        }

        public IDictionary<string, object> Obfuscate(object input)
        {
            var output = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var prop in input.GetType().GetProperties())
            {
                var name = prop.Name.ToLower();
                var value = prop.GetValue(input);

                var obfuscation = Obfuscations
                    .Where(x => x.Field == name)
                    .Select(x => x.Obfuscation)
                    .SingleOrDefault();

                if (obfuscation != null)
                    value = obfuscation.Obfuscate();

                output.Add(name, value);
            }

            return output;
        }
    }
}