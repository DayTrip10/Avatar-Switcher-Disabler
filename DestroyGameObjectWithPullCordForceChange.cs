using System;
using System.Reflection;
using UnityEngine;
using SLZ.Props;
using MelonLoader;
using HarmonyLib;

namespace MyGameMods
{
    public class DestroyGameObjectWithPullCordForceChange : MelonMod
    {
        private float checkInterval = 5.0f; // Check every 5 seconds
        private float nextCheckTime = 0.0f;

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("DestroyGameObjectWithPullCordForceChange mod loaded");
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony("com.daytrip.DestroyGameObjectWithPullCordForceChange");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public override void OnUpdate()
        {
            if (Time.time >= nextCheckTime)
            {
                nextCheckTime = Time.time + checkInterval;
                PullCordForceChange[] pullCordForceChanges = GameObject.FindObjectsOfType<PullCordForceChange>();
                foreach (PullCordForceChange pullCordForceChange in pullCordForceChanges)
                {
                    string objectName = pullCordForceChange.gameObject.name;
                    GameObject.Destroy(pullCordForceChange.gameObject);
                    MelonLogger.Msg($"Found and destroyed GameObject: {objectName}");
                }
            }
        }
    }

    [HarmonyPatch(typeof(PullCordForceChange))]
    [HarmonyPatch("Pull")]
    public static class ForcePullPatch
    {
        static void Prefix(PullCordForceChange __instance)
        {
            MelonLogger.Msg("Pull method called, executing prefix...");
        }

        static void Postfix(PullCordForceChange __instance)
        {
            MelonLogger.Msg("Pull method executed, executing postfix...");
        }
    }
}
