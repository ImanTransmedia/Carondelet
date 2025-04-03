using UnityEngine;
using TMPro;
public class UIIngameManager : MonoBehaviour
{
    public GameObject interactionText;

    public static UIIngameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowInteractPrompt()
    {
        interactionText.SetActive(true);
    }
    public void HideInteractPrompt()
    {
        interactionText.SetActive(false);
    }

}
