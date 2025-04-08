using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using System.Collections;
using System.Collections.Generic;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public GameObject dialoguePanel;
    public float textSpeed = 0.05f;
    public float waitBetweenSegments = 1f;
    public int linesPerSegment = 3;

    public List<LocalizedDialogue> dialogueList;

    private List<string> currentDialogueLines;
    private int currentLineIndex;
    private bool isDialogueActive = false;

    void Start()
    {
        textComponent.text = string.Empty;
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(int index)
    {
        if (index >= 0 && index < dialogueList.Count)
        {
            StopAllCoroutines();
            textComponent.text = string.Empty;

            LocalizedString localizedString = dialogueList[index].localizedString;
            string localizedText = localizedString.GetLocalizedString();

            currentDialogueLines = SplitTextIntoLines(localizedText);
            currentLineIndex = 0;

            if (isDialogueActive)
            {
                StopCoroutine("DisplaySegments");
            }

            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        StartCoroutine(DisplaySegments());
    }

    IEnumerator DisplaySegments()
    {
        while (currentLineIndex < currentDialogueLines.Count)
        {
            textComponent.text = "";

            int linesThisSegment = Mathf.Min(linesPerSegment, currentDialogueLines.Count - currentLineIndex);

            for (int i = 0; i < linesThisSegment; i++)
            {
                yield return StartCoroutine(TypeLine(currentDialogueLines[currentLineIndex]));
                textComponent.text += "\n";
                currentLineIndex++;
            }

            yield return new WaitForSeconds(waitBetweenSegments);
        }

        EndDialogue();
    }

    IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }

    private List<string> SplitTextIntoLines(string text)
    {
        List<string> lines = new List<string>();
        string[] words = text.Split(' ');

        string currentLine = "";
        foreach (var word in words)
        {
            if ((currentLine + " " + word).Length > 50)
            {
                lines.Add(currentLine.Trim());
                currentLine = word;
            }
            else
            {
                currentLine += " " + word;
            }
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine.Trim());
        }

        return lines;
    }
}
