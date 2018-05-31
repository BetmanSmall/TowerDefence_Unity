using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetOrModelsSet {
    public string name;
    public Dictionary<int, TileModel> tileModels;
    public Dictionary<string, string> properties;

    public TileSetOrModelsSet(string name) {
        Debug.Log("TileSetOrModelsSet::TileSetOrModelsSet(" + name + "); -- Start!");
        this.name = name;
        this.tileModels = new Dictionary<int, TileModel>();
        this.properties = new Dictionary<string, string>();
    }

    public override string ToString() {
        string str = "TileSetOrModelsSet[" + name + ",";
        str += tileModels + ",";
        str += properties + ",";
        str += "];";
        return str;
    }
}
