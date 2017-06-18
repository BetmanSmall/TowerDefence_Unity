using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour {
	public GameObject gameObjectTerrain1;
	public GameObject gameObjectWall1;
	public int sizeFieldX, sizeFieldZ;
	private float sizeCellX, sizeCellY, sizeCellZ;
	Cell[,] field;

	// Use this for initialization
	void Start() {
		Debug.Log("GameField::Start(); -- Start!");
	}

	void Awake() {
		//Debug.Log ("GameField::Awake(); -- gameObjectTerrain1:" + gameObjectTerrain1);
		Mesh mesh = gameObjectTerrain1.GetComponentInChildren<MeshFilter>().sharedMesh;
		//Debug.Log ("GameField::Awake(); -- mesh:" + mesh);
		sizeCellX = mesh.bounds.size.x;
		sizeCellY = mesh.bounds.size.y;
		sizeCellZ = mesh.bounds.size.z;
		//Debug.Log ("GameField::Awake(); -- sizeCellX:" + sizeCellX + " sizeCellY:" + sizeCellY + " sizeCellZ:" + sizeCellZ);

		field = new Cell[sizeFieldX, sizeFieldZ];
		for(int x = 0; x < sizeFieldX; x++) {
			for(int z = 0; z < sizeFieldZ; z++) {
//				field[x, z] = new Cell(x + (int)transform.position.x, 0, z + (int)transform.position.z);
				//field[x, z] = new Cell(x + nextPositionX, 0, z + nextPositionZ);

				int rand = Random.Range(0, 2);
//				Debug.Log ("GameField::Awake(); -- rand:" + rand);
				Cell cell = new Cell(x, 0, z);
				GameObject copyGameObject;
				if (rand == 0) {
					copyGameObject = Instantiate(gameObjectTerrain1, new Vector3(x * sizeCellX, 0, z * sizeCellZ), Quaternion.identity);
				} else {
					copyGameObject = Instantiate(gameObjectWall1, new Vector3(x * sizeCellX, 0, z * sizeCellZ), Quaternion.identity);
					cell.setTerrain();
				}
				copyGameObject.name = "Cell_" + cell.x + "_" + cell.z;
				copyGameObject.transform.SetParent(this.transform, true);
				//Debug.Log ("GameField::Awake(); -- copyGameObject:" + copyGameObject);
				//Debug.Log ("GameField::Awake(); -- copyGameObject.transform:" + copyGameObject.transform);
				field[x, z] = cell;
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		
	}
}
