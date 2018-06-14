using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(LineRenderer))]
public class Cell : MonoBehaviour { // пиздеЦЦЦ!!!
    // public ArrayList<Tree> trees;
    public bool empty;
    public bool terrain;
    public Tower tower;
    public List<Creep> creeps;
    public int gameX, layerY, gameZ;
    public Vector3 graphicCoordinates;
    // TileModel tileModel;
    // GameObject gameObjectModel;
    public int numberByWaveAlgorithm;
    
    // LineRenderer lineRenderer;
    // [Range(0.1f, 100f)]
    // public float radius = 1.0f;
    // [Range(0,5)]
    // public float xRadius = 5;
    // [Range(0,5)]
    // public float yRadius = 5;
    // [Range(0, 256)]
    // public int numSegments = 128;

    public Cell() {
        // Debug.Log("Cell::Cell(); -- ");
        // this.trees = ;
        this.empty = true;
        this.terrain = false;
        this.tower = null;
        this.creeps = null;
        // gameX = 0, layerY = 0, gameZ = 0;
        this.graphicCoordinates = Vector3.zero;
        // this.tileModel = null;
        // this.gameObjectModel = null;
        // this.numberByWaveAlgorithm;
        // setDebugLineRender();
    }

    public Cell(int gameX, int layerY, int gameZ, Vector3 graphicCoordinates) {
        // Debug.Log("Cell::Cell(); -- gameX:" + gameX + " layerY:" + layerY + " gameZ:" + gameZ + " graphicCoordinates:" + graphicCoordinates);
        setBasicValues(gameX, layerY, gameZ, graphicCoordinates);
//        this.gameObjectModel = (GameObject)Instantiate(tileModel.modelObject, graphicCoordinates, Quaternion.identity, this.transform);
//        MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer> ();
//        foreach (Material material in meshRenderer.materials) {
//            //                            Debug.Log("GameField::Start(); -- material:" + material);
//            Color color = material.color;
//            color.a = mapLayer.opacity; // It is not WOKR!=(
//            material.color = color;
//            Debug.Log("GameField::Start(); -- material.color:" + material.color);
//        }
        // setDebugLineRender();
    }

    public void setBasicValues(int gameX, int layerY, int gameZ, /*TileModel tileModel,*/ Vector3 graphicCoordinates) {
        // Debug.Log("Cell::setBasicValues(); -- gameX:" + gameX + " layerY:" + layerY + " gameZ:" + gameZ + " graphicCoordinates:" + graphicCoordinates);
        this.gameX = gameX;
        this.layerY = layerY;
        this.gameZ = gameZ;
        // this.tileModel = tileModel;
        this.graphicCoordinates = graphicCoordinates;
    }

    // public void setDebugLineRender(LineRenderer lineRenderer) {
    //     // this.gameObject.AddComponent<LineRenderer>();
    //     // lineRenderer = gameObject.GetComponentInParent<LineRenderer>();
    //     this.lineRenderer = lineRenderer;
    //     // lineRenderer.SetVertexCount(numSegments + 1);
    //     lineRenderer.useWorldSpace = true;
    //     CreatePoints();
    //     // DoRenderer();
    // }

    // void CreatePoints() {
    //     float x;
    //     float y;
    //     float z;
    //     float angle = 20f;
    //     for (int i = 0; i < (numSegments + 1); i++) {
    //         x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
    //         z = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;
    //         lineRenderer.SetPosition (i,new Vector3(x,0,z) );
    //         angle += (360f / numSegments);
    //     }
    // }

    // public void DoRenderer() {
    //     LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
    //     Color c1 = new Color(1f, 0f, 0f, 1f);
    //     Color c2 = new Color(0f, 1f, 0f, 1f);
    //     lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
    //     lineRenderer.SetColors(c1, c2);
    //     lineRenderer.SetWidth(0.1f, 0.9f);
    //     // lineRenderer.SetVertexCount(numSegments + 1);
    //     lineRenderer.useWorldSpace = true;

    //     float deltaTheta = (float) (2.0 * Mathf.PI) / numSegments;
    //     float theta = 0f;

    //     for (int i = 0 ; i < numSegments + 1 ; i++) {
    //         float x = radius * Mathf.Cos(theta);
    //         float z = radius * Mathf.Sin(theta);
    //         Vector3 pos = new Vector3(x, 0, z);
    //         lineRenderer.SetPosition(i, pos);
    //         theta += deltaTheta;
    //     }
    // }

    // public void setGraphicCoordinates();

    public Vector3 getGraphicCoordinates() {
        return graphicCoordinates;
    }

    public bool isEmpty() {
        // Debug.Log("Cell::isEmpty(); -- empty:" + empty);
        return empty;
    }

    public bool isTerrain() {
        return terrain;
    }

    public bool setTerrain() {
        if (empty) {
            terrain = true;
            empty = false;
            return true;
        }
        return false;
    }
    
    public bool removeTerrain() {
        if (terrain) {
            terrain = false;
            empty = true;
            return true;
        }
        return false;
    }

    public bool isPassable() {
        if (empty /*|| (!terrain && tower != null) || creeps != null*/) {
            return true;
        }
        return false;
    }

    public Tower getTower() {
        return tower;
    }

    public bool setTower(Tower tower) {
        if (empty) {
            this.tower = tower;
            empty = false;
            return true;
        }
        return false;
    }

    public bool removeTower() {
        if (tower != null) {
            tower = null;
            empty = true;
            return true;
        }
        return false;
    }

    public List<Creep> getCreeps() {
        return creeps;
    }

    public Creep getCreep() {
        if (creeps != null && creeps.Count != 0) {
            return creeps[0];
        }
        return null;
    }

    public bool setCreep(Creep creep) {
        if (empty) {
            creeps = new List<Creep>();
            creeps.Add(creep);
            empty = false;
            return true;
        } else if (creeps != null) {
            creeps.Add(creep);
            return true;
        }
        return false;
    }

    public int removeCreep(Creep creep) {
        if (creeps != null) {
            creeps.Remove(creep);
            if (creeps.Count == 0) {
                creeps = null;
                empty = true;
                return 0;
            }
            return creeps.Count;
        }
        return -1;
    }

    // public void dispose() {
    //     backgroundTiles.clear();
    //     foregroundTiles.clear();
    //     backgroundTiles = null;
    //     foregroundTiles = null;
    //     tower = null;
    //     creeps.clear();
    //     creeps = null;
    // }

    // public string toString() {
    //     StringBuilder sb = new StringBuilder();
    //     sb.append("Cell[");
    //     sb.append("cellX:" + cellX);
    //     sb.append("," + "cellY:" + cellY);
    //     sb.append("," + "empty:" + empty);
    //     sb.append("," + "terrain:" + terrain);
    //     sb.append("," + "tower:" + tower);
    //     sb.append("," + "creeps:" + creeps);
    //     sb.append("]");
    //     return sb.toString();
    // }
}
