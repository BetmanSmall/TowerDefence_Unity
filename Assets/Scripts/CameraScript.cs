using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	public float speed = 10.0f;
	public float spaceMouseDetection = 5f;
//	Camera cameraGame;
//	ClickCell curCell;
	public float mouseSensitivity = 1.0f;
	private Vector3 lastPosition;

	void Start() {
//		cameraGame = GetComponent<Camera>();
	}

	void Update() {
//		Ray ray = cameraGame.ScreenPointToRay(Input.mousePosition);
//		RaycastHit hit;
//
//		if (Physics.Raycast(ray, out hit)) {
//			bool isMouseDown = Input.GetMouseButtonDown(0);
//			bool isMouseUp = Input.GetMouseButtonUp(0);
//			ClickCell click = hit.collider.gameObject.GetComponent<ClickCell>();
//			if (hit.collider.gameObject) {
//				if (isMouseDown || isMouseUp) {
//
//				} else {
//					if (curCell != null) {
//						curCell.MouseExit();
//					}
//					click.MouseEnter();
//					curCell = click;
//				}
//			}
//		}

		if (Input.GetMouseButtonDown(0)) {
			lastPosition = Input.mousePosition;
		}

		if (Input.GetMouseButton(0)) {
			Vector3 delta = Input.mousePosition - lastPosition;
			transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
			lastPosition = Input.mousePosition;
		}

		if (0f < Input.mousePosition.x && Input.mousePosition.x < spaceMouseDetection) {
			transform.position -= new Vector3 (speed * Time.deltaTime, 0, 0);
		}
		if ((Screen.width-spaceMouseDetection) < Input.mousePosition.x && Input.mousePosition.x < Screen.width) {
			transform.position += new Vector3 (speed * Time.deltaTime, 0, 0);
		}
		if (0f < Input.mousePosition.y && Input.mousePosition.y < spaceMouseDetection) {
			transform.position -= new Vector3 (0, 0, speed * Time.deltaTime);
		}
		if ((Screen.height-spaceMouseDetection) < Input.mousePosition.y && Input.mousePosition.y < Screen.height) {
			transform.position += new Vector3 (0, 0, speed * Time.deltaTime);
		}
	}
}
