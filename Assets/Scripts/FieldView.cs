using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldView : MonoBehaviour {

	int sizeFieldX, sizeFieldZ;
	public Material lineMat;

	public FieldView() {
		this.sizeFieldX = 10;
		this.sizeFieldZ = 10;
	}

	void DrawConnectingLines() {

		for (int x = 0; x <= sizeFieldX; x++) {
			drawLine(x, 0, x, sizeFieldX);
		}
		for (int z = 0; z <= sizeFieldZ; z++) {
			drawLine(0, z, sizeFieldZ, z);
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
		DrawConnectingLines();
	}

	// To show the lines in the editor
	void OnDrawGizmos() {
		DrawConnectingLines();
	}
}
