using System;
using System.Reflection;
using UnityEngine;
using SLZ.Props;
using MelonLoader;
using HarmonyLib;
using System.Security.AccessControl;

namespace bonelab_template
{
    public class DestroyGameObjectWithPullCordForceChange : MelonMod
    {
        private float checkInterval = 5.0f; // Check every 5 seconds
        private float nextCheckTime = 0.0f;

        private bool isSceneLoaded = false;

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("DestroyGameObjectWithPullCordForceChange mod loaded");

            // Initialize Harmony
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("com.mygamemods.destroywithpullcordforcechange");

            // Find a suitable method in PullCordForceChange for patching
            var methodToPatch = FindMethodToPatch(typeof(PullCordForceChange));

            if (methodToPatch != null)
            {
                // Apply the Harmony patch
                harmony.Patch(methodToPatch, new HarmonyLib.HarmonyMethod(typeof(DestroyGameObjectWithPullCordForceChange), nameof(Prefix)));
            }
            else
            {
                MelonLogger.Error("Unable to find a suitable method in PullCordForceChange for patching.");
            }
        }

        private static MethodInfo FindMethodToPatch(Type targetType)
        {
            // Search for a suitable method in the targetType for patching
            // For example, you could search for Awake, Start, OnTriggerEnter, etc.
            // Modify this logic based on the requirements of your mod
            MethodInfo[] methods = targetType.GetMethods();
            foreach (MethodInfo method in methods)
            {
                if (method.Name == "Awake" || method.Name == "Start" || method.Name == "OnTriggerEnter")
                {
                    MelonLogger.Msg($"Mod Started SIGMA OHIO GYATT");

                    return method;
                }
            }
            return null;
        }

        private static bool Prefix(PullCordForceChange __instance)
        {
            // Destroy the GameObject if it has the PullCordForceChange component
            string objectName = __instance.gameObject.name;
            GameObject.Destroy(__instance.gameObject);
            MelonLogger.Msg($"Found and destroyed GameObject: {objectName}");

            // Skip the original method
            return false;
        }

        public override void OnUpdate()
        {
            // This method can be left empty since the patching handles the behavior.
        }
    }
}
