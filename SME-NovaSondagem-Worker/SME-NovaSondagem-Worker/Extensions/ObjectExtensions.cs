using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SME.NovaSondagem.Worker.Extensions
{
    public static class ObjectExtensions
    {
        public static bool NaoEhNulo(this object obj) => obj != null;

        public static IEnumerable<T> ObterConstantesPublicas<T>(this Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                      .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                      .Select(fi => (T)fi.GetRawConstantValue());
        }
    }
}