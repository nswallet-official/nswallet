using System;
using System.Linq;

namespace NSWallet.UnitTests
{
    public static class CommonUT
    {
        static readonly Random random = new Random(Guid.NewGuid().GetHashCode());

        public static int RandomRange(int min, int max)
        {
            return random.Next(min, max);
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz" +
                "0123456789" +
                "_!@#$%^&*()<>,./?" +
                "ЙЦУКЕНГШЩЗФЫВАПРОЛДЯЧСМИТЬБЮйцукенгшщзхъфывапролджэёячсмитьбю";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
