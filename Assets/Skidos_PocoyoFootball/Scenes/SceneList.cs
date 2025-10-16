using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "SceneReferenceDatabase", menuName = "Game/Scene Reference Database")]
public class SceneList : ScriptableObject
{
    public List<AssetReference> sceneReferences;
}