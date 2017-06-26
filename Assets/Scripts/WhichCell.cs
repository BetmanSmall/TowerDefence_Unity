using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhichCell : MonoBehaviour {
//	Camera cameraGame;
//	ClickCell curCell;

	void Start() {
//		cameraGame = GetComponent<Camera>();
	}

	void Update() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
//			Debug.Log ("WhichCell::Update(); -- hit:" + hit);
			bool isMouseDown = Input.GetButtonDown("Fire1");
//			bool isMouseUp = Input.GetMouseButtonUp(0);
//			ClickCell click = hit.collider.gameObject.GetComponent<ClickCell>();
//			Debug.Log ("WhichCell::Update(); -- hit.collider:" + hit.collider);
			if (hit.collider.gameObject) {
//				Debug.Log ("WhichCell::Update(); -- hit.collider.gameObject:" + hit.collider.gameObject);
				if (isMouseDown) {
					Debug.Log ("WhichCell::Update(); -- hit.transform.position:" + hit.transform.position);
				} 
//				else {
//					if (curCell != null) {
//						curCell.MouseExit();
//					}
//					click.MouseEnter();
//					curCell = click;
//				}
			}
		}
	}
}
