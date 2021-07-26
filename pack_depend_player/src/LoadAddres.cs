using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class LoadAddres : MonoBehaviour
{
    private AsyncOperationHandle<GameObject> ah0;
    async void Start()
    {
        Debug.Log("Clear cache: " + Caching.ClearCache());

        ResourceManager.ExceptionHandler = (ah0, e) =>
        {
            Addressables.LogException(ah0, e);
        };

        ah0 = Addressables.LoadAssetAsync<GameObject>("Assets/Addrable/Prefabs/Sphere.prefab");

        await ah0.Task;

        if (AsyncOperationStatus.Succeeded != ah0.Status)
        {
            Debug.Log("Load addres: " + ah0.Status);
            return;
        }

        GameObject.Instantiate(ah0.Result);
    }

    void Update()
    {
        if (ah0.IsDone)
        {
            return;
        }

        var img_prog = this.transform.Find("img_prog").gameObject.GetComponent<Image>();
        img_prog.fillAmount = ah0.GetDownloadStatus().Percent;

        var txt_prog = this.transform.Find("txt_prog").gameObject.GetComponent<Text>();
        txt_prog.text = ah0.GetDownloadStatus().DownloadedBytes + "/" + ah0.GetDownloadStatus().TotalBytes;
    }
}
