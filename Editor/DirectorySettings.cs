using UnityEngine;

namespace QuickPath
{
    public class DirectorySettings : ScriptableObject
    {
        public Directory[] Directories => directories;
        public bool IsEmpty => directories.Length == 0;

        [SerializeField] private Directory[] directories;
    }
}
