using UnityEngine;
using System.Collections;


public class AnimatedTexture : MonoBehaviour {

	public int frameRate = 30;
	public GameObject jointGameObj;

	public bool UseXComponent = true;
	public string xMaterialName;
    public Texture2D[] xTextures;

	public bool UseYComponent = false;
	public string yMaterialName;
	public Texture2D[] yTextures;

	public bool UseZComponent = false;
	public string zMaterialName;
	public Texture2D[] zTextures;

	private float timer = 0;

	private int xCurrentTexture = 0;
	private int xPreviousTexture = 0;
	private int xMaterialNumber = 0;

	private int yCurrentTexture = 0;
	private int yPreviousTexture = 0;
	private int yMaterialNumber = 0;

	private int zCurrentTexture = 0;
	private int zPreviousTexture = 0;
	private int zMaterialNumber = 0;

	void Start () { enabled = FindAnimatedTexture(); }
	void Update () { SetAnimatedTextureOffset(); }

	bool FindAnimatedTexture(){
		// Make sure a renderer exists on this object
		if(!GetComponent<Renderer>()){

			Debug.LogError ("Error in AnimatedTextureOffset: Could not find a renderer on this object.");
			return false;
		}

		// Determine offset ratio by querying the main texture	
		Material[] materials = GetComponent<Renderer> ().materials;

		for (int i = 0; i < materials.Length; i++)
		{
			Material material = materials[i];

			string matName = material.name;

			matName = matName.Replace(" (Instance)", "");

			if (UseXComponent && xMaterialName.CompareTo(matName) == 0)
			{
				xMaterialNumber = i;
			}

			if (UseYComponent && yMaterialName.CompareTo(matName) == 0)
			{
				yMaterialNumber = i;
			}

			if (UseYComponent && zMaterialName.CompareTo(matName) == 0)
			{
				zMaterialNumber = i;
			}
		}

		if(jointGameObj == null){
			//Debug.LogError ("Error in AnimatedTextureOffset: Could not find ");
			return false;
		}

		return true;
	}
	
	void SetAnimatedTextureOffset(){
		// This timer limits the script to only update a limited amount of times per
		// second, so it doesn't catch interim blended frames between two keys
		if(timer <= 0){

			if (UseXComponent)
			{
				xCurrentTexture = ((int)jointGameObj.transform.localScale.x /*- 1*/);

				if (xCurrentTexture != xPreviousTexture && xCurrentTexture >= 0 && xCurrentTexture < xTextures.Length)
				{
					Renderer rend = GetComponent<Renderer>();
					rend.materials[xMaterialNumber].SetTexture("_BaseMap", xTextures[xCurrentTexture]);
					xPreviousTexture = xCurrentTexture;
				}
			}

			if (UseYComponent)
			{
				yCurrentTexture = (int)jointGameObj.transform.localScale.y /*- 1*/;

				if (yCurrentTexture != yPreviousTexture && yCurrentTexture >= 0 && yCurrentTexture < yTextures.Length)
				{
					Renderer rend = GetComponent<Renderer>();
					rend.materials[yMaterialNumber].SetTexture("_BaseMap", yTextures[yCurrentTexture]);
					yPreviousTexture = yCurrentTexture;
				}
			}

			if (UseZComponent)
			{
				zCurrentTexture = (int)jointGameObj.transform.localScale.z /*- 1*/;

				if (zCurrentTexture != zPreviousTexture && zCurrentTexture >= 0 && zCurrentTexture < zTextures.Length)
				{
					Renderer rend = GetComponent<Renderer>();
					rend.materials[zMaterialNumber].SetTexture("_BaseMap", zTextures[zCurrentTexture]);
					zPreviousTexture = zCurrentTexture;
				}
			}

			timer = (1/(float)frameRate);
		}
		else
        {
			timer -= Time.deltaTime;
		}
	}
}
