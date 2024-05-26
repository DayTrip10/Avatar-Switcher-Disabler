using System;
using System.Reflection;
using UnityEngine;
using SLZ.Props;
using MelonLoader;
using HarmonyLib;
using LabFusion.Utilities;

namespace bonelab_template
{
    public class DestroyGameObjectWithPullCordForceChange : MelonMod
    {
        private static HarmonyLib.Harmony harmony;

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("DestroyGameObjectWithPullCordForceChange mod loaded");
            MelonLogger.Msg("Avatar Forced Changes Prevented");  // Log message
            string subtitle = "Forced Avatar Changes Have Been Disabled";
            FusionNotifier.Send(new FusionNotification()
            {
                title = $"Avatar Changes Prevented",
                showTitleOnPopup = true,
                message = subtitle,
                isMenuItem = false,
                isPopup = true,
            });
            // Initialize Harmony if it's not already initialized
            if (harmony == null)
                harmony = new HarmonyLib.Harmony("com.mygamemods.destroywithpullcordforcechange");

            // Apply the Harmony patch
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [HarmonyPatch(typeof(PullCordForceChange), "ForceChange")]
        public static class ForceChange_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(PullCordForceChange __instance)
            {
                // Skip the original method
                return false;
            }
        }

        public override void OnUpdate()
        {
            // This method can be left empty since the patching handles the behavior.
        }

        public override void OnApplicationQuit()
        {
            // Unpatch all methods when the application quits
            harmony?.UnpatchAll();
        }
    }
}
