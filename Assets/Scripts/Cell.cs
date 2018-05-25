using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour { // пиздеЦЦЦ!!!
	public int gameX;
	public int layerY;
	public int gameZ;
	TileModel tileModel;
	GameObject gameObjectModel;
	Vector3 graphicCoordinates;

	public bool empty;
	public bool terrain;

	public Cell() {

	}

	public Cell(int gameX, int layerY, int gameZ, TileModel tileModel, Vector3 graphicCoordinates) {
		setBasicValues(gameX, layerY, gameZ, tileModel, graphicCoordinates);
//		this.gameObjectModel = (GameObject)Instantiate(tileModel.modelObject, graphicCoordinates, Quaternion.identity, this.transform);
//		MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer> ();
//		foreach (Material material in meshRenderer.materials) {
//			//							Debug.Log("GameField::Start(); -- material:" + material);
//			Color color = material.color;
//			color.a = mapLayer.opacity; // It is not WOKR!=(
//			material.color = color;
//			Debug.Log("GameField::Start(); -- material.color:" + material.color);
//		}
	}

	public void setBasicValues(int gameX, int layerY, int gameZ, TileModel tileModel, Vector3 graphicCoordinates) {
		this.gameX = gameX;
		this.layerY = layerY;
		this.gameZ = gameZ;
		this.tileModel = tileModel;
		this.graphicCoordinates = graphicCoordinates;
	}

	public bool setTerrain() {
		if (empty) {
			terrain = true;
			empty = false;
			return true;
		}
		return false;
	}
}
