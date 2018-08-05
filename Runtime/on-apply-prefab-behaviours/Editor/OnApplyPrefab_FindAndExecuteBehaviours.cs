using System.Collections.Generic;
using BeatThat.GetComponentsExt;
using BeatThat.Pools;
using UnityEditor;
using UnityEngine;

namespace BeatThat.OnApplyPrefabBehaviours
{
    /// <summary>
    /// Enables you to execute custom behaviours on prefabs when they've been applied.
    /// 
    /// When present, this class will bind to events for PrefabUtility.prefabInstanceUpdated
    /// and then find and execute all <code>OnApplyPrefabBehaviour</code>s on the prefab (the prefab rather than the instance).
    /// </summary>
    [InitializeOnLoad]
    internal static class OnApplyPrefab_FindAndExecuteBehaviours
    {
        static OnApplyPrefab_FindAndExecuteBehaviours()
        {
            PrefabUtility.prefabInstanceUpdated += OnPrefabInstanceUpdate;
        }

        private static List<GameObject> instancesUpdating = new List<GameObject>();

        static void OnPrefabInstanceUpdate(GameObject prefabInstance)
        {
            if (instancesUpdating.Contains(prefabInstance))
            {
                return;
            }

            instancesUpdating.Add(prefabInstance);

            var prefab =
#if UNITY_2018_2_OR_NEWER
                PrefabUtility.GetCorrespondingObjectFromSource(prefabInstance) as GameObject;
#else
                PrefabUtility.GetPrefabParent(prefabInstance) as GameObject;
#endif

            FindAndExecuteBehaviours_DepthFirst (prefab.gameObject, prefabInstance, prefab);

			instancesUpdating.Remove (prefabInstance);

			AssetDatabase.SaveAssets ();
		}

		private static void FindAndExecuteBehaviours_DepthFirst(GameObject cur, GameObject prefabInstance, GameObject prefab)
		{
			foreach (Transform child in cur.transform) {
				FindAndExecuteBehaviours_DepthFirst (child.gameObject, prefabInstance, prefab);
			}

			using(var bComps = ListPool<OnApplyPrefabBehaviour>.Get()) {
				cur.GetComponents<OnApplyPrefabBehaviour> (bComps, true);

				foreach(var b in bComps) {
					b.OnApplyPrefab_BeforeAllSiblings(prefabInstance, prefab);
				}

				foreach(var b in bComps) {
					b.OnApplyPrefab(prefabInstance, prefab);
				}

				foreach(var b in bComps) {
					b.OnApplyPrefab_AfterAllSiblings(prefabInstance, prefab);
				}
			}
		}
	}
}

