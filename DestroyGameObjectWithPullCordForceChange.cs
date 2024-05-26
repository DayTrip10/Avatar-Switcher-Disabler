using System;
using UnityEngine;
using MelonLoader;
using HarmonyLib;
using SLZ.Props;

namespace MyGameMods
{
    public class Daytrip : MelonMod
    {
        private HarmonyLib.Harmony harmony;

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Daytrip mod loaded");
            harmony = new HarmonyLib.Harmony("com.daytrip.DestroyGameObjectWithPullCordForceChange");
            harmony.PatchAll(typeof(Daytrip).Assembly);
        }

        // No need for OnUpdate if using Harmony for patching
    }

    [HarmonyPatch(typeof(UnityEngine.Object), "Instantiate", typeof(UnityEngine.Object), typeof(Vector3), typeof(Quaternion), typeof(Transform))]
    public static class InstantiatePatch
    {
        static void Prefix(ref UnityEngine.Object original, ref Vector3 position, ref Quaternion rotation, Transform parent)
        {
            // Check if the object being instantiated has the PullCordForceChange component
            if (original is GameObject originalGameObject)
            {
                if (originalGameObject.GetComponent<PullCordForceChange>() != null)
                {
                    MelonLogger.Msg($"Destroying GameObject with PullCordForceChange component: {originalGameObject.name}");
                    UnityEngine.Object.Destroy(originalGameObject);
                }
            }
        }
    }
}
