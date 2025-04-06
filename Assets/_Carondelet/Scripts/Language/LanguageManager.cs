using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageManager : MonoBehaviour
{
    private bool isSwitching = false; 
    public void SwitchLanguage()
    {
        if (isSwitching) return;
        StartCoroutine(ChangeLanguage());
    }

    private IEnumerator ChangeLanguage()
    {
        isSwitching = true;
        yield return LocalizationSettings.InitializationOperation;

        int currentLocaleIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        int nextLocaleIndex = (currentLocaleIndex + 1) % LocalizationSettings.AvailableLocales.Locales.Count;
        
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[nextLocaleIndex];

        isSwitching = false;
    }
}
