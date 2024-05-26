using UnityEngine;
using MelonLoader;
using HarmonyLib;
using SLZ.Props;

namespace MyGameMods
{
    public class DestroyGameObjectWithPullCordForceChange : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Destroy GameObject With PullCordForceChange mod loaded");
        }
    }

    [HarmonyPatch]
    public static class InstantiatePatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(UnityEngine.Object), "Instantiate", typeof(UnityEngine.Object), typeof(Vector3), typeof(Quaternion), typeof(Transform))]
        public static bool Prefix(ref UnityEngine.Object original, ref Vector3 position, ref Quaternion rotation, Transform parent)
        {
            // Check if the object being instantiated has the PullCordForceChange component
            if (original is GameObject originalGameObject)
            {
                if (originalGameObject.GetComponent<PullCordForceChange>() != null)
                {
                    MelonLogger.Msg($"Destroying GameObject with PullCordForceChange component: {originalGameObject.name}");
                    UnityEngine.Object.Destroy(originalGameObject);
                    return false; // Skip the original method
                }
            }
            return true; // Continue with the original method
        }
    }
}
