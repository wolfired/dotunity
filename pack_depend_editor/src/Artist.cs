using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

using UnityEngine;
using UnityEditor;

using com.wolfired.command;
using com.wolfired.utils;

namespace com.wolfired.artist
{
    public static class ArtConst
    {
        public static string SettingTextureName = "setting.png";
        public static List<string> PlatformStrings = new List<string>{
            "DefaultTexturePlatform",
            "Standalone",
            "Web",
            "iPhone",
            "Android",
            "WebGL",
            "Windows Store Apps",
            "PS4",
            "XboxOne",
            "Nintendo 3DS",
            "tvOS",
        };
    }

    internal sealed class ArtSetting : Command
    {
        public static readonly new string CMD = "art_setting";
        public static readonly new string Desc = "生成模板图片";

        internal ArtSetting()
        {
            this._opts.Add<string>("p|assetPath=", "模板图片保存目录", v => this._assetPath = v);
        }

        private string _assetPath;

        public override string cmd => CMD;
        public override string desc => Desc;

        protected override void DoExec()
        {
            var filesystemPath = PathUtils.AssetPath2FilesystemPath(this._assetPath);
            string[] filesInPath = Directory.GetFiles(filesystemPath, ArtConst.SettingTextureName, SearchOption.TopDirectoryOnly);

            if (0 < filesInPath.Length)
            {
                return;
            }

            string textureFilesystemPath = Path.Combine(filesystemPath, ArtConst.SettingTextureName);

            Texture2D tex2d = new Texture2D(4, 4);
            File.WriteAllBytes(textureFilesystemPath, tex2d.EncodeToPNG());
            AssetDatabase.ImportAsset(textureFilesystemPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();
        }
    }

    [XmlRoot("TextureAtlas")]
    public class TextureAtlas
    {
        public static TextureAtlas Load(string filesystemPath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TextureAtlas));
            FileStream fileStream = new FileStream(filesystemPath, FileMode.Open);
            TextureAtlas textureAtlas = xmlSerializer.Deserialize(fileStream) as TextureAtlas;
            fileStream.Close();
            return textureAtlas;
        }

        [XmlRoot("sprite")]
        public class Sprite
        {
            [XmlAttribute("n")]
            public string name;
            [XmlAttribute("x")]
            public int x;
            [XmlAttribute("y")]
            public int y;
            [XmlAttribute("w")]
            public int w;
            [XmlAttribute("h")]
            public int h;
        }

        [XmlAttribute("imagePath")]
        public string imagePath;

        [XmlAttribute("width")]
        public int width;

        [XmlAttribute("height")]
        public int height;

        [XmlElement("sprite")]
        public List<Sprite> sprites;
    }

    public class ArtistPostprocessor : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            var filesystemPath = PathUtils.AssetPath2FilesystemPath(this.assetPath);

            if (ArtConst.SettingTextureName == Path.GetFileName(filesystemPath)) { return; }

            TextureImporter presetTextureImporter = this.LookupPresetTextureImporter(Path.GetDirectoryName(filesystemPath));

            if (null == presetTextureImporter) { return; }

            TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
            presetTextureImporter.ReadTextureSettings(textureImporterSettings);

            TextureImporter textureImporter = this.assetImporter as TextureImporter;
            textureImporter.SetTextureSettings(textureImporterSettings);

            foreach (string platform_string in ArtConst.PlatformStrings)
            {
                textureImporter.SetPlatformTextureSettings(presetTextureImporter.GetPlatformTextureSettings(platform_string));
            }

            if (TextureImporterType.Sprite == textureImporter.textureType && SpriteImportMode.Multiple == textureImporter.spriteImportMode)
            {
                var spriteSheetXML = Path.Combine(Path.GetDirectoryName(filesystemPath), Path.GetFileNameWithoutExtension(filesystemPath) + ".xml");
                if (File.Exists(spriteSheetXML))
                {
                    TextureAtlas textureAtlas = TextureAtlas.Load(spriteSheetXML);

                    List<SpriteMetaData> metas = new List<SpriteMetaData>();

                    foreach (TextureAtlas.Sprite sprite in textureAtlas.sprites)
                    {
                        SpriteMetaData meta = new SpriteMetaData();
                        meta.name = sprite.name;
                        meta.rect = new Rect(sprite.x, textureAtlas.height - sprite.y - sprite.h, sprite.w, sprite.h);
                        metas.Add(meta);
                    }

                    textureImporter.spritesheet = metas.ToArray();
                }
            }

            textureImporter.SaveAndReimport();

            AssetDatabase.Refresh();
        }

        void OnPostprocessTexture(Texture2D texture) { }

        private TextureImporter LookupPresetTextureImporter(string filesystemPath)
        {
            var root = Path.GetPathRoot(filesystemPath);
            while (root != filesystemPath)
            {
                string[] presetTextureImporters = Directory.GetFiles(filesystemPath, ArtConst.SettingTextureName, SearchOption.TopDirectoryOnly);
                foreach (string presetTextureImporter in presetTextureImporters)
                {
                    return TextureImporter.GetAtPath(PathUtils.FilesystemPath2AssetPath(presetTextureImporter)) as TextureImporter;
                }

                filesystemPath = Path.GetDirectoryName(filesystemPath);
            }

            return null;
        }
    }
}
