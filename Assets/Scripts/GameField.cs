using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour {
	public string mapPath = "maps/testMap";

	public int sizeFieldX, sizeFieldZ;
	public float sizeCellX=3f, sizeCellY=0f, sizeCellZ=3f;
//	public Cell[,] field;

	// Use this for initialization
	void Start() {
		Debug.Log("GameField::Start(); -- Start!");
		Map map = new MapLoader().loadMap(mapPath);

		sizeFieldX = int.Parse(map.properties ["width"]);
		sizeFieldZ = int.Parse(map.properties ["height"]);

		foreach (MapLayer mapLayer in map.mapLayers.Values) {
			for (int z = 0; z < sizeFieldZ; z++) {
				for (int x = 0; x < sizeFieldX; x++) {
					Object modelObject = mapLayer.tileModels [x, z].modelObject;
					if(modelObject != null) {
						GameObject gameObject = (GameObject)Instantiate (modelObject, new Vector3 (x*sizeCellX+sizeCellX, 0, z*sizeCellZ+sizeCellZ), Quaternion.identity);
	//					gameObject.GetComponent<Renderer> ().material.color.a = mapLayer.opacity;
						gameObject.transform.SetParent (this.transform);
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update() {
//		if (Input.GetMouseButtonDown(0)) {
//			print ("GameField::Update(); -- Input.GetMouseButtonDown(0);");
//			print ("GameField::Update(); -- Input.mousePosition" + Input.mousePosition);
//			Transform[] allTransforms = gameObject.GetComponentsInChildren<Transform>();
//			foreach(Transform childObjects in allTransforms){
//				if(gameObject.transform.IsChildOf(childObjects.transform) == false)
//					Destroy(childObjects.gameObject);
//			}
//			GenerateField();
//		}
//		if (Input.GetButtonDown("Fire1")) {
//			print ("GameField::Update(); -- Input.GetButtonDown(Fire1);");
//			print ("GameField::Update(); -- Input.mousePosition" + Input.mousePosition);
//			Camera camera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
//			print ("GameField::Update(); -- camera:" + camera);
//			Vector3 worldPos = camera.ScreenToWorldPoint (Input.mousePosition);
//			print ("GameField::Update(); -- worldPos:" + worldPos);
//
//		}
	}
}
