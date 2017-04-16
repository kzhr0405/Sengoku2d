using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;


public class AssetBundler {
    [MenuItem("Edit/Build AssetBundle")]
    static void Export() {
        //BuildPipeline.BuildAssetBundles("Assets/AssetBundles/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles/iOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
    }
}