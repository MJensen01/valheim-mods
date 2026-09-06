using System;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace OrionNet
{
    /// <summary>
    /// One toggleable feature. Owns its own Harmony instance so a failure in one
    /// module can never take down the others or the plugin.
    /// </summary>
    internal abstract class FeatureModule
    {
        public abstract string Name { get; }

        public ConfigEntry<bool> EnabledCfg;
        protected Harmony Harmony;

        public string Status = "not-run";
        public bool Applied;

        protected static ManualLogSource Log => OrionNetPlugin.Log;

        public abstract void Configure(ConfigFile cfg);

        /// <summary>Apply the module's patches. Throw loudly on any mismatch.</summary>
        protected abstract void ApplyPatches();

        public void TryEnable(string guidPrefix)
        {
            if (EnabledCfg == null || !EnabledCfg.Value)
            {
                Status = "disabled";
                Applied = false;
                return;
            }

            try
            {
                Harmony = new Harmony(guidPrefix + "." + Name);
                ApplyPatches();
                Applied = true;
                Status = "applied";
                Log.LogInfo("[" + Name + "] applied");
            }
            catch (Exception e)
            {
                Applied = false;
                var msg = e.InnerException != null ? e.InnerException.Message : e.Message;
                Status = "FAILED(" + msg + ")";
                Log.LogError("[" + Name + "] FAILED to patch: " + e);
                Disable();
            }
        }

        public virtual void Disable()
        {
            try
            {
                if (Harmony != null) Harmony.UnpatchSelf();
            }
            catch (Exception e)
            {
                Log.LogWarning("[" + Name + "] unpatch failed: " + e.Message);
            }
        }

        /// <summary>Runtime gate: every patch body must call this before changing behaviour.</summary>
        protected internal static bool ServerActive()
        {
            return ZNet.instance != null && ZNet.instance.IsServer();
        }
    }
}
