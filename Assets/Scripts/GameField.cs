using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class GameField : MonoBehaviour {
	private Hashtable mapProperties;
	private string modelsPath;
	private Dictionary<int, GameObject> modelsDictionary;
	public GameObject[] objectsTerrain;

	public int sizeFieldX, sizeFieldZ;
	private float sizeCellX=3f, sizeCellY=0f, sizeCellZ=3f;
	private Cell[,] field;

	// Use this for initialization
	void Start() {
		Debug.Log("GameField::Start(); -- Start!");
		mapProperties = new Hashtable ();
//		string modelsPath = "maps/textures/fieldModels";
		modelsDictionary = new Dictionary<int, GameObject> ();
//		objectsTerrain = Resources.LoadAll<GameObject>(modelsPath);
//		Debug.Log ("GameField::Start(); -- objectsTerrain.Length:" + objectsTerrain.Length);
//
//		GameObject mainTerrain = (GameObject)objectsTerrain[0] as GameObject;
////		Debug.Log ("GameField::Start(); -- block1:" + block1);
////		Mesh mesh = block1.GetComponentInChildren<MeshFilter>().mesh;
////		Debug.Log ("GameField::Start(); -- mesh:" + mesh);
////		sizeCellX = mesh.bounds.size.x;
////		sizeCellY = mesh.bounds.size.y;
////		sizeCellZ = mesh.bounds.size.z;
////		Debug.Log ("GameField::Start(); -- sizeCellX:" + sizeCellX + " sizeCellY:" + sizeCellY + " sizeCellZ:" + sizeCellZ);
//
//		int x = 0, z = 0;
////		foreach(Object tmpObject in objectsTerrain) {
//		for (int n = 0; n < objectsTerrain.Length; n++) {
//			GameObject tmpGameObject = objectsTerrain [n] as GameObject;
//			if (tmpGameObject != null) {
//				Debug.Log ("GameField::Start(); -- tmpGameObject:" + tmpGameObject);
//				GameObject realGameObject = (GameObject)Instantiate (tmpGameObject, new Vector3 (x*sizeCellX, 0, z*sizeCellZ), Quaternion.identity);
//				Debug.Log ("GameField::Start(); -- realGameObject:" + realGameObject);
//				realGameObject.transform.SetParent (this.transform);
//				z++;
//				if (z == 12) {
//					x++;
//					z = 0;
//				}
//			}
//		}
		loadMap("maps/arena0");
////		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);//Resources.Load("modelsField/Materials/BackColor.mat")
////		Renderer rend = go.GetComponent<Renderer>();
////		rend.material.mainTexture = Resources.Load("modelsField/Materials/BackColor.mat") as Texture;
//		Object ob = Resources.Load("naturePack_001");
//		Debug.Log ("GameField::Start(); -- ob:" + (ob==null) );
////		Debug.Log ("GameField::Start(); -- ob:" + ((ob!=null)?ob.name:"null") );
//		GameObject go = (GameObject)ob;
////		Debug.Log("GameField::Start(); -- go:" + ((go!=null)?go.name:"null") );
////		go.transform.SetParent (this.transform);
////		GameObject block1 = Instantiate(go, Vector3.zero, Quaternion.identity);
//		GameObject block2 = Instantiate(Resources.Load("modelsField/naturePack_001", typeof(GameObject))) as GameObject;
//		Debug.Log("GameField::Start(); -- block2:" + (block2==null) );
////		rend.transform.SetParent (this.transform);
//		block2.transform.SetParent (this.transform);
////		GenerateField();
	}

//	public void loadMaps(string mapsPath) {
//		print ("GameField::loadMaps(" + mapsPath + "); -- Start!");
//		Object[] maps = Resources.LoadAll(mapsPath);
//		Debug.Log ("GameField::loadMaps(); -- maps.Length:" + maps.Length);
//		foreach (Object mapObject in maps) {
//			TextAsset textAsset = mapObject as TextAsset;
//			if (textAsset != null) {
//				loadMap (textAsset);
//			}
//		}
//	}

	public bool loadMap(string mapPath) {
		print ("GameField::loadMap(" + mapPath + "); -- Start!");
		TextAsset textAsset = Resources.Load<TextAsset>(mapPath); // Не может загрузить TextAsset с расширением tmx только xml и другое гавно!
		print ("GameField::loadMap(); -- textAsset:" + textAsset);
		if (textAsset == null) {
			print ("GameField::loadMap(); -- Can't load map:" + mapPath);
			return false;
		}
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset.text);
		readNodes (xmlDoc, 0);
		XmlNodeList docChilds = xmlDoc.ChildNodes;
		foreach (XmlNode docChild in docChilds) {
			if (docChild.LocalName.Equals ("map")) {
				sizeFieldX = int.Parse(docChild.Attributes ["width"].Value);
				sizeFieldZ = int.Parse(docChild.Attributes ["height"].Value);
				print ("GameField::loadMap(); -- sizeFieldX:" + sizeFieldX + " sizeFieldZ:" + sizeFieldZ);
				mapProperties.Add ("width", sizeFieldX);
				mapProperties.Add ("height", sizeFieldZ);
				XmlNodeList mapNodeList = docChild.ChildNodes;
				foreach (XmlNode mapChildNode in mapNodeList) {
					if (mapChildNode.Name.Equals ("properties")) {
						loadProperties (mapChildNode, mapProperties);
					} else if (mapChildNode.Name.Equals ("tileset")) {
						XmlNodeList tilesetNodeList = mapChildNode.ChildNodes;
						foreach (XmlNode tilesetChildNode in tilesetNodeList) {
							if (tilesetChildNode.Name.Equals ("properties")) {
								loadProperties (tilesetChildNode, mapProperties);
								modelsPath = (string)mapProperties ["modelsPath"];
							} else if (tilesetChildNode.Name.Equals ("tile")) {
								int tileId = int.Parse (tilesetChildNode.Attributes ["id"].Value);
								XmlNodeList tileNodeList = tilesetChildNode.ChildNodes;
								foreach (XmlNode tileChildNode in tileNodeList) {
									if (tileChildNode.Name.Equals ("properties")) {
										Hashtable tileProperty = new Hashtable ();
										loadProperties (tileChildNode, tileProperty);
										string modelName = (string)tileProperty ["modelName"];
										print ("GameField::loadMap(); -- modelsPath:" + modelsPath + "/" + modelName);
										GameObject modelObject = Resources.Load<GameObject> ("maps/" + modelsPath + "/" + modelName);
										print ("GameField::loadMap(); -- modelObject:" + modelObject);
										modelsDictionary.Add (tileId, modelObject);
									}
								}
							}
						}
					} else if (mapChildNode.Name.Equals ("layer")) {
						XmlNodeList layerNodeList = mapChildNode.ChildNodes;
						foreach (XmlNode layerChildNode in layerNodeList) {
							if (layerChildNode.Name.Equals ("properties")) {
								Hashtable layerProperties = new Hashtable ();
								loadProperties (layerChildNode, layerProperties);
							} else if (layerChildNode.Name.Equals ("data")) {
								string[] array = layerChildNode.InnerText.Split (',');
								print (array.Length);
								int x = 0, z = 0;
								for (int k = 0; k < array.Length; k++) {
									GameObject tmpGameObject = modelsDictionary [int.Parse(array [k])];
									GameObject realGameObject = (GameObject)Instantiate (tmpGameObject, new Vector3 (x*sizeCellX, 0, z*sizeCellZ), Quaternion.identity);
									Debug.Log ("GameField::Start(); -- realGameObject:" + realGameObject);
									realGameObject.transform.SetParent (this.transform);
									x++;
									if (x == sizeFieldX) {
										x = 0;
										z++;
									}
								}
							}
						}
					}
				}
			}
		}

		foreach(DictionaryEntry de in mapProperties) {
			print ("GameField::loadMap(); -- key:" + de.Key + " value:" + de.Value);
		}
		print ("GameField::loadMap(); -- End!");
		return true;
	}

	public void loadProperties (XmlNode propertiesNode, Hashtable properties) {
		if(propertiesNode.Name.Equals("properties")) {
			XmlNodeList nodeList = propertiesNode.ChildNodes;
			foreach(XmlNode propertyNode in nodeList) {
				if(propertyNode.Name.Equals("property")) {
					string key = propertyNode.Attributes["name"].Value;
					string value = propertyNode.Attributes["value"].Value;
					properties.Add(key, value);
				}
			}
		}
	}

	public void readNodes(XmlNode xmlNode, int space) {
		string spaces = "";
		for (int s = 0; s < space; s++) {
			spaces += "  ";
		}
		print (spaces + xmlNode.Name + " : " + xmlNode.LocalName);
		XmlNodeList childs = xmlNode.ChildNodes;
		foreach (XmlNode xmlChildNode in childs) {
			readNodes (xmlChildNode, space++);
		}
	}

//	void GenerateField() {
//		Debug.Log("GameField::GenerateField(); -- Start!");
//		//Debug.Log ("GameField::GenerateField(); -- gameObjectTerrain1:" + gameObjectTerrain1);
//
//		//Debug.Log ("GameField::Awake(); -- sizeCellX:" + sizeCellX + " sizeCellY:" + sizeCellY + " sizeCellZ:" + sizeCellZ);
//
//		field = new Cell[sizeFieldX, sizeFieldZ];
//
//		for(int x = 0; x < sizeFieldX; x++) {
//			for(int z = 0; z < sizeFieldZ; z++) {
//				
////				field[x, z] = new Cell(x + (int)transform.position.x, 0, z + (int)transform.position.z);
////				field[x, z] = new Cell(x + nextPositionX, 0, z + nextPositionZ);
//
//				int rand = Random.Range(0, 2);
//
////				Debug.Log ("GameField::Awake(); -- rand:" + rand);
//
//				Cell cell = new Cell(x, 0, z);
//				GameObject copyGameObject;
//
//
//				if (rand == 0) {
//					copyGameObject = Instantiate(gameObjectTerrain1, new Vector3(x * sizeCellX, 0, z * sizeCellZ), Quaternion.identity);
//				} else {
//					copyGameObject = Instantiate(gameObjectWall1, new Vector3(x * sizeCellX, 0, z * sizeCellZ), Quaternion.identity);
//					cell.setTerrain();
//				}
//
//				copyGameObject.name = "Cell_" + cell.x + "_" + cell.z;
//				copyGameObject.transform.SetParent(this.transform, true);
//
//				//Debug.Log ("GameField::Awake(); -- copyGameObject:" + copyGameObject);
//				//Debug.Log ("GameField::Awake(); -- copyGameObject.transform:" + copyGameObject.transform);
//
//				field[x, z] = cell;
//			}
//		}
//	}

	void Update() {
//		if (Input.GetMouseButtonDown(0)) {
//			Transform[] allTransforms = gameObject.GetComponentsInChildren<Transform>();
//			foreach(Transform childObjects in allTransforms){
//				if(gameObject.transform.IsChildOf(childObjects.transform) == false)
//					Destroy(childObjects.gameObject);
//			}
//			GenerateField();
//		}
	}

	
	// Update is called once per frame
}
