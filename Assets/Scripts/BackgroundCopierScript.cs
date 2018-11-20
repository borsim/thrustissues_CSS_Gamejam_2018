using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BackgroundCopierScript : MonoBehaviour {

	
	void Start () {
		Material backgroundMaterial = (GameObject.Find("Background").GetComponent(typeof(Renderer)) as Renderer).material as Material;
		Texture2D clone = Instantiate(backgroundMaterial.mainTexture as Texture2D);
		backgroundMaterial.mainTexture = clone;
	}
}
