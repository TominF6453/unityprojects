using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	public float jump = 3.0f;
	public float speed = 1.0f;

	private GameObject player; // Target is always the player.
	private Rigidbody2D body;
	private GameObject rayTarget;

	// Start is called before the first frame update
	void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
		body = GetComponent<Rigidbody2D>();
		rayTarget = gameObject.transform.Find("RayTarget").gameObject;
		gameObject.transform.localScale = gameObject.transform.localScale * 0.5f;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	// Fixed update
	void FixedUpdate() {
		// Mouse Controls (Rewritten to follow player.)
		Vector3 mp = player.transform.position;
		Vector2 unitdir = new Vector2(mp.x - transform.position.x, mp.y - transform.position.y);
		float distance2mouse = unitdir.magnitude;
		unitdir.Normalize();

		float x = Mathf.Sign(unitdir.x);
		//x *= Mathf.Sqrt(Mathf.Abs(unitdir.x));

		if (unitdir.y >= 0.4 && distance2mouse >= 1f) {
			RaycastHit2D hit = Physics2D.Raycast(rayTarget.transform.position, Vector2.down, 0.03f);
			if (hit.collider != null) {
				body.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
			}
		}
		body.velocity = new Vector2(x * speed, body.velocity.y);
		//*/
	}
}
