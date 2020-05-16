using System.Collections;
using System.Collections.Generic;
using DigitalRuby.ThunderAndLightning;
using UnityEngine;

public class AnimCol : MonoBehaviour {

    private GameObject cripik;

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.transform.parent.name == "Creeps") {
            cripik = other.gameObject;
            gameObject.transform.Find ("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript> ().Source = gameObject.transform.Find ("Head").gameObject.transform.Find ("Oval").gameObject;
            gameObject.transform.Find ("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript> ().Destination = other.gameObject;
            gameObject.transform.Find ("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript> ().enabled = true;
            other.gameObject.GetComponent<Creep> ().die (100);
        }
    }

    void OnTriggerExit (Collider other) {

        if (other.gameObject.transform.parent.name == "Creeps") {
            gameObject.transform.Find ("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript> ().enabled = false;
        }
    }

    void Update () {
        if (cripik == null) {
            // gameObject.transform.Find ("LightningBoltPrefab").GetComponent<LightningBoltPrefabScript> ().enabled = false;
        }
    }
}