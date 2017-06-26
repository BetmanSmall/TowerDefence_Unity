using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public float spaceMouseDetection = 5f;
	public float cameraSpeed = 5.0f;
	public float mouseSensitivity = 0.05f;
//	Camera camera;
	private Vector3 lastPosition;

	void Start() {
//		camera = GetComponent<Camera>();
	}

	void Update() {
//		Debug.Log ("CameraController::Update(); -- Start!");
		if (0f < Input.mousePosition.x && Input.mousePosition.x < spaceMouseDetection) {
			transform.position -= new Vector3 (cameraSpeed * Time.deltaTime, 0, 0);
		}
		if ((Screen.width-spaceMouseDetection) < Input.mousePosition.x && Input.mousePosition.x < Screen.width) {
			transform.position += new Vector3 (cameraSpeed * Time.deltaTime, 0, 0);
		}
		if (0f < Input.mousePosition.y && Input.mousePosition.y < spaceMouseDetection) {
			transform.position -= new Vector3 (0, 0, cameraSpeed * Time.deltaTime);
		}
		if ((Screen.height-spaceMouseDetection) < Input.mousePosition.y && Input.mousePosition.y < Screen.height) {
			transform.position += new Vector3 (0, 0, cameraSpeed * Time.deltaTime);
		}

		if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			transform.Translate(new Vector3(cameraSpeed*Time.deltaTime, 0, 0));
		}
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			transform.Translate(new Vector3((-cameraSpeed*Time.deltaTime), 0, 0));
		}
		if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
			transform.Translate(new Vector3(0, -(cameraSpeed*Time.deltaTime), 0));
		}
		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			transform.Translate(new Vector3(0, cameraSpeed*Time.deltaTime, 0));
		}

		if (Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0)) { // Что лучше?
			lastPosition = Input.mousePosition;
		}
		if (Input.GetButton("Fire1") || Input.GetMouseButton(0)) { // Что лучше?
			Vector3 delta = Input.mousePosition - lastPosition;
//			Debug.Log ("CameraController::Update(); -- delta:" + delta);
//			transform.Translate(delta.x * mouseSensitivity, 0f, delta.y * mouseSensitivity);
			Vector3 olsPos = transform.position;
			Vector3 newPos = new Vector3 (olsPos.x - (delta.x * mouseSensitivity), olsPos.y, olsPos.z - (delta.y * mouseSensitivity));
			transform.position = newPos;
			lastPosition = Input.mousePosition;
		}

		float scrollDelta = Input.mouseScrollDelta.y;
		if (scrollDelta != 0) {
			Debug.Log ("CameraController::Update(); -- scrollDelta:" + scrollDelta + " Need implement zoom!");
		}

//		Vector3 vp = camera.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane));
//		vp.x -= 0.5f;
//		vp.y -= 0.5f;
//		vp.x *= mouseSensitivity;
//		vp.y *= mouseSensitivity;
//		vp.x += 0.5f;
//		vp.y += 0.5f;
//		Vector3 sp = camera.ViewportToScreenPoint(vp);
//		 
//		Vector3 v = camera.ScreenToWorldPoint(sp);
//		transform.LookAt(v, Vector3.up);
	}
}
