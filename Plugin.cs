using System;
using System.Runtime.InteropServices;
using BepInEx;
using UnityEngine;

namespace CleanupMyGT
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private static readonly ConsoleEventDelegate Handler = ConsoleEventCallback;

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        private delegate bool ConsoleEventDelegate(int eventType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll")]
        private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

        void Awake()
        {
            SetConsoleCtrlHandler(Handler, true);
            Application.quitting += OnQuit;
        }

        private void OnQuit() => Cleanup();

        private static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Cleanup();
            }
            return false;
        }

        private static void Cleanup()
        {
            TerminateProcess(GetCurrentProcess(), 0);
        }
    }

    public static class PluginInfo
    {
        internal const string
            GUID = "Steve.CleanupMyGT",
            Name = "CleanupMyGT",
            Version = "1.0.0";
    }
}
