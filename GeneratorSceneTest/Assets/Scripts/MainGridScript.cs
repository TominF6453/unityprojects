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
	private const float maxSize = 25f;
	private string[,] gridArray;
	private GameObject[,] gridArrayObjs;

	private Vector3 taploc = Vector3.zero;
	private Vector3 campos = Vector3.zero;
	private Vector3 curpos = Vector3.zero;

	private int xpos, ypos;

	// Use this for initialization
	void Start () {
		size = 5f;
		gridArray = new string[Mathf.FloorToInt(maxSize), Mathf.FloorToInt(maxSize)];
		gridArrayObjs = new GameObject[Mathf.FloorToInt(maxSize), Mathf.FloorToInt(maxSize)];
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

		if (Input.GetMouseButtonDown(0)) {
			taploc = Input.mousePosition;
			campos = cam.transform.position;
		}

		if (Input.GetMouseButton(0)) {
			curpos = Input.mousePosition;
			DraggingCam();
		}

		if (Input.GetMouseButtonUp(0) && xpos > -1 && Input.mousePosition == taploc) {
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
		trns.localScale = new Vector3(size, size, 1);
		trns.position = new Vector3(size / 2, size / 2, 1);
		for (int x = 0; x < maxSize; x++) {
			for (int y = 0; y < maxSize; y++) {
				gridArray[x, y] = "Empty";
			}
		}

		// Manage material tiling
		MeshRenderer mr = GetComponent<MeshRenderer>();
		Material material = mr.material;
		material.mainTextureScale = new Vector2(size, size);
		// Manage material tiling
	}

	void DraggingCam() {
		Vector3 dir = cam.ScreenToWorldPoint(curpos) - cam.ScreenToWorldPoint(taploc);
		dir *= -1;
		Vector3 pos = campos + dir;
		cam.transform.position = pos;
	}

	public void AddSize() {
		size++;
		trns.localScale = new Vector3(size, size, 1);
		trns.position = new Vector3(size / 2, size / 2, 1);

		// Manage material tiling
		MeshRenderer mr = GetComponent<MeshRenderer>();
		Material material = mr.material;
		material.mainTextureScale = new Vector2(size, size);
		// Manage material tiling
	}
}
