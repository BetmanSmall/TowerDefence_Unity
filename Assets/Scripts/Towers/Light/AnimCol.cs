using System.Collections;
using System.Collections.Generic;
using DigitalRuby.ThunderAndLightning;
using UnityEngine;

public class AnimCol : MonoBehaviour {

    // public Animator power;
	// public GameObject Source;
    // public GameObject Destination;
	public float Speed;
	// Use this for initialization
	void Start () {
		Speed = 7.0f;
	}
	
	 void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.transform.parent.name == "Creeps"){
            gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Source = gameObject.transform.Find("Head").gameObject.transform.Find("Oval").gameObject;
			gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Destination = other.gameObject;
            gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = true;
            other.gameObject.GetComponent<Creep>().die(100);
            }
    }

	void OnTriggerExit(Collider other)
    {
      if(other.gameObject.transform.parent.name == "Creeps"){
            gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Source = null;
			gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Destination = null;
            gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = false;
            }
    }
		
      
        








	//  void OnTriggerStay(Collider other)
    // {
    //   if(other.gameObject.transform.parent.name == "Creeps"){
    //       cubik.transform.position = Vector3.MoveTowards(cubik.transform.position,other.transform.position,Speed*Time.deltaTime);
	// 	  if(cubik.transform.position == other.transform.position){
	// 		  Destroy(cubik);
	// 	  }
    // }
	// }

	

    void Update(){

	}
}
