using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCameraGrid : MonoBehaviour {
	public int sizeFieldX, sizeFieldZ;
	public Material lineMat;
//	public Color matColor;

	void drawGrid() {
		for (int x = 0; x <= sizeFieldX; x++) {
			drawLine(x, 0, x, sizeFieldX);
		}
		for (int z = 0; z <= sizeFieldZ; z++) {
			drawLine(0, z, sizeFieldZ, z);
		}
	}

	void drawLine(float x1, float z1, float x2, float z2) {
//		lineMat.color = matColor;
		GL.Begin(GL.LINES);
		lineMat.SetPass(0);
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
