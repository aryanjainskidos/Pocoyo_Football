using UnityEngine;
using System.Collections;


public class AnimatedBlendShape : MonoBehaviour {

	public int frameRate = 30;
	public GameObject jointGameObj;

	public bool UseXComponent = true;
	public int xBlendShapeIndex = 0;
	public bool UseYComponent = false;
	public int yBlendShapeIndex = 0;
	public bool UseZComponent = false;
	public int zBlendShapeIndex = 0;


	private float timer = 0;
	private SkinnedMeshRenderer blendShapeObj;

	void Start () { enabled = FindBlendShapes(); }
	void Update () { SetBlendShapeValue(); }

	bool FindBlendShapes(){
		blendShapeObj = GetComponent<SkinnedMeshRenderer>();

		// Make sure a renderer exists on this object
		if (blendShapeObj == null){

			Debug.LogError ("Error in AnimatedBlendShape: Could not find a SkinnedMeshRenderer on this object.");
			return false;
		}
		
		if(jointGameObj == null){
			Debug.LogError ("Error in AnimatedBlendShape: Missing Joint ");
			return false;
		}

		return true;
	}
	
	void SetBlendShapeValue(){
		// This timer limits the script to only update a limited amount of times per
		// second, so it doesn't catch interim blended frames between two keys
		if(timer <= 0){

			if (UseXComponent)
			{
				float xBSValue = jointGameObj.transform.localScale.x * 100f;
				blendShapeObj.SetBlendShapeWeight(xBlendShapeIndex, xBSValue);
			}

			if (UseYComponent)
			{
				float yBSValue = jointGameObj.transform.localScale.y * 100f;
				blendShapeObj.SetBlendShapeWeight(yBlendShapeIndex, yBSValue);
			}

			if (UseZComponent)
			{
				float zBSValue = jointGameObj.transform.localScale.z * 100f;
				blendShapeObj.SetBlendShapeWeight(zBlendShapeIndex, zBSValue);
			}

			timer = (1/(float)frameRate);
		}
		else
        {
			timer -= Time.deltaTime;
		}
	}
}
