using System;
using UnityEngine;
using SLZ.Props;
using MelonLoader;
using HarmonyLib;

namespace bonelab_template
{
    public class DestroyGameObjectWithPullCordForceChange : MelonMod
    {
        private float checkInterval = 5.0f; // Check every 5 seconds
        private float nextCheckTime = 0.0f;

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("DestroyGameObjectWithPullCordForceChange mod loaded");

            // Initialize Harmony
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("com.mygamemods.destroywithpullcordforcechange");

            // Apply the Harmony patch
            harmony.Patch(typeof(PullCordForceChange).GetMethod("Update"), new HarmonyLib.HarmonyMethod(typeof(DestroyGameObjectWithPullCordForceChange), nameof(Prefix)));
        }

        private bool Prefix(PullCordForceChange __instance)
        {
            // Destroy the GameObject if it has the PullCordForceChange component
            string objectName = __instance.gameObject.name;
            GameObject.Destroy(__instance.gameObject);
            MelonLogger.Msg($"Found and destroyed GameObject: {objectName}");

            // Skip the original Update method
            return false;
        }

        public override void OnUpdate()
        {
            // This method can be left empty since the patching handles the behavior.
        }
    }
}
