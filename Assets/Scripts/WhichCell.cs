using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhichCell : MonoBehaviour {
	private GameObject gameFieldObject;
	private GameField gameField;
//	Camera cameraGame;
//	ClickCell curCell;

	void Start() {
//		cameraGame = GetComponent<Camera>();
		print ("WhichCell::Start(); -- Start!");
		gameFieldObject = GameObject.Find ("GameField");
		print ("WhichCell::Start(); -- gameFieldObject:" + gameFieldObject);
		if (gameFieldObject == null) {
			Debug.LogError ("WhichCell::Start(); -- Not found GameField<GameObject> in hierarchy");
			return;
		} else {
			gameField = gameFieldObject.GetComponent<GameField>();
			print ("WhichCell::Start(); -- gameField:" + gameField);
		}
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
				if (isMouseDown) {
					Debug.Log ("WhichCell::Update(); -- hit.collider.gameObject:" + hit.collider.gameObject);
					Debug.Log ("WhichCell::Update(); -- hit.transform.position:" + hit.transform.position);
					Cell cell = hit.collider.gameObject.GetComponentInParent<Cell>();
					if(cell != null) {
						Debug.Log ("WhichCell::Update(); -- cell:" + cell + " cell.setTerrain();");
						cell.setTerrain(); // need reWrite in future! All codes need reWrite!
					}
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
