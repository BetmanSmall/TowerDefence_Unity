using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileModel {
    public int id;
    public Dictionary<string, string> properties;
    public Object modelObject; // or GameObject type???

    public TileModel(int id, Object modelObject) {
        this.id = id;
        this.properties = new Dictionary<string, string>();
        this.modelObject = modelObject;
    }
}
