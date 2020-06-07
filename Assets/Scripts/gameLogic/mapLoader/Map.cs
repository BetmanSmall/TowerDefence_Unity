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

    public TileModel getTileModel(int id) {
        foreach (TileSetOrModelsSet tileSetOrModelsSet in tileSetsOrModelsSets.Values) {
            foreach (TileModel tileModel in tileSetOrModelsSet.tileModels.Values) {
                if (tileModel.id.Equals(id)) {
                    return tileModel;
                }
            }
        }
        return null;
    }

    public TileModel getTileModel(string tileName) {
        foreach (TileSetOrModelsSet tileSetOrModelsSet in tileSetsOrModelsSets.Values) {
            foreach (TileModel tileModel in tileSetOrModelsSet.tileModels.Values) {
                if (tileModel.modelObject.name.Equals(tileName)) {
                    return tileModel;
                }
            }
        }
        return null;
    }
}
