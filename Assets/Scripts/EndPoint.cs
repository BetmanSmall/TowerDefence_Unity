using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.transform.parent.name == "Creeps") {
            Debug.Log("EndPoint::OnTriggerEnter(); -- other:" + other);
            Destroy(other.gameObject);
        }
    }
}
