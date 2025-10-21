using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

public class TMPFontReplacer : EditorWindow
{
    private TMP_FontAsset newFontAsset;
    private string targetDirectory = "Assets/Bundles/UI";
    private List<string> processedPrefabs = new List<string>();
    private Vector2 scrollPosition;

    [MenuItem("Tools/替换所有TMP_Text的字体")]
    public static void ShowWindow()
    {
        GetWindow<TMPFontReplacer>("TMP Font Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("TMP Text Font Replacer", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // 目标目录显示
        EditorGUILayout.LabelField("Target Directory:", targetDirectory);
        
        // 新字体选择
        newFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField(
            "New Font Asset", 
            newFontAsset, 
            typeof(TMP_FontAsset), 
            false);

        GUILayout.Space(20);

        // 替换按钮
        if (GUILayout.Button("Replace Fonts", GUILayout.Height(30)))
        {
            if (newFontAsset == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a new font asset first!", "OK");
                return;
            }

            if (!Directory.Exists(targetDirectory))
            {
                EditorUtility.DisplayDialog("Error", $"Directory not found: {targetDirectory}", "OK");
                return;
            }

            ReplaceFontsInPrefabs();
            EditorUtility.DisplayDialog("Complete", 
                $"Font replacement complete!\nProcessed {processedPrefabs.Count} prefabs.", "OK");
        }

        GUILayout.Space(20);
        GUILayout.Label("Processed Prefabs:", EditorStyles.boldLabel);

        // 显示处理过的预制体列表
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
        foreach (var prefabPath in processedPrefabs)
        {
            EditorGUILayout.LabelField(prefabPath);
        }
        EditorGUILayout.EndScrollView();
    }

    private void ReplaceFontsInPrefabs()
    {
        processedPrefabs.Clear();
        
        // 获取所有预制体文件
        string[] prefabPaths = Directory.GetFiles(targetDirectory, "*.prefab", SearchOption.AllDirectories);
        
        foreach (string prefabPath in prefabPaths)
        {
            // 加载预制体
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null) continue;

            // 查找所有TMP_Text组件
            TMP_Text[] tmpTexts = prefab.GetComponentsInChildren<TMP_Text>(true);
            if (tmpTexts.Length == 0) continue;

            // 记录是否有修改
            bool isModified = false;

            // 替换字体
            foreach (TMP_Text tmpText in tmpTexts)
            {
                if (tmpText.font != newFontAsset)
                {
                    tmpText.font = newFontAsset;
                    isModified = true;
                }
            }

            // 如果有修改，保存预制体
            if (isModified)
            {
                EditorUtility.SetDirty(prefab);
                processedPrefabs.Add(prefabPath);
            }
        }

        // 保存所有修改
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}