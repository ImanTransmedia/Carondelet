using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.05f;
    public float timeBetweenLines = 1f;

    private List<LocalizedString> currentLines;
    private int index;

    public void ShowDialogue(List<LocalizedString> newLines)
    {
        gameObject.SetActive(true);
        currentLines = newLines;
        index = 0;
        textComponent.text = "";
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        var localizedString = currentLines[index];
        string line = "";

        var handle = localizedString.GetLocalizedStringAsync();
        yield return handle;
        line = handle.Result;

        textComponent.text = "";

        foreach (char c in line)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(timeBetweenLines);
        NextLine();
    }

    void NextLine()
    {
        if (index < currentLines.Count - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
