using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour {

	public MainGridScript mainGrid;
	public Vector2 pos;

	void Start() { }

	void Update() { }
	
	// Check if type of building is equal to another.
	public bool IsEqualType(BuildingBase other) {
		return other.GetType() == this.GetType();
	}

	// Check if type of building is equal to type.
	public bool IsEqualType(System.Type other) {
		return other == this.GetType();
	}
}
