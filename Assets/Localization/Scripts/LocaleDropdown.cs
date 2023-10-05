using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.Localization.Tables;

public class LocaleDropdown : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdown;
    public LocalizedTmpFont localizedTmpFont = null;
    public TMP_Text adsTxt = null;

    private List<LocalizedString> localizedStrings = null;
    IEnumerator Start()
    {
        localizedStrings = new List<LocalizedString>();
        localizedTmpFont.AssetChanged += LocalizeFonts;
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        var options = new List<TMPro.TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];

            if (LocalizationSettings.SelectedLocale == locale)
            {
                selected = i;
            }
            LocalizedString localizedString = new LocalizedString("Localization Table", locale.LocaleName);

            localizedStrings.Add(localizedString);
            TMPro.TMP_Dropdown.OptionData optionData = new TMPro.TMP_Dropdown.OptionData(locale.LocaleName);
            localizedStrings[i].StringChanged += val => optionData.text = val;
            options.Add(optionData);
        }

        dropdown.options = options;
        dropdown.value = selected;
        dropdown.onValueChanged.AddListener(LocaleSelected);
    }

    public void LocaleSelected(int index)
    {
        Locale locale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = locale;
        LocalizedString localizedString = new LocalizedString("Localization Table", locale.LocaleName);
        localizedString.StringChanged += val => dropdown.captionText.text = val;
        LocalizedTmpFont localizedTmpFont = new LocalizedTmpFont();
    }

    private void LocalizeFonts(TMP_FontAsset fontAsset)
    {
        dropdown.itemText.font = fontAsset;
        dropdown.itemText.ForceMeshUpdate();
        dropdown.captionText.font = fontAsset;
        dropdown.captionText.ForceMeshUpdate();
    }
}
