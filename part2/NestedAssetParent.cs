using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(NestedAssetParent), menuName = "Test/" + nameof(NestedAssetParent))]
public class NestedAssetParent : ScriptableObject
{
    [SerializeField] private List<NestedAsset> _childs;

#if UNITY_EDITOR
    public static string childs_prop_name => nameof(_childs);
#endif
}
