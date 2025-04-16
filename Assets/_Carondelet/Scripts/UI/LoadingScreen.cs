using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;

    public void UpdateProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);
        if (progressBar) progressBar.value = progress;
        if (progressText) progressText.text = $"{(progress * 100):0}%";
    }

    // Automatically destroy when scene changes
    private void Update()
    {
        if (!SceneManager.GetActiveScene().isLoaded)
        {
            Destroy(gameObject);
        }
    }
}