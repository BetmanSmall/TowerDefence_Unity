using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour {
	public string mapPath = "maps/testMap";

	public int sizeFieldX, sizeFieldZ;
	public float sizeCellX=3f, sizeCellY=0.3f, sizeCellZ=3f; // need change, load from map
	public Cell[,] field;

	// Use this for initialization
	void Start() {
		Debug.Log("GameField::Start(); -- Start!");
		Map map = new MapLoader().loadMap(mapPath);

		sizeFieldX = int.Parse(map.properties ["width"]);
		sizeFieldZ = int.Parse(map.properties ["height"]);

		createField (sizeFieldX, sizeFieldZ, map.mapLayers);
		Debug.Log("GameField::Start(); -- End!");
	}

	private void createField(int sizeFieldX, int sizeFieldZ, Dictionary<int, MapLayer> mapLayers) {
	Debug.Log("GameField::createField(" + sizeFieldX + ", " + sizeFieldZ + ", " + mapLayers + "); -- field:" + field);
		if (field == null) {
			field = new Cell[sizeFieldX, sizeFieldZ];
			for (int layerY = 0; layerY < mapLayers.Count; layerY++) {
				MapLayer mapLayer = mapLayers [layerY];
//			foreach (MapLayer mapLayer in mapLayers.Values) {
				Debug.Log ("GameField::Start(); -- mapLayer.opacity:" + mapLayer.opacity);
				for (int z = 0; z < sizeFieldZ; z++) {
					for (int x = 0; x < sizeFieldX; x++) {
						TileModel tileModel = mapLayer.tileModels [x, z];
						if (tileModel != null) {
							Vector3 graphicCoordinates = new Vector3 (x * sizeCellX + sizeCellX, layerY * sizeCellY, z * sizeCellZ + sizeCellZ); // все тут нужно понять
							GameObject gameObject = (GameObject)Instantiate(tileModel.modelObject, graphicCoordinates, Quaternion.identity, this.transform); 
							gameObject.AddComponent<Cell>();  ///УРЯ!
							//Cell cell = new Cell (x, layerY, z, tileModel, graphicCoordinates);
							Cell cell = gameObject.GetComponent<Cell>();
							cell.setBasicValues(x, layerY, z, tileModel, graphicCoordinates);
							field[x, z] = cell;

							MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer> (); // Дикие не понятки со всем этим!
							if (mapLayer.opacity == 0f) {
								meshRenderer.enabled = false;
							} else {
								foreach (Material material in meshRenderer.materials) {
	//							Debug.Log("GameField::Start(); -- material:" + material);
									Color color = material.color;    
									/// Прозрачность
									color.a = mapLayer.opacity; // It is not WOKR!=(
									material.color = color;
	//							Debug.Log("GameField::Start(); -- material.color:" + material.color);
								}
							}
							gameObject.transform.SetParent (this.transform);
						}
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
//		}
	}
}
