using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppSystem.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using MelonLoader;
using System.IO;


namespace SkyCoop
{
    internal class AssetManager
    {
        // The modern way to get the Mods folder on ML 0.7+
        public static string s_MainBundlePath = Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "SkyCoop", "skycoop_assets_all.bundle");
        public static AssetBundle s_MainBundle = null;
        public static GameObject s_PistolBulletPrefab = null;
        public static GameObject s_RevolverBulletPrefab = null;

        public static void PreloadMainBundle()
        {
            if(s_MainBundle == null)
            {
                // Try to load the physical bundle file
                s_MainBundle = AssetBundle.LoadFromFile(s_MainBundlePath);

                if (s_MainBundle == null)
                {
                    // Fallback check: try find it just as 'skycoop' without extension
                    Logger.Log(ConsoleColor.Yellow, $"Failed to load the Main Asset bundle normally. Trying to find without extension at: {s_MainBundlePath}");
                    string fallback = Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "SkyCoop", "skycoop_assets_all");
                    s_MainBundle = AssetBundle.LoadFromFile(fallback);
                }

                if (s_MainBundle == null)
                {
                    Logger.Log(ConsoleColor.Red, $"Failed to load the Main Asset bundle at: {s_MainBundlePath}");
                }
                else
                {
                    Logger.Log(ConsoleColor.Blue, "Main Asset Bundle is loaded.");
                }
            }

            string catalogPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "SkyCoop", "catalog.json");
            
            if (File.Exists(catalogPath))
            {
                Addressables.LoadContentCatalogAsync(catalogPath).WaitForCompletion();
                Logger.Log(ConsoleColor.Green, "Addressables Catalog linked! UI should now work.");
            }
        }

        public static T GetAssetFromGame<T>(string AssetName) where T : UnityEngine.Object
        {
            T Asset = Addressables.LoadAssetAsync<T>(AssetName).WaitForCompletion();
            if (Asset == null)
            {
                Logger.Log(System.ConsoleColor.Yellow, "Can't load "+AssetName+" from game assets!");
                Logger.Log(System.ConsoleColor.DarkMagenta, "Attempting to use the old method..");
                Asset = GetAssetFromResources_OLD<T>(AssetName);
                if(Asset == null)
                {
                    Logger.Log(System.ConsoleColor.Yellow, "Can't load " + AssetName + " using the old method either..");
                } else {
                    Logger.Log(System.ConsoleColor.DarkMagenta, AssetName+" successfully loaded using the old method!");
                }
            }
            return Asset;
        }

        public static void SetupLinuxPathFix()
        {
            // Explicitly define the Func to satisfy the compiler
            Func<IResourceLocation, string> transformFunc = (IResourceLocation location) =>
            {
                // If the catalog is looking for a bundle file...
                if (location.InternalId.EndsWith(".bundle"))
                {
                    // Extract just the filename (e.g., skycoop_assets_all.bundle)
                    string fileName = Path.GetFileName(location.InternalId);
                    
                    // Build the absolute Linux path
                    string newPath = Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "SkyCoop", "AssetBundles", fileName);
                    
                    // Log it so you can see if it's working in the console
                    Logger.Log(ConsoleColor.DarkCyan, $"[Linux Path Fix] Mapping {fileName} -> {newPath}");
                    
                    return newPath;
                }
                return location.InternalId;
            };

            // Assign the concrete delegate to the Addressables system
            Addressables.InternalIdTransformFunc = transformFunc;
        }

        public static void BogusIt(GameObject Obj)
        {
            if (Obj == null) return;

            foreach (Component Com in Obj.GetComponents<Component>())
            {
                if (Com == null) continue;

                // Use 'is' to catch the base types.
                // This avoids the PhysicMaterial ambiguity and string comparisons.
                if (Com is Transform
                    || Com is MeshFilter
                    || Com is MeshRenderer
                    || Com is SkinnedMeshRenderer
                    || Com is LODGroup
                    || Com is Rigidbody
                    || Com is AudioSource
                    || Com is Collider) // This automatically covers Box, Sphere, Capsule, and Mesh Colliders
                {
                    continue;
                }

                UnityEngine.Object.Destroy(Com);
            }
        }

        public static GameObject CreateLocalizedBogusGear(string GearName, out string LocalizedName, Transform parent = null)
        {
            string LN = "Invalid";
            GameObject Prefab = GetAssetFromGame<GameObject>(GearName);
            if (Prefab)
            {
                GameObject GearObject = UnityEngine.Object.Instantiate(Prefab, parent);
                if (GearObject)
                {
                    GearObject.name = GearName;

                    GearItem gi = GearObject.GetComponent<GearItem>();
                    if (gi)
                    {
                        LN = gi.DisplayName;
                    }
                    foreach (Component Com in GearObject.GetComponents<Component>())
                    {
                        string ComName = Com.GetIl2CppType().Name;
                        // if (ComName != Il2CppType.Of<BoxCollider>().Name
                        //     && ComName != Il2CppType.Of<SphereCollider>().Name
                        //     && ComName != Il2CppType.Of<CapsuleCollider>().Name
                        //     && ComName != Il2CppType.Of<MeshCollider>().Name
                        //     && ComName != Il2CppType.Of<PhysicMaterial>().Name
                        //     && ComName != Il2CppType.Of<MeshFilter>().Name
                        //     && ComName != Il2CppType.Of<LODGroup>().Name
                        //     && ComName != Il2CppType.Of<Transform>().Name
                        //     && ComName != Il2CppType.Of<Rigidbody>().Name
                        //     && ComName != Il2CppType.Of<MeshRenderer>().Name
                        //     && ComName != Il2CppType.Of<SkinnedMeshRenderer>().Name
                        //     && ComName != Il2CppType.Of<AudioSource>().Name)
                        // {
                        //     UnityEngine.Object.Destroy(Com);
                        // }
                        if (Com == null) continue;

                        // Use 'is' to catch the base types.
                        // This avoids the PhysicMaterial ambiguity and string comparisons.
                        if (Com is Transform
                            || Com is MeshFilter
                            || Com is MeshRenderer
                            || Com is SkinnedMeshRenderer
                            || Com is LODGroup
                            || Com is Rigidbody
                            || Com is AudioSource
                            || Com is Collider) // This automatically covers Box, Sphere, Capsule, and Mesh Colliders
                        {
                            continue;
                        }

                        UnityEngine.Object.Destroy(Com);
                    }
                    LocalizedName = LN;
                    return GearObject;
                }
                else
                {
                    Logger.Log(ConsoleColor.Red, "Can't instantiate " + Prefab.name);
                }
            }
            LocalizedName = LN;
            return null;
        }

        public static GameObject CreateBogusGear(string GearName, Transform parent = null)
        {
            GameObject Prefab = GetAssetFromGame<GameObject>(GearName);
            if (Prefab)
            {
                GameObject GearObject = UnityEngine.Object.Instantiate(Prefab, parent);
                if (GearObject)
                {
                    GearObject.name = GearName;
                    foreach (Component Com in GearObject.GetComponents<Component>())
                    {
                        string ComName = Com.GetIl2CppType().Name;
                        if (Com == null) continue;

                        // Use 'is' to catch the base types.
                        // This avoids the PhysicMaterial ambiguity and string comparisons.
                        if (Com is Transform
                            || Com is MeshFilter
                            || Com is MeshRenderer
                            || Com is SkinnedMeshRenderer
                            || Com is LODGroup
                            || Com is Rigidbody
                            || Com is AudioSource
                            || Com is Collider) // This automatically covers Box, Sphere, Capsule, and Mesh Colliders
                        {
                            continue;
                        }

                        UnityEngine.Object.Destroy(Com);
                    }
                    return GearObject;
                } else
                {
                    Logger.Log(ConsoleColor.Red, "Can't instantiate " + Prefab.name);
                }
            }

            return null;
        }

        public static T GetAssetFromResources_OLD<T>(string AssetName) where T : UnityEngine.Object
        {
            UnityEngine.Object Asset = Resources.Load(AssetName);
            if (Asset)
            {
                return Resources.Load(AssetName).Cast<T>();
            }
            return null;
        }

        // This using casting, because we can use it to load textures, audio clips and etc, not just prefabs.
        public static T GetAssetFromBundle<T>(string AssetName) where T : UnityEngine.Object
        {
            if (s_MainBundle == null)
            {
                Logger.Log(ConsoleColor.Red, "Can't load "+AssetName+" because bundle is missing!");
                return null;
            }
            return s_MainBundle.LoadAsset<T>(AssetName);
        }

        public static void DumpAddressablesContent()
        {
            foreach (var item in Addressables.ResourceLocators.ToList())
            {
                foreach (var key in item.Keys.ToList())
                {
                    Logger.Log(ConsoleColor.Magenta, "[Addressables][LocatorId=" + item.LocatorId + "] " + key.ToString());
                }
            }
        }
        public static void DumpPrefabsList()
        {
            foreach (var item in Resources.LoadAll<UnityEngine.Object>(""))
            {
                Logger.Log(ConsoleColor.Magenta, "[Resources] " + item.name);
            }
        }

        public static void DumpLocalizationKeysList()
        {
            foreach (StringTableData Data in Localization.s_CurrentLanguageStringTable.m_DataFiles)
            {
                foreach (StringTableData.Entry Entry in Data.m_Entries)
                {
                    Logger.Log(ConsoleColor.Magenta, $"{Data.name} Key {Entry.m_Key}");
                }
            }
        }

        public static void RegisterIlegalGearsCommand()
        {
            uConsole.RegisterCommand("give", new Action(GiveIlegalGear));
        }

        public static void GiveIlegalGear()
        {
            GameObject reference = GetAssetFromGame<GameObject>(uConsole.GetString());
            if (reference)
            {
                GameObject GearObject = UnityEngine.Object.Instantiate(reference);
                GearItem item = GearObject.GetComponent<GearItem>();
                if(item != null)
                {
                    item.CompleteSpawnFromCONSOLE();
                    GameManager.GetInventoryComponent().AddGear(item);
                }

            }
        }
    }
}
