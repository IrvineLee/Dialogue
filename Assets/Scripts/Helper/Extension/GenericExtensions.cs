using System.Collections.Generic;

namespace Dico.Helper
{
    public static class GenericExtensions
    {
        public static bool IsDefault<T>(this T t)
        {
            return EqualityComparer<T>.Default.Equals(t, default(T));
        }
    }
}