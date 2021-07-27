using System;
using System.IO;

using UnityEngine;
using UnityEditor;

using com.wolfired.log;

namespace com.wolfired
{
    [InitializeOnLoad]
    public class InitializeOnX
    {
        static InitializeOnX()
        {
            LogHandler.ins.ReplaceDefaultLogHandler(true);
            LogHandler.ins.LogToFile(Application.dataPath + "/../logs.txt");
            LogWriter.ins.CaptureConsoleOutput(true);

            Debug.Log("InitializeOnLoad");

            AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                Debug.Log("AssemblyReloadEvents.beforeAssemblyReload");

                LogHandler.ins.ReplaceDefaultLogHandler(false);
                LogHandler.ins.LogToFile("");
                LogWriter.ins.CaptureConsoleOutput(false);
            };

            AssemblyReloadEvents.afterAssemblyReload += () =>
            {
                Debug.Log("AssemblyReloadEvents.afterAssemblyReload");
            };
        }

        [InitializeOnLoadMethod]
        public static void InitializeOnLoadMethod()
        {
            Debug.Log("InitializeOnLoadMethod");
        }

        [InitializeOnEnterPlayMode]
        public static void InitializeOnEnterPlayMode(EnterPlayModeOptions options)
        {
            Debug.Log("InitializeOnEnterPlayMode: " + options);
        }
    }
}
