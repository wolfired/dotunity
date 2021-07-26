using System;

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
            Debug.Log("InitializeOnLoad");

            AssemblyReloadEvents.beforeAssemblyReload += () =>
            {
                LogHandler.ins.ReplaceDefaultLogHandler(false);
                LogHandler.ins.LogToFile("");
                LogWriter.ins.CaptureConsoleOutput(false);

                Debug.Log("AssemblyReloadEvents.beforeAssemblyReload");
            };

            AssemblyReloadEvents.afterAssemblyReload += () =>
            {
                Debug.Log("AssemblyReloadEvents.afterAssemblyReload");

                LogWriter.ins.CaptureConsoleOutput(true);
                LogHandler.ins.LogToFile(Application.dataPath + "/../logs.txt");
                LogHandler.ins.ReplaceDefaultLogHandler(true);
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
