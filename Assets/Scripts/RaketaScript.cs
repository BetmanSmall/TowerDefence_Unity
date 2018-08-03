using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaketaScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	

    void OnTriggerEnter(Collider other){
		if (other.gameObject.transform.parent.name == "Creeps"){
        Destroy(gameObject);
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
