using System.Linq;
using System.IO;

using UnityEngine;
using UnityEditor;

using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

using com.wolfired.command;

namespace com.wolfired.addrable
{
    public sealed class AddrInit : Command
    {
        public static readonly new string CMD = "addr_init";
        public static readonly new string Desc = "初始化";

        public override string cmd => CMD;
        public override string desc => Desc;

        protected override void DoExec()
        {
            var addrSetting = AddressableAssetSettingsDefaultObject.GetSettings(true);

            var profileSetting = addrSetting.profileSettings;

            var defaultID = profileSetting.GetProfileId("Default");
            var developID = profileSetting.AddProfile("Develop", defaultID);
            var releaseID = profileSetting.AddProfile("Release", defaultID);
            addrSetting.activeProfileId = developID;

            profileSetting.SetValue(developID, AddressableAssetSettings.kRemoteBuildPath, "HostData/Local/[BuildTarget]");
            profileSetting.SetValue(developID, AddressableAssetSettings.kRemoteLoadPath, "http://localhost:50000");

            profileSetting.SetValue(releaseID, AddressableAssetSettings.kRemoteBuildPath, "HostData/Remote/[BuildTarget]");
            profileSetting.SetValue(releaseID, AddressableAssetSettings.kRemoteLoadPath, "http://localhost:50001");
        }
    }

    public sealed class AddrImport : Command
    {
        public static readonly new string CMD = "addr_import";
        public static readonly new string Desc = "导入可寻址资源";

        public override string cmd => CMD;
        public override string desc => Desc;

        protected override void DoExec()
        {
            var setting = AddressableAssetSettingsDefaultObject.GetSettings(true);

            var group = setting.FindGroup("Default Local Group");

            var paths = Directory.GetFiles(Application.dataPath + "/Addrable", "*.*", SearchOption.AllDirectories).Where(file => !file.EndsWith("meta")).ToArray();

            foreach (var path in paths)
            {
                var temp = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var guid = AssetDatabase.AssetPathToGUID("Assets" + temp.Substring(Application.dataPath.Length));
                var e = setting.CreateOrMoveEntry(guid, group);
            }

            EditorUtility.SetDirty(setting);
        }
    }
}
