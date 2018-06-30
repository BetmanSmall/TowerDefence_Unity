using System.Collections;
using System.Collections.Generic;
using DigitalRuby.ThunderAndLightning;
using UnityEngine;

public class AnimCol : MonoBehaviour {

    // public Animator power;
	// public GameObject Source;
    // public GameObject Destination;

	// Use this for initialization
    private GameObject cripik;
	
	 void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.transform.parent.name == "Creeps"){
            cripik = other.gameObject;
            gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Source = gameObject.transform.Find("Head").gameObject.transform.Find("Oval").gameObject;
			gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Destination = other.gameObject;
            gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = true;
            other.gameObject.GetComponent<Creep>().die(100);
            }
    }
    // void OnTriggerStay(Collider other){
    //     if(other == null){
    //         gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = false;
    //     }
    // }
	void OnTriggerExit(Collider other)
    {
        
      if(other.gameObject.transform.parent.name == "Creeps"){
            gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = false;
            // gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Source = null;
			// gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().Destination = null;
       
            }
    }
		
      
    void Update(){
        if(cripik == null){
            gameObject.transform.Find("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript>().enabled = false;
	}
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

	


