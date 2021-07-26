using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

using Mono.Options;

using UnityEngine;
using UnityEditor;

using com.wolfired.artist;
using com.wolfired.log;
using com.wolfired.addrable;

namespace com.wolfired.command
{
    public class Commands
    {
        public static Dictionary<string, Func<Command>> cmds = new Dictionary<string, Func<Command>>{
            {Help.CMD, () => new Help()},
            {LogInfo.CMD, () => new LogInfo()},
            {LogEmpty.CMD, () => new LogEmpty()},
            {AddrInit.CMD, () => new AddrInit()},
            {AddrImport.CMD, () => new AddrImport()},
            {ArtSetting.CMD, () => new ArtSetting()},
        };

        public static void Exec(string str_cmd_args)
        {
            str_cmd_args = str_cmd_args.Trim();

            if ("" == str_cmd_args)
            {
                return;
            }

            var cmd = Regex.Match(str_cmd_args, @"^[a-zA-Z0-9_]+").Value;

            var str_args = str_cmd_args.Replace(cmd, "").Trim();

            var args = Regex.Matches(str_args, @"[\""].+?[\""]|\S+").Cast<Match>().Select(m => m.Value).ToList();

            Exec(cmd, args);
        }

        public static void Exec(string cmd, List<string> args)
        {
            cmd = cmd.Trim();

            if (cmds.ContainsKey(cmd))
            {
                cmds[cmd]().Exec(args);
            }
            else
            {
                Console.WriteLine("无效命令: " + cmd);
            }
        }

        public static void Exec(List<string> list_cmd_args)
        {
            var cmd = list_cmd_args[0].Trim();
            var args = list_cmd_args.Skip<string>(1).ToList();
            Exec(cmd, args);
        }
    }

    public abstract class Command
    {
        public static readonly string CMD = "";
        public static readonly string Desc = "";

        public Command()
        {
            this._opts = new OptionSet
            {
                { "h|help", "显示帮助信息", value => this._help = null != value },
            };
        }

        protected OptionSet _opts;
        protected List<string> _exts;
        protected bool _help = false;

        public abstract string cmd { get; }

        public abstract string desc { get; }

        public void Exec(List<string> args)
        {
            this._exts = this._opts.Parse(args);

            if (this._help && this.ShowHelp()) { return; }

            this.DoExec();
        }

        protected virtual bool ShowHelp()
        {
            Console.WriteLine(this.cmd + "\t" + this.desc);
            Console.WriteLine("参数:");
            this._opts.WriteOptionDescriptions(Console.Out);
            return true;
        }

        protected virtual void DoExec() { }
    }

    internal sealed class Help : Command
    {
        public static readonly new string CMD = "help";
        public static readonly new string Desc = "显示全部命令";

        public override string cmd => CMD;

        public override string desc => Desc;

        protected override bool ShowHelp() { return false; }

        protected override void DoExec()
        {
            foreach (var fn in Commands.cmds.Values)
            {
                var item = fn();
                Console.WriteLine(String.Format("{0,24}: {1}", item.cmd, item.desc));
            }
        }
    }

    internal sealed class LogInfo : Command
    {
        public static readonly new string CMD = "log_info";
        public static readonly new string Desc = "显示日志信息";

        public override string cmd => CMD;

        public override string desc => Desc;

        protected override void DoExec() { }
    }

    internal sealed class LogEmpty : Command
    {
        public static readonly new string CMD = "log_empty";
        public static readonly new string Desc = "清空日志";

        internal LogEmpty()
        {
            this._opts.Add<string>("f", "清空日志文件", v => this._clearLogFile = null != v);
            this._opts.Add<string>("c", "清空控制台", v => this._clearConsole = null != v);
        }

        private bool _clearLogFile = false;
        private bool _clearConsole = false;

        public override string cmd => CMD;

        public override string desc => Desc;

        protected override void DoExec()
        {
            if (this._clearLogFile)
            {
                LogHandler.ins.EmptyLogFile();
            }

            if (this._clearConsole)
            {
                var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
                var type = assembly.GetType("UnityEditor.LogEntries");
                var method = type.GetMethod("Clear");
                method.Invoke(new object(), null);
            }
        }
    }
}
