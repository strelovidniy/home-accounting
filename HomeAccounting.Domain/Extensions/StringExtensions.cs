using System.Text;

namespace HomeAccounting.Domain.Extensions;

public static class StringExtensions
{
    public static byte[] ToByteArray(this string value) => Encoding.ASCII.GetBytes(value);
}