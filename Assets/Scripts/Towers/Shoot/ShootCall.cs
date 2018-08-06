using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.ThunderAndLightning;
using UnityEngine;

public class ShootCall : MonoBehaviour {

    private GameObject cripik;
    public GameObject pula;
    private Animator animator;
    private float speed;
    private DateTime time;
    private TimeSpan NowTime;

    void Start () {
        time = DateTime.Now;
        animator = gameObject.GetComponent<Animator> ();
        speed = 5.0f;
    }

    void OnTriggerEnter (Collider other) {
        if (other.gameObject.transform.parent.name == "Creeps") {
            cripik = other.gameObject;
            animator.SetBool ("Shoot", true);
            Vector3 direction = gameObject.transform.position - other.gameObject.transform.position;
            float angle = Vector3.Angle (direction, gameObject.transform.forward);
            float Speed = 3.0f;
            Vector3 newDir = -Vector3.RotateTowards (gameObject.transform.forward, direction, angle, 0.0f);
            gameObject.transform.rotation = Quaternion.LookRotation (newDir);
            Pulemet ();
        }
    }

    void Pulemet () {
        GameObject patron1 = Instantiate (pula, gameObject.transform.Find ("Head").gameObject.transform.Find ("Cannon_1").gameObject.transform.Find ("Right").gameObject.transform.position, gameObject.transform.Find ("Head").gameObject.transform.Find ("Cannon_1").gameObject.transform.Find ("Right").gameObject.transform.rotation);
        GameObject patron2 = Instantiate (pula, gameObject.transform.Find ("Head").gameObject.transform.Find ("Cannon_2").gameObject.transform.Find ("Left").gameObject.transform.position, gameObject.transform.Find ("Head").gameObject.transform.Find ("Cannon_1").gameObject.transform.Find ("Right").gameObject.transform.rotation);
        Destroy (patron1, 0.25f);
        Destroy (patron2, 0.25f);
    }

    void OnTriggerStay (Collider other) {
        if (other.gameObject.transform.parent.name == "Creeps") {
            animator.SetBool ("Shoot", true);
            Pulemet ();
        }
    }

    void OnTriggerExit (Collider other) {
        animator.SetBool ("Shoot", false);
        CancelInvoke ();
    }

    void Update () {
        if (cripik == null) {
            animator.SetBool ("Shoot", false);
            CancelInvoke ();
        }
    }
}