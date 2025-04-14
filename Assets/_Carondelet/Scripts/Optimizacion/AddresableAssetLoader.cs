using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddresableAssetLoader : MonoBehaviour
{

    [SerializeField] private AssetReference assetReference;

    private void Awake()
    {
        assetReference.LoadAssetAsync<GameObject>().Completed += (AsyncOperationHandle) =>
        {
            if (AsyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Instantiate(AsyncOperationHandle.Result);
            }
            else
            {
                Debug.Log("Error al cargar!");
            }
        };
    }
}
