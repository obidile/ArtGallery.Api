namespace ArtGallery.Application.Common.Helpers;

public static class StringHelper
{
    public static string ToCamelCase(this string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return keyword;
        }
        return Char.ToLowerInvariant(keyword[0]) + keyword.Substring(1);
    }
}
