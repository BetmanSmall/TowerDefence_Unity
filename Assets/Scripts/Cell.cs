using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell /*: MonoBehaviour*/ { // пиздеЦЦЦ!!!
	public int gameX, layerY, gameZ;
	TileModel tileModel;
	GameObject gameObjectModel;

	public bool empty, terrain;

	public Cell(int gameX, int layerY, int gameZ, TileModel tileModel, Vector3 graphicCoordinates) { 
		this.gameX = gameX;
		this.layerY = layerY;
		this.gameZ = gameZ;
		this.tileModel = tileModel;
//		this.gameObjectModel = (GameObject)Instantiate(tileModel.modelObject, graphicCoordinates, Quaternion.identity, this.transform);
//		MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer> ();
//		foreach (Material material in meshRenderer.materials) {
//			//							Debug.Log("GameField::Start(); -- material:" + material);
//			Color color = material.color;
//			color.a = mapLayer.opacity; // It is not WOKR!=(
//			material.color = color;
//			//							Debug.Log("GameField::Start(); -- material.color:" + material.color);
//		}

		empty = true;
		terrain = false;
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
