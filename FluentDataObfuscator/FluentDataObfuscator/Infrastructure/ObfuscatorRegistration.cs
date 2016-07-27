using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentDataObfuscator.Infrastructure
{
    public class ObfuscatorRegistration : IObfuscatorRegistration
    {
        public string Table { get; }

        private readonly IList<FieldObfuscation> _obfuscations;

        public IEnumerable<FieldObfuscation> Obfuscations => _obfuscations.AsEnumerable();

        public ObfuscatorRegistration(string table)
        {
            Table = table.ToLower();
            _obfuscations = new List<FieldObfuscation>();
        }

        public IObfuscatorRegistration WithField<TObfuscation>(string field)
            where TObfuscation : IObfuscation
        {
            _obfuscations.Add(new FieldObfuscation(field, Activator.CreateInstance<TObfuscation>()));
            return this;
        }
    }
}