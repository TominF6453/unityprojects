using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
	private GameObject sprite_back; // The back sprite, should just rotate in a random direction.
	private GameObject sprite_face; // The face sprite, face the player?

	private float rot_dir;
	private float rot_force;

	// Start is called before the first frame update
	void Start() {
		rot_dir = Mathf.Sign(Random.Range(-1f, 1f));
		rot_force = Random.Range(2, 8);
		sprite_back = gameObject.transform.Find("EnemyBody").gameObject;
		sprite_face = gameObject.transform.Find("EnemyFace").gameObject;
	}

	// Update is called once per frame
	void Update() {

	}

	// Fixed interval update.
	void FixedUpdate() {
		sprite_back.transform.Rotate(Vector3.forward * rot_force * rot_dir);
	}
}
