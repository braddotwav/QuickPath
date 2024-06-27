using UnityEngine;
using UnityEditor;
using System.IO;

namespace QuickPath
{
    public class DirectoryEditorWindow : EditorWindow
    {
        private string folderPath = string.Empty;
        private string resourcesPath = string.Empty;

        private Texture headerTexture = default;
        private GUIStyle headerStyle = default;
        private Vector2 scrollPos = Vector2.zero;
        
        private DirectorySettings directorySettings = default;
        private DirectoryGenerator directoryGenerator = default;
        private Editor directoryEditor = default;

        [MenuItem("Tools/QuickPath")]
        public static void OpenEditorWindow()
        {
            var window = GetWindow<DirectoryEditorWindow>("QuickPath");
            window.minSize = new(450, 500);
        }

        private void OnEnable()
        {
            folderPath = PackageUtility.GetFolderPath("QuickPath");
            resourcesPath = PackageUtility.GetResourcePath(folderPath);
            directoryGenerator = new(resourcesPath);
            directorySettings = GetSettings(Path.Combine(resourcesPath, "Settings.asset"));
            headerTexture = Resources.Load<Texture>("quickpathheader");
            directoryEditor = Editor.CreateEditor(directorySettings);
        }

        private void OnDisable()
        {
            directoryGenerator = null;
        }

        private void OnGUI()
        {
            headerStyle = new()
            {
                alignment = TextAnchor.MiddleCenter
            };

            GUILayout.Label(headerTexture, headerStyle);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            directoryEditor.OnInspectorGUI();
            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();
            {
                if (GUILayout.Button("Generate Class", GUILayout.MinHeight(40)))
                {
                    directoryGenerator.GenerateClass(directorySettings);
                }

                GUILayout.Space(5);

                if (GUILayout.Button("Delete Class", GUILayout.MinHeight(30)))
                {
                    directoryGenerator.DeleteGeneratedClassFile();
                }
            }
        }

        /// <summary>
        /// Returns the directory settings file creating one if it does not already exist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private DirectorySettings GetSettings(string path)
        {
            if (LoadSettings(path) == null)
                SaveSettings(path);

            return LoadSettings(path);
        }

        /// <summary>
        /// Loads the directory settings from the desired path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private DirectorySettings LoadSettings(string path)
        {
            return (DirectorySettings)EditorGUIUtility.Load(path);
        }

        /// <summary>
        /// Creates and saves settings to the desired path
        /// </summary>
        /// <param name="path"></param>
        private void SaveSettings(string path)
        {
            var obj = CreateInstance<DirectorySettings>();
            AssetDatabase.CreateAsset(obj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log(string.Join(" ", "Created settings file at the following path", path));
        }
    }
}
