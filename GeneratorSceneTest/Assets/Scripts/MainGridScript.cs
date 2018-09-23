using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGridScript : MonoBehaviour {

	public GameObject selectionObj; // The object to place over the selection grid.
	public GameObject toggleObj; // The object to place on selected grid points.
	public Camera cam; // The camera of the scene.
	public Text coords; // Text to edit to check coordinates of selected grid square.

	private Transform trns; // Parent object's transform.
	private float size; // Size as a square, 5x5 = 25 blocks.
	private string[,] gridArray;
	private GameObject[,] gridArrayObjs;

	private int xpos, ypos;

	// Use this for initialization
	void Start () {
		size = 5f;
		gridArray = new string[Mathf.FloorToInt(size), Mathf.FloorToInt(size)];
		gridArrayObjs = new GameObject[Mathf.FloorToInt(size), Mathf.FloorToInt(size)];
		trns = gameObject.transform;
		genGrid();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit)) {
			if (hit.collider == GetComponent<Collider>()) {
				Vector3 newPos = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, Mathf.Floor(hit.point.y) + 0.5f);
				selectionObj.transform.position = newPos;
				xpos = Mathf.FloorToInt(newPos.x);
				ypos = Mathf.FloorToInt(newPos.y);
				coords.text = string.Format("({0}, {1})   {2}", xpos, ypos, gridArray[xpos, ypos]);
			}
		} else {
			xpos = -1;
			ypos = -1;
			selectionObj.transform.position = new Vector3(-100.0f, -100.0f);
			coords.text = string.Format("({0}, {1})", -1, -1);
		}

		if (Input.GetMouseButtonDown(0) && xpos > -1) {
			if (gridArray[xpos, ypos] == "Empty") {
				gridArray[xpos, ypos] = "Selected";
				gridArrayObjs[xpos,ypos] = Instantiate(toggleObj, new Vector3(xpos + 0.5f, ypos + 0.5f, -2), new Quaternion());
			} else if (gridArray[xpos,ypos] == "Selected") {
				gridArray[xpos, ypos] = "Empty";
				Destroy(gridArrayObjs[xpos, ypos]);
				gridArrayObjs[xpos, ypos] = null;
			}
		}
	}

	void genGrid() {
		trns.localScale = new Vector3(size, size);
		trns.position = new Vector3(size / 2, size / 2);
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				gridArray[x, y] = "Empty";
			}
		}
	}
}
