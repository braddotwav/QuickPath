using System.IO;
using UnityEditor;

namespace QuickPath
{
    public static class PackageUtility
    {
        public static string GetFolderPath(string folderName)
        {
            string[] guids = AssetDatabase.FindAssets(folderName, new[] { "Assets" });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (AssetDatabase.IsValidFolder(path) && path.EndsWith(folderName))
                {
                    return path;
                }
            }

            return null;
        }

        public static string GetResourcePath(string parentFolder)
        {
            string[] directories = System.IO.Directory.GetDirectories(parentFolder, "*", SearchOption.AllDirectories);
            foreach (string directory in directories)
            {
                if (directory.EndsWith("Resources") && AssetDatabase.IsValidFolder(directory))
                {
                    return directory;
                }
            }

            return null;
        }
    }
}
