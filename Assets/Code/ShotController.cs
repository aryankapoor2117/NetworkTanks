using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 2);
	}

	void OnCollisionEnter(Collision other){
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
