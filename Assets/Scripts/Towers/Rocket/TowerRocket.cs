using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRocket: MonoBehaviour {

	private GameObject head;
	public GameObject roket;
	private GameObject raketa;
	private GameObject creep;
	private float speed;
	bool povorot;
	bool raketastart;
	private Vector3 snaradpoint;
	
	void Start() {
		head = gameObject.transform.Find("Head").gameObject;
		speed = 5.0f;
		povorot = false;
		raketastart = false;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.transform.parent.name == "Creeps") {
			creep = other.gameObject;
			povorot = true;
            snaradpoint = other.gameObject.transform.position;
			GameObject newraketa = (GameObject) Instantiate(roket,gameObject.transform.Find("Head").gameObject.transform.Find("StartPoint").gameObject.transform.position, gameObject.transform.Find("Head").gameObject.transform.Find("StartPoint").gameObject.transform.rotation);
			raketa = newraketa;
			raketastart = true;
		}
	}

	void OnTriggerExit(Collider other) {
		povorot = false;
		creep = null;
	}



	void Update() {
		if (povorot == false) {
			head.transform.Rotate(Vector3.up, 1, Space.World);
		} 
		else if (povorot == true) {
			var newRotation = Quaternion.LookRotation(creep.transform.position-head.transform.position, Vector3.forward);
			newRotation.x = 0.0f;
			newRotation.z = 0.0f;
			head.transform.rotation = Quaternion.Slerp(head.transform.rotation, newRotation, Time.deltaTime * 30);
		}

		if (raketastart == true) {
        raketa.transform.position = Vector3.MoveTowards(raketa.transform.position, snaradpoint,Time.deltaTime * 15.0f);
	}
}
}