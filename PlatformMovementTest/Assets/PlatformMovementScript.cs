using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementScript : MonoBehaviour {

	public float speed = 5.0f;
	public GameObject platformObject;
	public GameObject spawnedObject;

	private float xInput;
	private float yInput;

	private Transform platformTransform;
	private Vector3 objectSpawnOffset = new Vector3(-1f, 2f, 0f);

	// Use this for initialization
	void Start () {
		// Nothing to define for now
		platformTransform = platformObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		// Getting input values from controls
		xInput = Input.GetAxis("Horizontal");	// Handle rotation of platform
		yInput = Input.GetAxis("Vertical");     // Handle height of platform

		// Actually moving platform
		// Vertical, easy.
		this.transform.Translate(new Vector3(0.0f, yInput / 2.0f, 0.0f));
		// Horizontal, rotate the transform.
		this.transform.Rotate(new Vector3(0f, 1f, 0f), xInput * speed * -1);

		// Spawning an object test
		if (Input.GetButtonDown("Jump")) {
			GameObject newSpawn = Object.Instantiate(spawnedObject, new Vector3(platformTransform.position.x, platformTransform.position.y, platformTransform.position.z), new Quaternion());
			Vector3 dir = new Vector3(this.transform.position.x - platformTransform.position.x, 0.0f, this.transform.position.z - platformTransform.position.z);
			newSpawn.transform.Translate(dir.normalized);
			newSpawn.transform.Translate(0f, 2f, 0f);
		}
	}
}
