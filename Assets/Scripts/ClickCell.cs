using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCell : MonoBehaviour {
	public Color colorMaterial;

	void Start(){
		colorMaterial = GetComponent<MeshRenderer> ().material.color;
	}
	public void MouseEnter()
	{
		GetComponent<MeshRenderer> ().material.color = new Color (1, 0, 0);
	}

	public void MouseExit()
	{
		GetComponent<MeshRenderer> ().material.color = colorMaterial;
	}
}
