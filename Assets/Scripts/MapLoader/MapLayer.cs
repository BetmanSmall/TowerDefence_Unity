using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayer {
	public string name = "";
	public float opacity = 1.0f;
	public bool visible = true;
//	public MapObjects objects = new MapObjects();
	public Dictionary<string, string> properties;
	public TileModel[,] tileModels;

	public MapLayer(int width, int height) {
		this.properties = new Dictionary<string, string> ();
		this.tileModels = new TileModel[width, height];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
