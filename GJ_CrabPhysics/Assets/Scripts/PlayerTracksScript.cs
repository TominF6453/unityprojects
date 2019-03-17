using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracksScript : MonoBehaviour {

	private Vector3 lookAtVector;

	// Use this for initialization
	void Start () {
		lookAtVector = new Vector3(0, 0, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		// Manage rotation
		float xforce = Input.GetAxis("Horizontal");
		float yforce = Input.GetAxis("Vertical");

		Vector3 move = new Vector3(xforce, 0f, yforce);

		if (move != Vector3.zero) {
			lookAtVector = move;
		}
		transform.LookAt(transform.position + lookAtVector);
		// --- END --- //
	}

	// Called after update, but once per frame
	private void LateUpdate() {
		
	}
}
