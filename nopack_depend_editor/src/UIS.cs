using System;
using System.Collections;

using UnityEngine;
using UnityEditor;

namespace com.wolfired.uis
{
    public class InputTextWindow : EditorWindow
    {
        [MenuItem("wolfired/uis/InputTextWindow/ShowWindow")]
        public static void ShowWindow()
        {
            new InputTextWindow().ShowUtility();
        }

        public static void ShowWindow(string title, Action<string> cb)
        {
            new InputTextWindow(title, cb).ShowUtility();
        }

        public InputTextWindow() { }

        public InputTextWindow(string title, Action<string> cb)
        {
            this.titleContent = new GUIContent(title);
            this.cb = cb;
        }

        private Action<string> cb;
        private string inputText = "";

        void OnGUI()
        {
            this.inputText = EditorGUILayout.TextField(this.inputText);

            if (GUILayout.Button("确定") && null != this.cb)
            {
                this.cb(this.inputText);
                this.Close();
            }
        }
    }
}
