using System;
using System.Collections.Generic;

using Mono.Options;

using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;

using com.wolfired.uis;
using Unity.CodeEditor;

namespace com.wolfired.pack
{
    internal class ListWrapper
    {
        public static void List(Action<PackageCollection> callback)
        {
            Debug.Log("Do List");
            new ListWrapper(callback);
        }

        private ListWrapper(Action<PackageCollection> callback)
        {
            this.callback = callback;
            this.request = Client.List();
            EditorApplication.update += this.update;
        }

        private ListRequest request;
        private Action<PackageCollection> callback;

        private void update()
        {
            if (StatusCode.InProgress == this.request.Status)
            {
                Debug.Log("Doing List");
            }

            if (!this.request.IsCompleted)
            {
                return;
            }

            Debug.Log("Done List");

            EditorApplication.update -= this.update;

            if (StatusCode.Failure == this.request.Status)
            {
                Debug.Log(request.Error.message);
                callback(null);
                return;
            }

            callback(this.request.Result);
        }
    }

    internal class AddWrapper
    {
        public static void Add(string identifier, Action<UnityEditor.PackageManager.PackageInfo> callback)
        {
            Debug.Log("Do Add");
            new AddWrapper(identifier, callback);
        }

        private AddWrapper(string identifier, Action<UnityEditor.PackageManager.PackageInfo> callback)
        {
            this.callback = callback;
            this.request = Client.Add(identifier);
            EditorApplication.update += this.update;
        }

        private AddRequest request;
        private Action<UnityEditor.PackageManager.PackageInfo> callback;

        private void update()
        {
            if (StatusCode.InProgress == this.request.Status)
            {
                Debug.Log("Doing Add");
            }

            if (!this.request.IsCompleted)
            {
                return;
            }

            Debug.Log("Done Add");

            EditorApplication.update -= this.update;

            if (StatusCode.Failure == this.request.Status)
            {
                Debug.Log(request.Error.message);
                callback(null);
                return;
            }

            callback(this.request.Result);
        }
    }

    public class Packer
    {
        [MenuItem("wolfired/pack/Packer/List")]
        public static void List()
        {
            ListWrapper.List((PackageCollection pc) =>
            {
                if (null == pc)
                {
                    return;
                }
                foreach (var p in pc)
                {
                    Debug.Log(p.displayName);
                }
            });
        }

        [MenuItem("wolfired/pack/Packer/Add")]
        public static void Add()
        {
            if (Application.isBatchMode)
            {
                // com.unity.2d.sprite
                // com.unity.addressables
                var help = false;
                var packs = new List<string>();

                var os = new OptionSet{
                    {"h|help", "show help message", v => help = null != v},
                    {"p|packs=", "packs to installed", v => packs.Add(v)},
                };

                var exts = os.Parse(System.Environment.GetCommandLineArgs());

                if (help)
                {
                    os.WriteOptionDescriptions(Console.Out);
                    EditorApplication.Exit(0);
                    return;
                }

                Action installer = () => { };
                Action<UnityEditor.PackageManager.PackageInfo> callback = (pi) => { installer(); };
                installer = () =>
                {
                    if (0 == packs.Count)
                    {
                        CodeEditor.Editor.CurrentCodeEditor.SyncAll();
                        EditorApplication.Exit(0);
                        return;
                    }
                    var pack = packs[0];
                    packs.RemoveAt(0);
                    AddWrapper.Add(pack, callback);
                };
                installer();
            }
            else
            {
                InputTextWindow.ShowWindow("请输入安装包名", (string inputText) =>
                {
                    if (null == inputText || "" == inputText)
                    {
                        AddWrapper.Add(inputText, (pi) => { });
                    }
                });
            }
        }
    }
}
