using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class MapLoader : MonoBehaviour {

	public MapLoader(/*WaveManager waveManager*/) {
		print ("MapLoader::MapLoader(//*waveManager*//); -- Start!");
	}

//	public void loadMaps(string mapsPath) {
//		print ("MapLoader::loadMaps(" + mapsPath + "); -- Start!");
//		Object[] maps = Resources.LoadAll(mapsPath);
//		Debug.Log ("MapLoader::loadMaps(); -- maps.Length:" + maps.Length);
//		foreach (Object mapObject in maps) {
//			TextAsset textAsset = mapObject as TextAsset;
//			if (textAsset != null) {
//				loadMap (textAsset);
//			}
//		}
//	}

	public Map loadMap(string mapPath) {
		print ("MapLoader::loadMap(" + mapPath + "); -- Start!");
		TextAsset textAsset = Resources.Load<TextAsset>(mapPath); // Не может загрузить TextAsset с расширением tmx только xml и другое гавно!
		print ("MapLoader::loadMap(); -- textAsset:" + textAsset);
		if (textAsset == null) {
			print ("MapLoader::loadMap(); -- Can't load map:" + mapPath);
			return null;
		}
		Map map = new Map();
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(textAsset.text);
		XmlNodeList docChilds = xmlDoc.ChildNodes;
		foreach (XmlNode docChild in docChilds) {
			if (docChild.LocalName.Equals ("map")) {
				map.properties.Add ("width", docChild.Attributes ["width"].Value);
				map.properties.Add ("height", docChild.Attributes ["height"].Value);
//				sizeFieldX = int.Parse(mapProperties["width"]);
//				sizeFieldZ = int.Parse(mapProperties["height"]);
//				print ("MapLoader::loadMap(); -- sizeFieldX:" + sizeFieldX + " sizeFieldZ:" + sizeFieldZ);
				XmlNodeList mapNodeList = docChild.ChildNodes;
				foreach (XmlNode mapChildNode in mapNodeList) {
					if (mapChildNode.Name.Equals ("properties")) {
						loadProperties(mapChildNode, map.properties);
					} else if (mapChildNode.Name.Equals ("tileset")) { // in future need change "tileset" to "modelsSet" or another
						loadTileSet(map, mapChildNode);
					} else if (mapChildNode.Name.Equals ("layer")) {
						loadMapLayer(map, mapChildNode);
					}
				}
			}
		}

		foreach(KeyValuePair<string, string> de in map.properties) {
			print ("MapLoader::loadMap(); -- key:" + de.Key + " value:" + de.Value);
		}
		print ("MapLoader::loadMap(); -- End! return map:" + map);
		return map;
	}

	public void loadProperties (XmlNode propertiesNode, Dictionary<string, string> properties) {
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

	private void loadTileSet(Map map, XmlNode tileSetNode) { // in future need change "tileset" to "modelsSet" or another
		if (tileSetNode.Name.Equals ("tileset")) {
//			string source = tileSetNode.Attributes["source"].Value;
//			if (source != null) {
//				Debug.Log("MapLoader::loadTileSet(); -- need implement this peace of code! found source:" + source);
//				return;
//			}
			string modelsPath = null;
			string name = tileSetNode.Attributes ["name"].Value;
			TileSetOrModelsSet tileSetOrModelsSet = new TileSetOrModelsSet(name);
			tileSetOrModelsSet.properties.Add ("firstgid", tileSetNode.Attributes ["firstgid"].Value);
			XmlNodeList tilesetNodeList = tileSetNode.ChildNodes;
			foreach (XmlNode tilesetChildNode in tilesetNodeList) {
				if (tilesetChildNode.Name.Equals ("properties")) {
					loadProperties (tilesetChildNode, tileSetOrModelsSet.properties);
					modelsPath = (string)tileSetOrModelsSet.properties["modelsPath"];
					if (modelsPath == null) {
						Debug.Log("MapLoader::loadTileSet(); -- not found modelsPath on properties node in tileset:" + name);
						return;
					}
				} else if (tilesetChildNode.Name.Equals ("tile")) {
					int tileId = int.Parse (tilesetChildNode.Attributes ["id"].Value);
					XmlNodeList tileNodeList = tilesetChildNode.ChildNodes;
					foreach (XmlNode tileChildNode in tileNodeList) {
						if (tileChildNode.Name.Equals ("properties")) {
							Dictionary<string, string> tileProperty = new Dictionary<string, string> ();
							loadProperties (tileChildNode, tileProperty);
							string modelName = (string)tileProperty ["modelName"];
							print ("MapLoader::loadMap(); -- modelsPath:" + modelsPath + "/" + modelName);
							Object modelObject = Resources.Load<Object> ("maps/" + modelsPath + "/" + modelName); // or GameObject?
							print ("MapLoader::loadMap(); -- modelObject:" + modelObject);
							TileModel tileModel = new TileModel (tileId, modelObject);
							tileModel.properties = tileProperty;
							tileSetOrModelsSet.tileModels.Add (tileId, tileModel);
						}
					}
				}
			}
			map.tileSetsOrModelsSets.Add (map.tileSetsOrModelsSets.Count, tileSetOrModelsSet);
		}
	}

	public void loadMapLayer(Map map, XmlNode layerNode) {
		if (layerNode.Name.Equals ("layer")) {
			int width = int.Parse(layerNode.Attributes["width"].Value);
			int height = int.Parse(layerNode.Attributes["height"].Value);
//			MapLayer mapLayer = new MapLayer(int.Parse(map.properties["width"]), int.Parse(map.properties["height"])); // diko, o4enb DiKo! need rewrite!
			MapLayer mapLayer = new MapLayer(width, height);
			mapLayer.name = layerNode.Attributes["name"].Value;
//			mapLayer.opacity = layerNode.Attributes["opacity"].Value; // need implement true
//			mapLayer.visible = layerNode.Attributes["visible"].Value; // need implement too
			XmlNodeList layerNodeList = layerNode.ChildNodes;
			foreach (XmlNode layerChildNode in layerNodeList) {
				if (layerChildNode.Name.Equals ("properties")) {
					loadProperties (layerChildNode, mapLayer.properties);
				} else if (layerChildNode.Name.Equals ("data")) {
					string[] ids = layerChildNode.InnerText.Split (','); // need implement getTileIds();
					print ("MapLoader::loadMapLayer(); -- ids.Length:" + ids.Length);
//					int x = 0, z = 0;
//					for (int k = 0; k < array.Length; k++) {
					for(int z = 0; z < height; z++) {
						for (int x = 0; x < width; x++) {
							int id = int.Parse(ids[z * width + x]) - int.Parse(map.tileSetsOrModelsSets[0].properties["firstgid"]); // not good | in future not only one!
							if (id >= 0) {
								TileModel tileModel = map.tileSetsOrModelsSets [0].tileModels [id]; // need create TileSets class and getTile();
								print ("MapLoader::loadMapLayer(); -- tileModels[" + x + "," + z + "]:" + tileModel);
								mapLayer.tileModels [x, z] = tileModel;
							} else {
								print ("MapLoader::loadMapLayer(); -- Empte cell tileModels[" + x + "," + z + "]:id:" + id );
							}
//							GameObject tmpGameObject = objectsTerrain [localId];
//							GameObject realGameObject = (GameObject)Instantiate (tmpGameObject, new Vector3 (x * sizeCellX + sizeCellX, 0, z * sizeCellZ + sizeCellZ), Quaternion.identity); // add shift for sizeCell
//							Debug.Log ("MapLoader::Start(); -- realGameObject:" + realGameObject + " array[k]:" + array [k]);
//							realGameObject.transform.SetParent (this.transform);
//							x++;
//							if (x == sizeFieldX) {
//								x = 0;
//								z++;
//							}
						}
					}
				}
			}
			map.mapLayers.Add (map.mapLayers.Count, mapLayer);
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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
