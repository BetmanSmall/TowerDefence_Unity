using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	Camera cameraGame;
	ClickCell curCell;

	void Start () {
		cameraGame = GetComponent<Camera> ();
	}

	void Update () {
		Ray ray = cameraGame.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;



		if (Physics.Raycast(ray, out hit)) {
			bool isMouseDown = Input.GetMouseButtonDown (0);
			bool isMouseUp = Input.GetMouseButtonUp (0);
			ClickCell click = hit.collider.gameObject.GetComponent<ClickCell> ();
			if (hit.collider.gameObject) {
				
				if (isMouseDown || isMouseUp) {

				} else {
					if (curCell != null) {
						curCell.MouseExit ();
					}
					click.MouseEnter ();
					curCell = click;
				}
			}
			
		}
	}
}
