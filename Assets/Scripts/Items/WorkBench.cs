using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorkBench : MonoBehaviour 
{
	//public Stack<InventoryItem>[] slots;
	public InventoryItem[] slots;
	
	// Use this for initialization
	void Start () 
	{
		slots = new InventoryItem[9];
		/*slots = new Stack<InventoryItem>[9];
		slots[0] = new Stack<InventoryItem>();
		slots[1] = new Stack<InventoryItem>();
		slots[2] = new Stack<InventoryItem>();
		slots[3] = new Stack<InventoryItem>();
		slots[4] = new Stack<InventoryItem>();
		slots[5] = new Stack<InventoryItem>();
		slots[6] = new Stack<InventoryItem>();
		slots[7] = new Stack<InventoryItem>();
		slots[8] = new Stack<InventoryItem>();*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
