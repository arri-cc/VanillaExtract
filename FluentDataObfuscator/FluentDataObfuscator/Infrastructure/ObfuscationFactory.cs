using System;
using FluentDataObfuscator.Obfuscations;

namespace FluentDataObfuscator.Infrastructure
{
    public class ObfuscationFactory
    {
        public static IObfuscation Create(ObfuscationType type)
        {
            switch (type)
            {
                case ObfuscationType.FirstName:
                    return new FirstNameObfuscation();
                case ObfuscationType.Ssn:
                    return new SsnObfuscation();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}