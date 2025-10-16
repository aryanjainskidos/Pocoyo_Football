#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "PocoyoSetupData", menuName = "Pocoyo Setup Data", order = 0)]
public class PocoyoSetupData : ScriptableObject
{
    public enum UseCompionentType
    {
        UCT_X,
        UCT_Y,
        UCT_Z
    };

    [System.Serializable]
    public struct AnimatedTextureData
    {
        public string GameObjectName;
        public string JointName;
        public UseCompionentType UseComponent;
        public string materialName;
        public List<Texture2D> textures;
    }

    public int FrameRate = 30;
    public List<AnimatedTextureData> setup;

    [ContextMenu("Setup Prefab")]
    public void ApplyToPrefab()
    {
        string prefabName = EditorUtility.OpenFilePanel("Select the Pocoyo Prefab", "Assets", "prefab");

        if(!string.IsNullOrEmpty(prefabName))
        {
            GameObject pocoyo = PrefabUtility.LoadPrefabContents(prefabName);

            foreach(AnimatedTextureData atd in setup)
            {
                GameObject meshGameObject = null;
                GameObject joinGameObject = null;

                Transform[] meshChildrens = pocoyo.transform.GetComponentsInChildren<Transform>();
                foreach(Transform child in meshChildrens)
                {
                    if (child.name == atd.GameObjectName)
                        meshGameObject = child.gameObject;
                    if (child.name == atd.JointName)
                        joinGameObject = child.gameObject;
                }

                if (meshGameObject != null && joinGameObject != null)
                {
                    AnimatedTexture at = meshGameObject.AddComponent<AnimatedTexture>();

                    at.frameRate = FrameRate;
                    at.jointGameObj = joinGameObject;

                    switch (atd.UseComponent)
                    {
                        case UseCompionentType.UCT_X:
                            {
                                at.UseXComponent = true;
                                at.xMaterialName = atd.materialName;
                                at.xTextures = atd.textures.ToArray();
                            }
                            break;
                        case UseCompionentType.UCT_Y:
                            {
                                at.UseYComponent = true;
                                at.yMaterialName = atd.materialName;
                                at.yTextures = atd.textures.ToArray();
                            }
                            break;
                        case UseCompionentType.UCT_Z:
                            {
                                at.UseZComponent = true;
                                at.zMaterialName = atd.materialName;
                                at.zTextures = atd.textures.ToArray();
                            }
                            break;
                    }
                }
            }

            PrefabUtility.SaveAsPrefabAsset(pocoyo, prefabName);
            PrefabUtility.UnloadPrefabContents(pocoyo);
        }
    }
}


#endif