using UnityEditor;
using Unity.CodeEditor;

namespace com.wolfired
{
    public class CommandLineExecuteMethods
    {
        public static void GenU3DProjectFiles()
        {
            CodeEditor.Editor.CurrentCodeEditor.SyncAll();
            EditorApplication.Exit(0);
        }
    }
}