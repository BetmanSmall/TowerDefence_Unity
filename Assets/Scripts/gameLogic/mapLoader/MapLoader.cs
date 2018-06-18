using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class MapLoader {
    string mapPath;
    public MapLoader(/*WaveManager waveManager*/) {
        Debug.Log("MapLoader::MapLoader(//*waveManager*//); -- Start!");
    }

//    public void loadMaps(string mapsPath) {
//        Debug.Log("MapLoader::loadMaps(" + mapsPath + "); -- Start!");
//        Object[] maps = Resources.LoadAll(mapsPath);
//        Debug.Log ("MapLoader::loadMaps(); -- maps.Length:" + maps.Length);
//        foreach (Object mapObject in maps) {
//            TextAsset textAsset = mapObject as TextAsset;
//            if (textAsset != null) {
//                loadMap (textAsset);
//            }
//        }
//    }

    public Map loadMap(string mapPath) {
        Debug.Log("MapLoader::loadMap(" + mapPath + "); -- Start!");
        this.mapPath = mapPath;
        TextAsset textAsset = Resources.Load<TextAsset>(mapPath); // Не может загрузить TextAsset с расширением tmx только xml и другое гавно!
        Debug.Log("MapLoader::loadMap(); -- textAsset:" + textAsset);
        if (textAsset == null) {
            Debug.Log("MapLoader::loadMap(); -- Can't load map:" + mapPath);
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
//                sizeFieldX = int.Parse(mapProperties["width"]);
//                sizeFieldZ = int.Parse(mapProperties["height"]);
//                Debug.Log("MapLoader::loadMap(); -- sizeFieldX:" + sizeFieldX + " sizeFieldZ:" + sizeFieldZ);
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

        Debug.Log("MapLoader::loadMap(); -- map.properties.Count:" + map.properties.Count);
        foreach(KeyValuePair<string, string> property in map.properties) {
            Debug.Log("MapLoader::loadMap(); -- property.Key[" + property.Key + "]:" + property.Value);
        }
        Debug.Log("MapLoader::loadMap(); -- End! return map:" + map);
        return map;
    }

    public static void loadProperties(XmlNode propertiesNode, Dictionary<string, string> properties) {
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
            string source;
            if(tileSetNode.Attributes["source"] != null) {
            // if (source != null) {
                source = tileSetNode.Attributes["source"].Value;
                string tsxPath = findFile(mapPath, source);
                TextAsset textAsset = Resources.Load<TextAsset>(tsxPath); // Не может загрузить TextAsset с расширением tmx только xml и другое гавно!
                Debug.Log("MapLoader::loadTileSet(); -- textAsset:" + textAsset);
                if (textAsset == null) {
                    Debug.Log("MapLoader::loadTileSet(); -- Can't load:" + tsxPath);
                    return;
                }
                XmlDocument tsxDoc = new XmlDocument();
                tsxDoc.LoadXml(textAsset.text);
                tileSetNode = tsxDoc.ChildNodes[1]; // XML is Bad. In xmlDox first child in not index 0 / tsxDoc.firstChild() not work!
            }
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
//                            Debug.Log("MapLoader::loadMap(); -- modelsPath:" + modelsPath + "/" + modelName);
                            Object modelObject = Resources.Load<Object> ("maps/" + modelsPath + "/" + modelName); // or GameObject?
//                            Debug.Log("MapLoader::loadMap(); -- modelObject:" + modelObject);
                            TileModel tileModel = new TileModel (tileId, modelObject);
                            tileModel.properties = tileProperty;
                            tileSetOrModelsSet.tileModels.Add (tileId, tileModel);
                        }
                    }
                }
            }
            Debug.Log("MapLoader::loadTileSet(); -- map.tileSetsOrModelsSets.Add(" + map.tileSetsOrModelsSets.Count + ", " + tileSetOrModelsSet + ");");
            map.tileSetsOrModelsSets.Add (map.tileSetsOrModelsSets.Count, tileSetOrModelsSet);
        }
    }

    public void loadMapLayer(Map map, XmlNode layerNode) {
        if (layerNode.Name.Equals("layer")) {
            int width = int.Parse(layerNode.Attributes["width"].Value);
            int height = int.Parse(layerNode.Attributes["height"].Value);
//            MapLayer mapLayer = new MapLayer(int.Parse(map.properties["width"]), int.Parse(map.properties["height"])); // diko, o4enb DiKo! need rewrite!
            MapLayer mapLayer = new MapLayer(width, height);
            mapLayer.loadBasicLayerInfo(layerNode);
            Debug.Log("MapLoader::loadMapLayer(); -- mapLayer:" + mapLayer);
            XmlNodeList layerNodeList = layerNode.ChildNodes;
            foreach (XmlNode layerChildNode in layerNodeList) {
                if (layerChildNode.Name.Equals("properties")) {
                    loadProperties (layerChildNode, mapLayer.properties);
                } else if (layerChildNode.Name.Equals("data")) {
                    string[] ids = layerChildNode.InnerText.Split(','); // need implement getTileIds();
//                    Debug.Log("MapLoader::loadMapLayer(); -- ids.Length:" + ids.Length);
//                    int x = 0, z = 0;
//                    for (int k = 0; k < array.Length; k++) {
                    for(int z = 0; z < height; z++) {
                        for (int x = 0; x < width; x++) {
                            int firstgid = 0;
                            if(map.tileSetsOrModelsSets[0].properties["firstgid"] != null) {
                                firstgid = int.Parse(map.tileSetsOrModelsSets[0].properties["firstgid"]);
                            }
                            int id = int.Parse(ids[z * width + x]) - firstgid; // not good | in future not only one!
                            if (id >= 0) {
                                // Debug.Log("MapLoader::loadMapLayer(); -- id:" + id + " firstgid:" + firstgid);
                                TileModel tileModel = map.tileSetsOrModelsSets[0].tileModels[id]; // need create TileSets class and getTile();
//                                Debug.Log("MapLoader::loadMapLayer(); -- tileModels[" + x + "," + z + "]:" + tileModel);
                                mapLayer.tileModels [x, z] = tileModel;
//                            } else {
//                                Debug.Log("MapLoader::loadMapLayer(); -- In " + mapLayer.name + "[" + x + "," + z + "] not set tileModel!");
                            }
                        }
                    }
                }
            }
            Debug.Log("MapLoader::loadMapLayer(); -- map.mapLayers.Add(" + map.mapLayers.Count + ", " + mapLayer + ");");
            map.mapLayers.Add(map.mapLayers.Count, mapLayer);
        }
    }

    public void readNodes(XmlNode xmlNode, int space) {
        string spaces = "";
        for (int s = 0; s < space; s++) {
            spaces += "  ";
        }
        Debug.Log(spaces + xmlNode.Name + " : " + xmlNode.LocalName);
        XmlNodeList childs = xmlNode.ChildNodes;
        foreach (XmlNode xmlChildNode in childs) {
            readNodes (xmlChildNode, space++);
        }
    }

    public static string findFile(string mapPath, string filePath) {
        // Debug.Log("MapLoader::findFile(" + mapPath + ", " + filePath + "); -- ");
        try {
            // mapPath = mapPath.ReplaceAll need =(
            string result = mapPath.Substring(0, mapPath.LastIndexOf("/"));
            bool finished = false;
            do {
                // Debug.Log("MapLoader::findFile(); -1- result:" + result + " filePath:" + filePath + " finished:" + finished);
                if(result.Length == 0) {
                    result = filePath;
                    break;
                }
                int slashIndex = filePath.IndexOf("/");
                if(slashIndex == -1) {
                    // Debug.Log("MapLoader::findFile(); -1- result:" + result + " filePath:" + filePath + " slashIndex:" + slashIndex);
                    result = result + "/" + filePath;
                    finished = true;
                } else {
                    // Debug.Log("MapLoader::findFile(); -2- result:" + result + " filePath:" + filePath + " slashIndex:" + slashIndex);
                    string token = filePath.Substring(0, slashIndex);
                    filePath = filePath.Substring(slashIndex+1);
                    if(token == "..") {
                        int lastSlashIndex = result.LastIndexOf("/");
                        if(lastSlashIndex == -1) {
                            result = "";
                        } else {
                            result = result.Substring(0, result.LastIndexOf("/"));
                        }
                    } else {
                        result = result + "/" + token;
                    }
                }
                // Debug.Log("MapLoader::findFile(); -2- result:" + result + " filePath:" + filePath + " finished:" + finished);
            } while (!finished);
            if(result.Contains(".xml")) {
                result = result.Replace(".xml", "");
                // result = result.Substring(0, result.LastIndexOf(".xml"));
            }
            Debug.Log("MapLoader::findFile(); -- Exit result:" + result);
            return result;
        } catch(System.Exception exp) {
            Debug.LogError("MapLoader::findFile(); -- exp:" + exp);
            return null;
        }
    }
}
