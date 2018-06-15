using UnityEngine;

namespace BeatThat.OnApplyPrefabBehaviours
{
    // TODO: probably will need a way to control ordering, e.g. a Priority property or attribute.

    /// <summary>
    /// Implement this interface on a component to have a behaviour execute on a prefab when that prefab is applied.
    /// 
    /// The actual execution is done by @see FindAndExecuteBehaviours_DepthFirst,
    /// which listens for unity's <code>UnityEditor.PrefabUtility.prefabInstanceUpdated</code> event 
    /// (https://docs.unity3d.com/ScriptReference/PrefabUtility-prefabInstanceUpdated.html).
    /// 
    /// An important note: prefabInstanceUpdated is called AFTER the prefab has been updated. 
    /// There is no unity event at the time of this writing that cleanly tells you a prefab will updated BEFORE the update occurs.
    /// </summary>
    public interface OnApplyPrefabBehaviour
	{
		/// <summary>
		/// Called by <code>FindAndExecuteBehaviours_DepthFirst</code> on all components of a prefab that implement this behaviour.
		/// 
		/// For a given GameObject in the applied prefab's hierarchy, this is called *BEFORE* OnApplyPrefab is call on any of the components.
		/// 
		/// </summary>
		/// <param name="prefabInstance">the root prefab instance that was passed via <code>PrefabUtility.prefabInstanceUpdated</code></param>
		/// <param name="prefab">the resolved prefab from the prefan instance.</param>
		void OnApplyPrefab_BeforeAllSiblings(GameObject prefabInstance, GameObject prefab);

		/// <summary>
		/// called by <code>FindAndExecuteBehaviours_DepthFirst</code> on all components of a prefab that implement this behaviour.
		/// </summary>
		/// <param name="prefabInstance">the root prefab instance that was passed via <code>PrefabUtility.prefabInstanceUpdated</code></param>
		/// <param name="prefab">the resolved prefab from the prefan instance.</param>
		void OnApplyPrefab(GameObject prefabInstance, GameObject prefab);

		/// <summary>
		/// Called by <code>FindAndExecuteBehaviours_DepthFirst</code> on all components of a prefab that implement this behaviour.
		/// 
		/// For a given GameObject in the applied prefab's hierarchy, this is called *AFTER* OnApplyPrefab is call on all of the components.
		/// 
		/// </summary>
		/// <param name="prefabInstance">the root prefab instance that was passed via <code>PrefabUtility.prefabInstanceUpdated</code></param>
		/// <param name="prefab">the resolved prefab from the prefan instance.</param>
		void OnApplyPrefab_AfterAllSiblings(GameObject prefabInstance, GameObject prefab);
	}
}
