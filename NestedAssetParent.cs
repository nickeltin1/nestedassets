using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(NestedAssetParent), menuName = "Test/" + nameof(NestedAssetParent))]
public class NestedAssetParent : ScriptableObject
{
    public List<NestedAsset> childs;
}
