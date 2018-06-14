using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : MonoBehaviour {
    public int mapHeight, mapWidth;
//    public Cell[,] field;
    public Material lineMat;
    public Camera gameCamera;

    private GameObject defaultTerrain;
    private string ModelPath = "maps/textures/fieldModels/naturePack_001";

    // Use this for initialization
    void Start () {
        //defaultTerrain = Instantiate (Resources.Load<GameObject>(ModelPath));
    //    field = new Cell[mapHeight, mapWidth];
        //LoadDefailtTerrain ();
    }

    void LoadDefailtTerrain() {
        Mesh mesh = Resources.Load<GameObject>(ModelPath).GetComponentInChildren<MeshFilter>().sharedMesh;

        // int sizeCellX = (int)mesh.bounds.size.x;
        // int sizeCellY = (int)mesh.bounds.size.y;
        // int sizeCellZ = (int)mesh.bounds.size.z;

//        for (int x = 0; x < mapHeight; x++) {
//            for (int z = 0; z < mapWidth; z++) {
//                
//                Cell cell = new Cell(x, 0, z);
//
//                defaultTerrain = Instantiate(Resources.Load<GameObject>(ModelPath), new Vector3(x * sizeCellX, 0, z * sizeCellZ), Quaternion.identity);
//
//                cell.setTerrain ();
//
//                defaultTerrain.name = "Cell_" + cell.x + "_" + cell.z;
//
//                defaultTerrain.transform.SetParent(this.transform, true);
//
//                field[x, z] = cell;
//
//            }
//        }
    }

    // Update is called once per frame
    void Update () {
        drawGrid();
    }

    void drawGrid() {
        for (int x = 0; x <= mapHeight; x++) {
            drawLine(x, 0, x, mapHeight);
        }
        for (int z = 0; z <= mapWidth; z++) {
            drawLine(0, z, mapWidth, z);
        }
    }

    void drawLine(float x1, float z1, float x2, float z2) {
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(Color.blue);
        GL.Vertex3(x1, 0f, z1);
        GL.Vertex3(x2, 0f, z2);
        GL.End();
    }

    // To show the lines in the game window whne it is running
    void OnPostRender() {
        drawGrid();
    }

    // To show the lines in the editor
    void OnDrawGizmos() {
        drawGrid();
    }
}
