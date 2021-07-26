using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace com.wolfired.log
{
    public class LogHandler : ILogHandler
    {
        private static LogHandler _logHandler = null;

        public static LogHandler ins
        {
            get
            {
                if (null == _logHandler)
                {
                    _logHandler = new LogHandler();
                }
                return _logHandler;
            }
        }

        private LogHandler() { }

        private string _logFile = "";
        private StreamWriter _streamWriter = null;
        private ILogHandler _defaultLogHandler = null;

        public string LogFile
        {
            get { return this._logFile; }
        }

        public void ReplaceDefaultLogHandler(bool replaceOrNot)
        {
            if (replaceOrNot && null == this._defaultLogHandler)
            {
                this._defaultLogHandler = Debug.unityLogger.logHandler;
                Debug.unityLogger.logHandler = this;
            }

            if (!replaceOrNot && null != this._defaultLogHandler)
            {
                Debug.unityLogger.logHandler = this._defaultLogHandler;
                this._defaultLogHandler = null;
            }
        }

        public void LogToFile(string logFile)
        {
            if (this._logFile == logFile)
            {
                return;
            }

            if (!String.IsNullOrEmpty(this._logFile))
            {
                this._streamWriter.Close();
                this._streamWriter = null;
            }


            if (!String.IsNullOrEmpty(logFile))
            {
                this._streamWriter = new StreamWriter(logFile, true);
                this._streamWriter.AutoFlush = true;
            }

            this._logFile = logFile;
        }

        public void EmptyLogFile()
        {
            if ("" == this._logFile)
            {
                return;
            }

            this._streamWriter.Close();
            this._streamWriter = null;

            File.WriteAllText(this._logFile, string.Empty);

            this._streamWriter = new StreamWriter(this._logFile, true);
            this._streamWriter.AutoFlush = true;
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            this._defaultLogHandler.LogException(exception, context);
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            if (null != this._streamWriter)
            {
                this._streamWriter.WriteLine(String.Format(format, args));
            }

            this._defaultLogHandler.LogFormat(logType, context, format, args);
        }
    }

    public class LogWriter : TextWriter
    {
        private static LogWriter _logWriter = null;

        public static LogWriter ins
        {
            get
            {
                return null == _logWriter ? new LogWriter() : _logWriter;
            }
        }

        private StringBuilder _buffer = new StringBuilder();

        private bool _isCaptureConsoleOutput = false;

        public bool IsCaptureConsoleOutput
        {
            get { return this._isCaptureConsoleOutput; }
        }

        public void CaptureConsoleOutput(bool captureOrNot)
        {
            if (this._isCaptureConsoleOutput == captureOrNot)
            {
                return;
            }

            this._isCaptureConsoleOutput = captureOrNot;

            Console.Out.Close();
            if (this._isCaptureConsoleOutput)
            {
                Console.SetOut(this);
            }
            else
            {
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
        }

        public override Encoding Encoding => Encoding.Default;

        public override void Flush()
        {
            Debug.Log(_buffer.ToString().TrimEnd('\r', '\n'));
            _buffer.Clear();
        }

        public override void Write(char value)
        {
            _buffer.Append(value);
            if (this.IsNewline(value))
            {
                this.Flush();
            }
        }

        public override void Write(char[] value, int index, int count)
        {
            _buffer.Append(value, index, count);
            if (this.IsNewline(value[value.Length - 1]))
            {
                this.Flush();
            }
        }

        public override void Write(string value)
        {
            _buffer.Append(value);
            if (this.IsNewline(value[value.Length - 1]))
            {
                this.Flush();
            }
        }

        private bool IsNewline(char value)
        {
            if (1 == Environment.NewLine.Length && ('\r' == value || '\n' == value))
            {
                return true;
            }
            if (2 == Environment.NewLine.Length && '\n' == value)
            {
                return true;
            }
            return false;
        }
    }
}
