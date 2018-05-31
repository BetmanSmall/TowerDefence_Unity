using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {
    // string mapPath; // ???MB here???
    public Dictionary<string, string> properties;
//    public Dictionary<int, GameObject> objectsTerrain;
    public Dictionary<int, TileSetOrModelsSet> tileSetsOrModelsSets;
    //    public ArrayList tileSetsOrModelsSets;
    public Dictionary<int, MapLayer> mapLayers;

    public Map() {
        Debug.Log("Map::Map(); -- Start!");
        properties = new Dictionary<string, string>();
//        objectsTerrain = new Dictionary<int, GameObject>();
        tileSetsOrModelsSets = new Dictionary<int, TileSetOrModelsSet>();
//        tileSetsOrModelsSets = new ArrayList();
        mapLayers = new Dictionary<int, MapLayer>();
    }
}
