using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceScript : MonoBehaviour {

	// Vars
	public int pieceID;
	public float forceMultiplier;

	private GameObject player;
	private Rigidbody body;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		// Movement
		if (checkInPlayerSight()) {
			float force = Input.GetAxis("Fire1");
			force -= Input.GetAxis("Fire2");

			Vector3 forcedir = transform.position - player.transform.position;
			float dist = forcedir.sqrMagnitude;
			forcedir.Normalize();
			forcedir *= force * forceMultiplier * mapfloat(dist, 15 * 15, 1);

			//transform.position += forcedir;
			body.MovePosition(transform.position + forcedir);
		}
		// --- End --- //
	}

	// Physics based update.
	private void FixedUpdate() {
		
	}


	// Helpers

	// Returns a mapped value 
	private float mapfloat(float input, float max, float min, float valmax = 0f, float valmin = 1f) {
		float retval = 0;

		if (input <= min) {
			retval = valmin;
		} else if (input >= max) {
			retval = valmax;
		} else { // The real meat n potatoes
			float scalar = (input - min) / (max - min);
			retval = (scalar * (valmax - valmin)) + valmin;
		}

		return retval;
	}

	// Checks if block is within 45 degrees of player vision.
	private bool checkInPlayerSight() {
		// Check angle.
		Vector3 plrforward = player.transform.forward;
		Vector3 plrtoblock = transform.position - player.transform.position;
		plrforward.Normalize();
		plrtoblock.Normalize();

		float dotprod = Vector3.Dot(plrtoblock, plrforward);

		// Check raycast
		if (dotprod >= 0.7) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 30f)) {
				if (hit.transform.tag == "Player") {
					return true;
				}
			}
		}

		return false;
	}
}
