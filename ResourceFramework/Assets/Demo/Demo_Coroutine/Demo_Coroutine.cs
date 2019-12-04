﻿using UnityEngine;
using ResourceFramework;
using System.IO;
using System.Collections;

public class Demo_Coroutine : MonoBehaviour
{
    private const string BUNDLE_FOLDER_NAME = "bundle";
    private string PrefixPath { get; set; }
    private string Platform { get; set; }

    private void Start()
    {
        Platform = GetPlatform();
        PrefixPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../build")).Replace("\\", "/");
        PrefixPath += $"/{Platform}/{BUNDLE_FOLDER_NAME}";
        ResourceManager.instance.Initialize(GetFileUrl, false, 0);

        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        IResource uiResource = ResourceManager.instance.Load("Assets/AssetBundle/UI/UIRoot.prefab", true);
        yield return uiResource;
        uiResource.Instantiate();
        Transform uiParent = GameObject.Find("Canvas").transform;

        IResource testResource = ResourceManager.instance.Load("Assets/AssetBundle/UI/TestUI.prefab", true);
        yield return testResource;
        testResource.Instantiate(uiParent, false);
    }

    private string GetPlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                return "windows";
            case RuntimePlatform.Android:
                return "android";
            case RuntimePlatform.IPhonePlayer:
                return "ios";
            default:
                throw new System.Exception($"未支持的平台:{Application.platform}");
        }
    }

    private string GetFileUrl(string assetUrl)
    {
        return $"{PrefixPath}/{assetUrl}";
    }

    void Update()
    {
        ResourceManager.instance.Update();
    }

    private void LateUpdate()
    {
        ResourceManager.instance.LateUpdate();
    }
}
