using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace QuickPath
{
    internal sealed class DirectoryGenerator
    {
        private const string FILE_NAME = "GeneratedDirectoryClass.cs";
        private const string TEMPLATE_FILE_NAME = "GenerateClassTemplate.template";

        private readonly string generateClassPath;
        private readonly string templateClassPath;

        public DirectoryGenerator(string resourcePath)
        {
            generateClassPath = Path.Combine(resourcePath, FILE_NAME);
            templateClassPath = Path.Combine(resourcePath, TEMPLATE_FILE_NAME);
        }

        public void GenerateClass(DirectorySettings settings)
        {
            try
            {
                var generatedClass = CreateGeneratedClassFromSettings(settings);
                File.WriteAllLines(generateClassPath, generatedClass);
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog("An Error Occurred", ex.Message, "OK");
            }
        }

        public void DeleteGeneratedClassFile()
        {
            if (!File.Exists(generateClassPath))
            {
                EditorUtility.DisplayDialog("An Error Occurred", "The generated class file does not exist", "OK");
                return;
            }

            AssetDatabase.DeleteAsset(generateClassPath);
            AssetDatabase.Refresh();
        }

        private List<string> CreateGeneratedClassFromSettings(DirectorySettings settings)
        {
            if (settings.IsEmpty)
            {
                throw new Exception("Cannot create generated class with empty settings");
            }

            var template = ReadAllLinesToListFromTemplateClass();
            List<string> array = new();

            for (int i = 0; i < settings.Directories.Length; i++)
            {
                array.Add(CreateAttributeAndMethod(settings.Directories[i].Name, settings.Directories[i].Path));
            }

            template.InsertRange(9, array);

            return template;
        }

        private List<string> ReadAllLinesToListFromTemplateClass()
        {
            return File.ReadAllLines(templateClassPath).ToList();
        }
        
        private string CreateAttributeAndMethod(string name, string path)
        {
            var attribute = $"\t[MenuItem(\"Directorys/{name}\")]";
            var method = $"\tprivate static void Open{name.Replace(" ", string.Empty)}() => System.Diagnostics.Process.Start(\"explorer.exe\", @\"{path}\");";
            return $"{attribute}\r\n{method}";
        }
    }
}
