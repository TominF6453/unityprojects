using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWire : BuildingBase {

	public int wireSum;

	// Use this for initialization
	void Start () {
		CheckSides();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Check adjacent buildings and update material.
	public void CheckSides() {
		BuildingWire x;
		wireSum = 0;
		// Check top.
		if (pos.y != mainGrid.GetSize()) {
			x = (BuildingWire)mainGrid.getBuilding((int)pos.x, (int)pos.y + 1);
			if (x != null && x.IsEqualType(this)) {
				x.wireSum += mainGrid.wireVals["D"];
				x.ChangeMaterial();
				wireSum += mainGrid.wireVals["U"];
			}
		}
		// Check down.
		if (pos.y != 0) {
			x = (BuildingWire)mainGrid.getBuilding((int)pos.x, (int)pos.y - 1);
			if (x != null && x.IsEqualType(this)) {
				x.wireSum += mainGrid.wireVals["U"];
				x.ChangeMaterial();
				wireSum += mainGrid.wireVals["D"];
			}
		}
		// Check left.
		if (pos.x != 0) {
			x = (BuildingWire)mainGrid.getBuilding((int)pos.x - 1, (int)pos.y);
			if (x != null && x.IsEqualType(this)) {
				x.wireSum += mainGrid.wireVals["R"];
				x.ChangeMaterial();
				wireSum += mainGrid.wireVals["L"];
			}
		}
		// Check right.
		if (pos.x != mainGrid.GetSize()) {
			x = (BuildingWire)mainGrid.getBuilding((int)pos.x + 1, (int)pos.y);
			if (x != null && x.IsEqualType(this)) {
				x.wireSum += mainGrid.wireVals["L"];
				x.ChangeMaterial();
				wireSum += mainGrid.wireVals["R"];
			}
		}

		ChangeMaterial();
	}

	// Change the material of the wire object, based on the wireSum value.
	public void ChangeMaterial() {
		GetComponent<MeshRenderer>().material = mainGrid.wires[wireSum];
	}
}
