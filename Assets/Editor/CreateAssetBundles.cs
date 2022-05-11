using UnityEditor;
public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None,
            BuildTarget.Android);
    }
    
    [MenuItem("Assets/Build AssetBundles Windows")]
    static void BuildAllAssetBundlesWindows()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows);
    }
}
