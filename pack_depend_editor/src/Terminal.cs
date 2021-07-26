using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mono.Options;
using UnityEngine;
using UnityEditor;
using com.wolfired.command;

namespace com.wolfired
{
    public class TerminalWindow : EditorWindow
    {
        [MenuItem("Terminal/Open")]
        public static void Open()
        {
            EditorWindow.GetWindow<TerminalWindow>().Show();
        }

        public TerminalWindow() { }

        private string _txt_input = "help";

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("log_empty -c -f")) { Commands.Exec("log_empty -c -f"); }
            if (GUILayout.Button("addr_init")) { Commands.Exec("addr_init"); }
            if (GUILayout.Button("addr_import")) { Commands.Exec("addr_import"); }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(16f);

            EditorGUILayout.BeginHorizontal();
            GUI.SetNextControlName("txt_input");
            this._txt_input = EditorGUILayout.TextField(this._txt_input);
            GUI.SetNextControlName("btn_enter");
            if (GUILayout.Button("Enter", GUILayout.Width(86)) && "" != this._txt_input)
            {
                GUI.FocusControl("btn_enter");
                Commands.Exec(this._txt_input);
                this._txt_input = "";
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
