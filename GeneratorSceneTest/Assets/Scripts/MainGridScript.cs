using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGridScript : MonoBehaviour {

	// Dictionary of 4-bit binary mappings to the correct wire materials. Up, right, down, left -> 0000.
	//public Dictionary<int, Material> wireMats;

	// Dictionary of values corresponding to adjacent buildings. Used to sum and find correct wire material to use in wireMats.
	public readonly Dictionary<string, int> wireVals = new Dictionary<string, int> {
		{ "U", 8 }, // 1000
		{ "R", 4 }, // 0100
		{ "D", 2 }, // 0010
		{ "L", 1 }, // 0001
	};

	public Material[] wires;

	//public GameObject selectionObj; // The object to place over the selection grid.
	public GameObject toggleObj; // The object to place on selected grid points.
	public Camera cam; // The camera of the scene.
	public Text coords; // Text to edit to check coordinates of selected grid square.

	private Transform trns; // Parent object's transform.
	private float size; // Size as a square, 5x5 = 25 blocks.
	private const float maxSize = 15f; // Maximum size of the grid.
	private BuildingBase[,] gridArray; // The array of buildings in data, handles calculations.
	private GameObject[,] gridArrayObjs; // The array of buildings in the scene, handles sprite changes, removal, visible elements.

	//private BuildingEmpty nothing;

	// Camera vars
	private Vector3 taploc = Vector3.zero;
	private Vector3 campos = Vector3.zero;
	private Vector3 curpos = Vector3.zero;

	private int xpos, ypos;

	// Use this for initialization
	void Start () {
		//nothing = ScriptableObject.CreateInstance<BuildingEmpty>();
		size = 5f;
		gridArray = new BuildingBase[Mathf.FloorToInt(maxSize), Mathf.FloorToInt(maxSize)];
		gridArrayObjs = new GameObject[Mathf.FloorToInt(maxSize), Mathf.FloorToInt(maxSize)];
		trns = gameObject.transform;
		genGrid();

		// Populate wire material dictionary.
		/*wireMats = new Dictionary<int, Material> {
			{ 0, wires[0]		}, // 0000 - None.
			{ 1, wires[1]		}, // 0001 - Left only.
			{ 2, wires[2]		}, // 0010 - Down only.
			{ 3, wires[3]		}, // 0011 - Down & left.
			{ 4, wires[4]		}, // 0100 - Right only.
			{ 5, wires[5]		}, // 0101 - Right and left.
			{ 6, wires[6]		}, // 0110 - Right and down.
			{ 7, wires[7]		}, // 0111 - Right, down and left.
			{ 8, wires[8]		}, // 1000 - Up only.
			{ 9, wires[9]		}, // 1001 - Up and left.
			{ 10, wires[10]		}, // 1010 - Up and down.
			{ 11, wires[11]		}, // 1011 - Up, down and left.
			{ 12, wires[12]		}, // 1100 - Up and right.
			{ 13, wires[13]		}, // 1101 - Up, right and left.
			{ 14, wires[14]      }, // 1110 - Up, right and down.
			{ 15, wires[15]      }, // 1111 - All
		};*/
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		// Check what player is mousing over.
		if (Physics.Raycast(ray, out hit)) {
			if (hit.collider == GetComponent<Collider>()) {
				Vector3 newPos = new Vector3(Mathf.Floor(hit.point.x) + 0.5f, Mathf.Floor(hit.point.y) + 0.5f);
				//selectionObj.transform.position = newPos;
				xpos = Mathf.FloorToInt(newPos.x);
				ypos = Mathf.FloorToInt(newPos.y);
				coords.text = string.Format("({0}, {1})   {2}", xpos, ypos, gridArray[xpos,ypos]);
			}
		} else {
			xpos = -1;
			ypos = -1;
			//selectionObj.transform.position = new Vector3(-100.0f, -100.0f);
			coords.text = string.Format("({0}, {1})", -1, -1);
		}

		// Camera controls.
		if (Input.GetMouseButtonDown(0)) {
			taploc = Input.mousePosition;
			campos = cam.transform.position;
		}

		if (Input.GetMouseButton(0)) {
			curpos = Input.mousePosition;
			DraggingCam();
		}
		//* Camera controls.

		// Handle placing new blocks.
		if (Input.GetMouseButtonUp(0) && xpos > -1 && Input.mousePosition == taploc) {
			if (gridArray[xpos, ypos] == null) {
				//gridArray[xpos, ypos] = ScriptableObject.CreateInstance<BuildingWire>();
				gridArrayObjs[xpos,ypos] = Instantiate(toggleObj, new Vector3(xpos + 0.5f, ypos + 0.5f, -2), new Quaternion());
				gridArrayObjs[xpos, ypos].transform.eulerAngles = new Vector3(0f, 180f, 0f);
				gridArray[xpos, ypos] = gridArrayObjs[xpos, ypos].GetComponent<BuildingBase>();
				gridArray[xpos, ypos].mainGrid = GetComponent<MainGridScript>();
				gridArray[xpos, ypos].pos = new Vector2(xpos, ypos);
			} else if (gridArray[xpos,ypos].IsEqualType(typeof(BuildingWire))) {
				gridArray[xpos, ypos] = null;
				Destroy(gridArrayObjs[xpos, ypos]);
				gridArrayObjs[xpos, ypos] = null;
				FixAdjacent(xpos, ypos);
			}
		}
	}

	void genGrid() {
		trns.localScale = new Vector3(size, size, 1);
		trns.position = new Vector3(size / 2, size / 2, 1);
		for (int x = 0; x < maxSize; x++) {
			for (int y = 0; y < maxSize; y++) {
				gridArray[x, y] = null;
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
		if (size != maxSize) {
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

	public void FixAdjacent(int x, int y) {
		BuildingWire z;
		// Check top.
		if (y != GetSize()) {
			z = (BuildingWire)getBuilding(x, y + 1);
			if (z != null && z.IsEqualType(typeof(BuildingWire))) {
				z.wireSum -= wireVals["D"];
				z.ChangeMaterial();
			}
		}
		// Check down.
		if (y != 0) {
			z = (BuildingWire)getBuilding(x, y - 1);
			if (z != null && z.IsEqualType(typeof(BuildingWire))) {
				z.wireSum -= wireVals["U"];
				z.ChangeMaterial();
			}
		}
		// Check left.
		if (x != 0) {
			z = (BuildingWire)getBuilding(x - 1, y);
			if (z != null && z.IsEqualType(typeof(BuildingWire))) {
				z.wireSum -= wireVals["R"];
				z.ChangeMaterial();
			}
		}
		// Check right.
		if (x != GetSize()) {
			z = (BuildingWire)getBuilding(x + 1, y);
			if (z != null && z.IsEqualType(typeof(BuildingWire))) {
				z.wireSum -= wireVals["L"];
				z.ChangeMaterial();
			}
		}
	}


	// Getters, setters.
	public BuildingBase getBuilding(Vector2 pos) {
		return gridArray[(int)pos.x, (int)pos.y];
	}
	public BuildingBase getBuilding(int x, int y) {
		return gridArray[x, y];
	}
	public float GetSize() {
		return size;
	}
}
