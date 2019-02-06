using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBehaviour : MonoBehaviour
{
	private int frame_count;
	private float rot_force;

	private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
		sprite = GetComponent<SpriteRenderer>();
		frame_count = 0;
		rot_force = Random.Range(-180, 180);
		transform.Rotate(Vector3.forward * rot_force);
    }

    // Update is called once per frame
    void Update()
    {
        switch(frame_count) {
			case 50:
				Destroy(gameObject);
				break;
			default:
				sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.02f);
				break;
		}
		frame_count++;
    }
}
