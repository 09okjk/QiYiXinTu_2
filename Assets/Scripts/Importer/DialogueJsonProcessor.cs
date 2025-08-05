#if UNITY_EDITOR
using System;
using System.IO;
using Dialogue; // 确保引用了包含 DialogueData 的命名空间
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Importer
{
    public class DialogueJsonProcessor : EditorWindow
    {
        private const string SAVE_PATH = "Assets/Resources/ScriptableObjects/Dialogues";
        private const string JSON_FILES_PATH = "Assets/DialogueJson"; // JSON文件的导入/导出目录

        private DialogueData dialogueDataToProcess; // 用于在窗口中拖拽S.O.进行导出

        [MenuItem("Tools/Dialogue System/JSON Processor")]
        public static void ShowWindow()
        {
            GetWindow<DialogueJsonProcessor>("Dialogue JSON Processor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Dialogue JSON Import/Export", EditorStyles.boldLabel);
            
            EditorGUILayout.Space(10);

            // --- 导入部分 ---
            GUILayout.Label("Import from JSON", EditorStyles.miniBoldLabel);
            if (GUILayout.Button("Import JSON File(s)"))
            {
                // 让用户从指定的目录选择文件
                string[] paths = new[]
                {
                    EditorUtility.OpenFilePanelWithFilters("Select JSON Dialogue File(s)", JSON_FILES_PATH, new[] { "JSON", "json" })
                };
                if (paths.Length > 0)
                {
                    ImportFromJsons(paths);
                }
            }

            EditorGUILayout.Space(20);

            // --- 导出部分 ---
            GUILayout.Label("Export to JSON", EditorStyles.miniBoldLabel);
            dialogueDataToProcess = (DialogueData)EditorGUILayout.ObjectField("DialogueData to Export", dialogueDataToProcess, typeof(DialogueData), false);

            if (GUILayout.Button("Export Selected DialogueData"))
            {
                if (dialogueDataToProcess == null)
                {
                    EditorUtility.DisplayDialog("Error", "Please select a DialogueData asset to export.", "OK");
                    return;
                }
                ExportToJson(dialogueDataToProcess);
            }
            
            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox(
                "Import: Select one or more .json files to create or update DialogueData assets.\n\n" +
                "Export: Drag a DialogueData asset above and click export to save it as a .json file.", 
                MessageType.Info);
        }

        private void ImportFromJsons(string[] filePaths)
        {
            EnsureDirectoryExists(SAVE_PATH);
            int successCount = 0;

            foreach (string path in filePaths)
            {
                try
                {
                    string jsonContent = File.ReadAllText(path);

                    // 定义序列化设置，这是正确处理多态（DialogueCondition的子类）的关键
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };
                    
                    // 临时的反序列化检查，以获取 dialogueID
                    var temp = JsonConvert.DeserializeObject<DialogueData>(jsonContent, settings);
                    if (temp == null || string.IsNullOrEmpty(temp.dialogueID))
                    {
                        Debug.LogWarning($"Skipping file {Path.GetFileName(path)}: Could not parse or missing dialogueID in JSON.");
                        continue;
                    }

                    string assetPath = Path.Combine(SAVE_PATH, $"{temp.dialogueID}.asset");
                    DialogueData existingAsset = AssetDatabase.LoadAssetAtPath<DialogueData>(assetPath);

                    if (existingAsset != null)
                    {
                        // 更新现有资产
                        // 使用 PopulateObject 将JSON数据填充到已存在的 ScriptableObject 实例中
                        JsonConvert.PopulateObject(jsonContent, existingAsset, settings);
                        EditorUtility.SetDirty(existingAsset); // 标记为已修改，以便保存
                        Debug.Log($"Updated existing asset: {assetPath}");
                    }
                    else
                    {
                        // 创建新资产
                        DialogueData newAsset = ScriptableObject.CreateInstance<DialogueData>();
                        JsonConvert.PopulateObject(jsonContent, newAsset, settings);
                        AssetDatabase.CreateAsset(newAsset, assetPath);
                        Debug.Log($"Created new asset: {assetPath}");
                    }
                    successCount++;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to import {Path.GetFileName(path)}. Error: {e.Message}\n{e.StackTrace}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Import Complete", $"Successfully imported/updated {successCount} of {filePaths.Length} files.", "OK");
        }

        private void ExportToJson(DialogueData data)
        {
            try
            {
                EnsureDirectoryExists(JSON_FILES_PATH);
                
                // 定义序列化设置
                var settings = new JsonSerializerSettings
                {
                    // 在序列化时自动添加 "$type" 字段以保存类型信息，这对于多态至关重要
                    TypeNameHandling = TypeNameHandling.Auto,
                    // 忽略循环引用，虽然在当前数据结构中不太可能出现
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    // 使JSON文件格式化，易于人类阅读
                    Formatting = Formatting.Indented 
                };

                string jsonContent = JsonConvert.SerializeObject(data, settings);
                string filePath = Path.Combine(JSON_FILES_PATH, $"{data.dialogueID}.json");
                File.WriteAllText(filePath, jsonContent);
                
                EditorUtility.DisplayDialog("Export Successful", $"DialogueData has been exported to:\n{filePath}", "OK");
                // 刷新AssetDatabase以在Project窗口中立即显示新文件
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Export Failed", $"An error occurred during export: {e.Message}", "OK");
            }
        }

        /// <summary>
        /// 确保指定的目录存在，如果不存在则创建它。
        /// </summary>
        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
#endif