using System;
using System.Runtime.InteropServices;
using BepInEx;
using UnityEngine;

namespace CleanupMyGT
{
    [BepInPlugin("Steve.CleanupMyGT", "CleanupMyGT", "1")]
    public class Plugin : BaseUnityPlugin
    {
        [DllImport("kernel32.dll")] static extern bool SetConsoleCtrlHandler(Func<int, bool> cb, bool add);
        [DllImport("kernel32.dll")] static extern IntPtr GetCurrentProcess();
        [DllImport("kernel32.dll")] static extern bool TerminateProcess(IntPtr h, uint c);

        void Start() => GorillaTagger.OnPlayerSpawned(() => {
            SetConsoleCtrlHandler(e => { if (e == 2) Cleanup(); return false; }, true);
            Application.quitting += Cleanup;
        });

        static void Cleanup() => TerminateProcess(GetCurrentProcess(), 0);
    }
}
