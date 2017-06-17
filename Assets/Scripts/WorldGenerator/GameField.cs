using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour {

	public GameObject cellMesh;
	public int xSize, zSize;
	Cell[,] field;

	// Use this for initialization
	void Start () {
		
	}

	void Awake() {
		field = new Cell[xSize, zSize];

		for(int x = 0; x < xSize; x++){
			for(int z = 0; z < zSize; z++){
				field[x, z] = new Cell(x + (int)transform.position.x, 0, z + (int)transform.position.z);
				GameObject copyMesh = Instantiate(cellMesh, cellMesh.transform.position, Quaternion.identity);
				copyMesh.name = "Cell_" + field[x, z].x + "_" + field[x, z].z;
				copyMesh.transform.position = new Vector3(field[x, z].x, 0, field[x, z].z);
				copyMesh.transform.SetParent (this.transform, true);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
