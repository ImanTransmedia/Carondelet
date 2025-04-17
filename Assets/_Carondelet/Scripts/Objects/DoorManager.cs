using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;
    public string LastDoorUsed { get; set; }
    public SceneInstance PreviousScene { get; set; }

    void Awake()
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
}
