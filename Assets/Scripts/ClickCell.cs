using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCell : MonoBehaviour {
	public Color textureMaterial;
	public Color texture;

	void Start(){
		textureMaterial = GetComponentInChildren<MeshRenderer> ().materials[1].color;

	}
	public void MouseEnter()
	{
		//GetComponent<MeshRenderer> ().material.color. = new Color (1, 0, 0);
		GetComponentInChildren<MeshRenderer> ().materials[1].color = texture;
		//Debug.Log(GetComponentInChildren<MeshRenderer> ().sharedMaterials[1]);

	}

	public void MouseExit()
	{
		GetComponentInChildren<MeshRenderer> ().materials[1].color = textureMaterial;
	}
}
