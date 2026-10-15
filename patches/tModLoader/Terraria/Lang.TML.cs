using Terraria.Localization;

namespace Terraria;

public partial class Lang
{
	/// <summary>Matches dialogue keys with the specified prefix whose embedded conditions and substitutions are satisfied by the object's public properties.</summary>
	public static LanguageSearchFilter CreateDialogFilter(string startsWith, object substitutions)
		=> (string key, LocalizedText text) => key.StartsWith(startsWith) && text.ConditionsMetWith(substitutions);
}
