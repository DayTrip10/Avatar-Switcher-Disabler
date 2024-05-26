using UnityEngine;
using MelonLoader;
using HarmonyLib;
using SLZ.Props;

namespace MyGameMods
{
    public class DestroyGameObjectWithPullCordForceChange : MonoBehaviour
    {
        private void Awake()
        {
            MelonLogger.Msg("DestroyGameObjectWithPullCordForceChange script attached and awake.");
        }

        private void Start()
        {
            MelonLogger.Msg("DestroyGameObjectWithPullCordForceChange script started.");
        }
    }

    [HarmonyPatch(typeof(UnityEngine.Object))]
    [HarmonyPatch("Instantiate", typeof(UnityEngine.Object), typeof(Vector3), typeof(Quaternion), typeof(Transform))]
    public static class InstantiatePatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ref UnityEngine.Object original, ref Vector3 position, ref Quaternion rotation, Transform parent)
        {
            // Check if the object being instantiated is a GameObject
            if (original is GameObject originalGameObject)
            {
                // Log the name of the GameObject being instantiated
                MelonLogger.Msg($"Attempting to instantiate GameObject: {originalGameObject.name}");

                // Check if the GameObject has a PullCordForceChange component
                if (originalGameObject.GetComponent<PullCordForceChange>() != null)
                {
                    // Log the destruction of the GameObject
                    MelonLogger.Msg($"Destroying GameObject with PullCordForceChange component: {originalGameObject.name}");

                    // Destroy the GameObject and prevent instantiation
                    UnityEngine.Object.Destroy(originalGameObject);
                    return false; // Skip the original Instantiate method
                }
            }

            // Continue with the original Instantiate method
            return true;
        }
    }

    public class MainMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("MainMod loaded. Creating GameObject with DestroyGameObjectWithPullCordForceChange script.");

            // Create a new GameObject in the scene
            GameObject newGameObject = new GameObject("DestroyGameObjectWithPullCordForceChangeObj");

            // Attach the DestroyGameObjectWithPullCordForceChange script to the new GameObject
            newGameObject.AddComponent<DestroyGameObjectWithPullCordForceChange>();

            // Optionally, set the GameObject to not be destroyed on load
            UnityEngine.Object.DontDestroyOnLoad(newGameObject);
        }
    }
}
