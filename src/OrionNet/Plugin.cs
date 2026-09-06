using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace OrionNet
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class OrionNetPlugin : BaseUnityPlugin
    {
        public const string PluginGuid = "net.mjensen.orion.net";
        public const string PluginName = "OrionNet";
        public const string PluginVersion = "0.1.0";

        internal static ManualLogSource Log;
        internal static readonly List<FeatureModule> Modules = new List<FeatureModule>();

        private Harmony _bootstrap;
        private static bool _summaryLogged;

        private void Awake()
        {
            Log = Logger;

            Modules.Add(new TelemetryModule());
            Modules.Add(new SendCadenceModule());
            Modules.Add(new SendBudgetModule());
            Modules.Add(new CreateBudgetModule());
            Modules.Add(new FrameRateModule());

            foreach (var m in Modules)
            {
                try { m.Configure(Config); }
                catch (Exception e) { Log.LogError("[" + m.Name + "] config bind failed: " + e); }
            }

            // Patches are installed now (ZNet.Start -> ServerLoadWorld happens too late for
            // some hooks), but every patch body gates on ZNet.instance.IsServer() at runtime.
            foreach (var m in Modules) m.TryEnable(PluginGuid);

            _bootstrap = new Harmony(PluginGuid + ".bootstrap");
            var znetStart = AccessTools.Method(typeof(ZNet), "Start");
            if (znetStart == null)
                Log.LogError("OrionNet: ZNet.Start not found - cannot detect server/client mode");
            else
                _bootstrap.Patch(znetStart,
                    postfix: new HarmonyMethod(typeof(OrionNetPlugin), nameof(ZNetStartPostfix)));

            Log.LogInfo("OrionNet " + PluginVersion + " loaded, " + Modules.Count + " modules");
        }

        private static void ZNetStartPostfix()
        {
            if (_summaryLogged) return;
            _summaryLogged = true;

            if (ZNet.instance == null || !ZNet.instance.IsServer())
            {
                foreach (var m in Modules) m.Disable();
                Log.LogInfo("OrionNet: client mode, no patches");
                return;
            }

            var parts = new List<string>();
            foreach (var m in Modules) parts.Add(m.Name + "=" + m.Status);
            Log.LogInfo("OrionNet module summary: " + string.Join(", ", parts.ToArray()));

            FrameRateModule.OnZNetStart();
        }

        private void Update()
        {
            float dt = Time.unscaledDeltaTime;
            TelemetryModule.Tick(dt);
            FrameRateModule.Tick(dt);
        }

        private void OnDestroy()
        {
            foreach (var m in Modules) m.Disable();
            try { if (_bootstrap != null) _bootstrap.UnpatchSelf(); }
            catch { /* shutting down */ }
        }
    }
}
