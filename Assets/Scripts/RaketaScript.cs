using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaketaScript : MonoBehaviour {

    void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.transform.parent.name == "Creeps") {
        Destroy(gameObject);
		}
	}
}
