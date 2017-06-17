using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMesh : MonoBehaviour {
	Mesh mesh;
	public static Vector3[] vertices;

	void Awake() {
		vertices = new Vector3[] {
			new Vector3 (0, 0, 0),
			new Vector3 (1, 0, 0),
			new Vector3 (1, 0, 1),
			new Vector3 (0, 0, 1),
		};

		int[] triangles = {
			1, 0, 2,
			2, 0, 3
		};

		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider> ().sharedMesh = mesh;

		mesh.vertices = vertices;
		mesh.triangles = triangles;
	}
}
