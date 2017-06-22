using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCameraGrid : MonoBehaviour {
	private Material gridLineMaterial;// = Resources.Load<Material>("Materials/GridLineMaterial"); // this not work!=(
	public float drawOnY = 0.4f; // Приподнять отрисовку сетки над GameField'ом
	private GameObject gameFieldObject;
	private GameField gameField;

//	public DrawCameraGrid() {
	public void Start() {
		print ("DrawCameraGrid::Start(); -- Start!");
		gridLineMaterial = Resources.Load<Material>("Materials/GridLineMaterial");
		print ("DrawCameraGrid::Start(); -- gridLineMaterial:" + gridLineMaterial);
		gameFieldObject = GameObject.Find ("GameField");
		print ("DrawCameraGrid::Start(); -- gameFieldObject:" + gameFieldObject);
		if (gameFieldObject == null) {
			Debug.LogError ("DrawCameraGrid::Start(); -- Not found GameField<GameObject> in hierarchy");
			return;
		} else {
			gameField = gameFieldObject.GetComponent<GameField>();
			print ("DrawCameraGrid::Start(); -- gameField:" + gameField);
		}
	}

	// To show the lines in the game window whne it is running
	void OnPostRender() {
//		print ("DrawCameraGrid::OnPostRender(); -- Start!");
		drawGrid();
	}

//	// To show the lines in the editor
	void OnDrawGizmos() {
//		print ("DrawCameraGrid::OnDrawGizmos(); -- Start!");
		drawGrid();
	}

	void drawGrid() {
		print ("DrawCameraGrid::drawGrid(); -- gameField:" + gameField);
		if (gameField != null) {
			int sizeFieldX = gameField.sizeFieldX;
			int sizeFieldZ = gameField.sizeFieldZ;
			float sizeCellX = gameField.sizeCellX;
			float sizeCellZ = gameField.sizeCellZ;
			print ("DrawCameraGrid::drawGrid(); -- gameField.sizeFieldX:" + gameField.sizeFieldX + " gameField.sizeFieldZ:" + gameField.sizeFieldZ);
			print ("DrawCameraGrid::drawGrid(); -- gameField.sizeCellX:" + gameField.sizeCellX + " gameField.sizeCellZ:" + gameField.sizeCellZ);
			for (int x = 0; x <= sizeFieldX; x++) {
				drawLine (x*sizeCellX, 0, x*sizeCellX, sizeFieldX*sizeCellZ);
			}
			for (int z = 0; z <= sizeFieldZ; z++) {
				drawLine (0, z*sizeCellZ, sizeFieldZ*sizeCellX, z*sizeCellZ);
			}
		}
	}

	void drawLine(float x1, float z1, float x2, float z2) {
		GL.Begin(GL.LINES);
		gridLineMaterial.SetPass(0);
		GL.Vertex3(x1, drawOnY, z1);
		GL.Vertex3(x2, drawOnY, z2);
		GL.End();
	}
}
