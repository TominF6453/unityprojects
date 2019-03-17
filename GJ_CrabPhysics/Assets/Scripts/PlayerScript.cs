using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Vars
    public float movespeed;

	private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
		body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		
	}

	// Physics based update.
	private void FixedUpdate() {
		/* Movement */
		float xforce = Input.GetAxis("Horizontal");
		float yforce = Input.GetAxis("Vertical");

		Vector3 move = new Vector3(movespeed * xforce, 0f, movespeed * yforce);
		//transform.position += move;
		body.MovePosition(transform.position + move);
		body.velocity = Vector3.zero;
		/* --- END --- */

		/* Look at Mouse */
		Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
		Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
		lookPos.y = 1f;
		transform.LookAt(lookPos);
		/* --- END --- */
	}

	// Called after update, but once per frame
	private void LateUpdate() {
		
	}
}
