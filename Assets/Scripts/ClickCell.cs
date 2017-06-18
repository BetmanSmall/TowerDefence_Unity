using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCell : MonoBehaviour {
	public Color textureMaterial;
	public Color texture;

	void Start() {
		textureMaterial = GetComponentInChildren<MeshRenderer> ().materials[1].color;
	}

	public void MouseEnter() {
		GetComponentInChildren<MeshRenderer> ().materials[1].color = texture;
	}

	public void MouseExit() {
		GetComponentInChildren<MeshRenderer> ().materials[1].color = textureMaterial;
	}
}
