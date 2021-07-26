using System.IO;

using UnityEngine;

namespace com.wolfired.utils
{
    public sealed class PathUtils
    {
        public static string FilesystemPath2AssetPath(string filesystemPath)
        {
            // E:\workspace_u3d\addrsrc\Assets\Addrable\UITextures\bag.png -> Assets/Addrable/UITextures/bag.png
            return filesystemPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace(Application.dataPath, "Assets");
        }

        public static string AssetPath2FilesystemPath(string assetPath)
        {
            // Assets/Addrable/UITextures/bag.png -> E:/workspace_u3d/addrsrc/Assets/Addrable/UITextures/bag.png
            return Path.Combine(Path.GetDirectoryName(Application.dataPath), assetPath).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }
    }
}
