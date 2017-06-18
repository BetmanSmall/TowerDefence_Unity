using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {
	public int x{ get; private set;}
	public int y{ get; private set;}
	public int z{ get; private set;}

	public bool empty, terrain;

	public Cell(int x, int y, int z) {
		this.x = x;
		this.y = y;
		this.z = z;

		empty = true;
		terrain = false;
	}

	public bool setTerrain() {
		if (empty) {
			terrain = true;
			empty = false;
			return true;
		}
		return false;
	}
}
