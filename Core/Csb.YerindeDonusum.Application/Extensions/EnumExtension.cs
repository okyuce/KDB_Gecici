using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;

namespace Csb.YerindeDonusum.Application.Extensions;

public static class EnumExtension
{
    /// <summary>
    /// [Display(Name ="Şirket")] formatindaki bilgiyi geriye doner
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string GetDisplayName(this Enum enumSource)
    {
        string? displayName = enumSource.GetType()
                                .GetMember(name: enumSource.ToString())
                                    .FirstOrDefault()?
                                        .GetCustomAttribute<DisplayAttribute>()
                                            ?.GetName();

        if (string.IsNullOrWhiteSpace(displayName))
            displayName = enumSource.ToString();

        return displayName;
    }

    /// <summary>
    /// [Description("Description text")] formatindaki bilgiyi geriye doner
    /// </summary>
    /// <param name="enumValue"></param>
    /// <returns></returns>
    public static string GetEnumDescription(this Enum enumSource)
    {
        string? description = enumSource.GetType()
                                .GetMember(name: enumSource.ToString())
                                    .FirstOrDefault()?
                                        .GetCustomAttribute<DescriptionAttribute>()
                                            ?.Description;

        if (string.IsNullOrWhiteSpace(description))
            description = enumSource.ToString();

        return description;
    }

    /// <summary>
    /// description bilgisinden enum property adini doner.
    /// string sirket = "Şirket";
    /// EnumExtension.GetEnumValueByDescription<TuzelKisilikEnum>(sirket);
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="description"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Eslesen bir veri bulunamamasi halinde hata doner</exception>
    /// <source>
    ///     http-s://www.c-sharpcorner.com/article/creating-an-extension-method-to-get-enum-description/
    /// </source>
    public static T GetEnumValueByDescription<T>(this string description) where T : Enum
    {
        foreach (Enum enumItem in Enum.GetValues(typeof(T)))
        {
            if (enumItem.GetEnumDescription() == description)
            {
                return (T)enumItem;
            }
        }
        throw new ArgumentException("Not found.", nameof(description));
    }
    public static T GetEnumValueByDisplayName<T>(this string name) where T : Enum
    {
        foreach (Enum enumItem in Enum.GetValues(typeof(T)))
        {
            if (enumItem.GetDisplayName().ToLower() == name.ToLower())
            {
                return (T)enumItem;
            }
        }
        return default;
    }

    public static IList<KeyValuePair<int, string>> GetList<T>() where T : Enum
    {
        var result = Enum.GetValues(typeof(T))
            .Cast<T>()
                .Select(v => new KeyValuePair<int, string>(Convert.ToInt32(v), v.GetDisplayName()))
                    .ToList();

        return result;
    }
}

