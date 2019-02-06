using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float jump = 6.0f;
	public float speed = 3.0f;

	private float horiz;

	private GameObject rayTarget;
	private BoxCollider2D collider;
	private Rigidbody2D body;
	private Transform transform;
	private SpriteRenderer sprite;

	private int animcounter = 0;
	public Sprite idle;
	public Sprite step;

	public GameObject enemy;

	// Start is called before the first frame update
	void Start() {
		collider = GetComponent<BoxCollider2D>();
		transform = GetComponent<Transform>();
		body = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		rayTarget = gameObject.transform.Find("RayTarget").gameObject;
	}

	// Update is called once per frame
	void Update() {
		//Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//if (Input.GetMouseButtonDown(0)) {
		//	Instantiate(enemy, new Vector3(mp.x, mp.y, 0f), Quaternion.identity);
		//}
	}

	// FixedUpdate for physics checks
	void FixedUpdate() {
		// Keyboard Controls
		horiz = Input.GetAxis("Horizontal");
		Vector2 movement = new Vector2(horiz, 0f);

		if (Input.GetAxis("Vertical") > 0) {
			RaycastHit2D hit = Physics2D.Raycast(rayTarget.transform.position, Vector2.down, 0.03f);
			if (hit.collider != null && hit.collider.isTrigger == false) {
				body.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
			}
		}

		body.velocity = new Vector2(movement.x * speed, body.velocity.y);
		//*/

		/*// Mouse Controls
		Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 unitdir = new Vector2(mp.x - transform.position.x, mp.y - transform.position.y);
		float distance2mouse = unitdir.magnitude;
		unitdir.Normalize();

		float x = 1;
		if (unitdir.x < 0) x *= -1;
		x *= Mathf.Sqrt(Mathf.Abs(unitdir.x));

		if (unitdir.y >= 0.70 && distance2mouse >= 1f) {
			RaycastHit2D hit = Physics2D.Raycast(rayTarget.transform.position, Vector2.down, 0.03f);
			if (hit.collider != null) {
				body.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
			}
		}
		body.velocity = new Vector2(x * speed, body.velocity.y);
		//*/

		// Animation
		Vector2 curdir = body.velocity;
		if (curdir.x < 0 && transform.localScale.x > 0) {
			//sprite.flipX = true;
			transform.localScale = new Vector3(-1, 1, 1);
		} else if (curdir.x > 0 && transform.localScale.x < 0) {
			//sprite.flipX = false;
			transform.localScale = new Vector3(1, 1, 1);
		}

		if (Mathf.Abs(horiz) > 0) {
			animcounter++;
		} else {
			animcounter = 0;
			sprite.sprite = idle;
		}
		if (animcounter >= 5) {
			if (sprite.sprite == idle) {
				sprite.sprite = step;
			} else {
				sprite.sprite = idle;
			}
			// Reset
			animcounter = 0;
		}
	}
}
