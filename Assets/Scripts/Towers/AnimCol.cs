using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCol : MonoBehaviour {

    public Animator power;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void OnTriggerEnter(Collider other) {
        if(other.gameObject.transform.parent.name == "Creeps"){
			power.SetBool("R",true);
		}
    }

	void OnTriggerExit(Collider other) {
        if(other.gameObject.transform.parent.name == "Creeps"){
			power.SetBool("R",false);
		}
    }
}
