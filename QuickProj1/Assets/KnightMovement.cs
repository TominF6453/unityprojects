using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : MonoBehaviour
{
	public float jump = 8.0f;
	public float speed = 4.0f;

	private GameObject rayTarget;
	private BoxCollider2D collider;
	private Rigidbody2D body;
	private Transform transform;
	private SpriteRenderer sprite;

	private int animcounter = 0;
	public Sprite idle;
	public Sprite step;

	private GameObject player;

	public GameObject hit;

    // Start is called before the first frame update
    void Start()
    {
		collider = GetComponent<BoxCollider2D>();
		transform = GetComponent<Transform>();
		body = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		player = GameObject.FindGameObjectWithTag("Player");
		rayTarget = gameObject.transform.Find("RayTarget").gameObject;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	// FixedUpdate for physics checks
	void FixedUpdate() {
		/*// Keyboard Controls
		horiz = Input.GetAxis("Horizontal");
		Vector2 movement = new Vector2(horiz, 0f);

		if (Input.GetAxis("Vertical") > 0) {
			RaycastHit2D hit = Physics2D.Raycast(rayTarget.transform.position, Vector2.down, 0.03f);
			if (hit.collider != null) {
				body.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
			}
		}

		body.velocity = new Vector2(movement.x * speed, body.velocity.y);
		//*/

		// Mouse Controls (Rewritten to target closest enemy to player)
		//Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 unitdir = Vector2.zero;
		GameObject enemy = GetClosestEnemyToPlayer();
		if (enemy != null) {
			Vector3 mp = GetClosestEnemyToPlayer().transform.position;
			unitdir = new Vector2(mp.x - transform.position.x, mp.y - transform.position.y);
			float distance2mouse = unitdir.magnitude;
			unitdir.Normalize();

			float x = Mathf.Sign(unitdir.x);
			//if (unitdir.x < 0) x *= -1;
			x *= Mathf.Sqrt(Mathf.Abs(unitdir.x));

			if (unitdir.y >= 0.70 && distance2mouse >= 1f) {
				RaycastHit2D hit = Physics2D.Raycast(rayTarget.transform.position, Vector2.down, 0.03f);
				if (hit.collider != null) {
					body.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
				}
			}
			body.velocity = new Vector2(x * speed, body.velocity.y);
		}
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

		if (Mathf.Abs(unitdir.x) > 0) {
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

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Enemy")) {
			Instantiate(hit, collision.transform.position, Quaternion.identity);
			Destroy(collision.gameObject);
		}
	}

	private GameObject GetClosestEnemyToPlayer() {
		GameObject[] enemies;
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject closest = null;
		float dist = Mathf.Infinity;
		Vector3 pos = player.transform.position;

		foreach (GameObject enemy in enemies) {
			Vector3 diff = enemy.transform.position - pos;
			float curdist = diff.sqrMagnitude;
			if (curdist < dist) {
				closest = enemy;
				dist = curdist;
			}
		}
		return closest;
	}
}
