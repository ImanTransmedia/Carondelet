using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class DoorSceneLoader : MonoBehaviour
{
    [SerializeField] public string doorID;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private AssetReference targetScene;
    [SerializeField] private GameObject loadingScreenPrefab;

    private GameObject loadingScreenInstance;
    private AsyncOperationHandle<SceneInstance> _sceneLoadHandle;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        DoorManager.Instance.LastDoorUsed = doorID;
        ShowLoadingScreen();
        LoadNewScene();
    }

    private void ShowLoadingScreen()
    {
        if (!loadingScreenPrefab) return;

        loadingScreenInstance = Instantiate(loadingScreenPrefab);
        DontDestroyOnLoad(loadingScreenInstance);
    }

    private void LoadNewScene()
    {
        // Release previous scene if needed
        if (_sceneLoadHandle.IsValid())
        {
            Addressables.Release(_sceneLoadHandle);
        }

        _sceneLoadHandle = Addressables.LoadSceneAsync(targetScene,
            LoadSceneMode.Single,
            activateOnLoad: true);

        _sceneLoadHandle.Completed += HandleSceneLoadComplete;
    }

    private void HandleSceneLoadComplete(AsyncOperationHandle<SceneInstance> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Scene loaded successfully");
            CleanupLoadingScreen();
        }
        else
        {
            Debug.LogError($"Scene load failed: {handle.OperationException}");
            CleanupLoadingScreen();
        }
    }

    private void CleanupLoadingScreen()
    {
        if (loadingScreenInstance != null)
        {
            Destroy(loadingScreenInstance);
            loadingScreenInstance = null;
        }
    }

    private void OnDestroy()
    {
        // Clean up resources when this object is destroyed
        if (_sceneLoadHandle.IsValid())
        {
            _sceneLoadHandle.Completed -= HandleSceneLoadComplete;
            Addressables.Release(_sceneLoadHandle);
        }
        CleanupLoadingScreen();
    }

    public Transform GetSpawnPoint() => spawnPoint;
}
