using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class MapLayer {
    public string name = "";
    public float opacity = 1.0f;
//    public bool visible = true;
//    public MapObjects objects = new MapObjects();
    public Dictionary<string, string> properties;
    public TileModel[,] tileModels;

    public MapLayer(int width, int height) {
        this.properties = new Dictionary<string, string> ();
        this.tileModels = new TileModel[width, height];
    }

    public void loadBasicLayerInfo(XmlNode layerNode) {
        XmlAttributeCollection attributeCollection = layerNode.Attributes;
        foreach (XmlAttribute xmlAttribute in attributeCollection) {
            string key = xmlAttribute.Name;
            string value = xmlAttribute.Value;
            if (key.Equals ("name")) {
                this.name = value;
            } else if (key.Equals ("visible")) {
                this.opacity = value.Equals("0") ? 0f : 1f;
            } else if (key.Equals ("opacity")) {
                this.opacity = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            }
        }
    }

    public override string ToString() {
        string str = "MapLayer[" + name + ",";
        str += opacity + ",";
//        str += visible + ",";
        str += properties + ",";
        str += tileModels + ",";
        str += "];";
        return str;
    }
}
