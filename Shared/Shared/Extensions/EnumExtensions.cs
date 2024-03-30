namespace Shared.Extensions;

public static class EnumExtensions
{
    public static int GetValue<T>(this T enumType) where T : Enum
    {
        return Convert.ToInt32(enumType);
    }
}