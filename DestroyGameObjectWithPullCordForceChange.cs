using UnityEngine;
using MelonLoader;
using HarmonyLib;
using SLZ.Props;

namespace MyGameMods
{
    public class Daytrip : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Daytrip mod loaded");
            var harmony = new Harmony("com.daytrip.DestroyGameObjectWithPullCordForceChange");
            harmony.PatchAll();
        }

        public override void OnApplicationQuit()
        {
            var harmony = new Harmony("com.daytrip.DestroyGameObjectWithPullCordForceChange");
            harmony.UnpatchAll();
        }
    }

    [HarmonyPatch(typeof(UnityEngine.Object))]
    public class InstantiatePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch("Instantiate", typeof(UnityEngine.Object), typeof(Vector3), typeof(Quaternion), typeof(Transform))]
        public static void Prefix(ref UnityEngine.Object original, ref Vector3 position, ref Quaternion rotation, Transform parent)
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
