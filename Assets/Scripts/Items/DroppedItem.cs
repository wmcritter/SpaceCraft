using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroppedItem : MonoBehaviour {

	//public Stack<InventoryItem> Items;
	public InventoryItem Item;
	
	//public ItemType ItemType;

	public float rotateSpeed = 10f;
	
	void Awake()
	{
		/*if (Item.ItemType != ItemType.None)
		{
			ItemDefinition def = ItemDictionary.currentDictionary.GetItemDefinition(Item.ItemType);
			if (def == null)
			{
				Debug.Log("Dropped Item: no definition found for ItemType " +Item.ItemType.ToString ());
			}
			//Items = new Stack<InventoryItem>();
			//Items.Push(new InventoryItem(def));
		}
		else
		{
			Debug.Log("Dropped Item: Invalid Item Type");
		}*/
	}
	void Start () 
	{
		Destroy (gameObject, 300);//destroy this after 5 minutes
	}
	
	void Update () {
		transform.Rotate (new Vector3(0, rotateSpeed * Time.deltaTime,0));
	}
	
	/*void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == Tags.player)
		{
			PlayerIO pio = other.gameObject.GetComponent<PlayerIO>();			
			
			if (pio.AddToInventory(Item))
			{
				if (gameObject != null)
					Destroy(gameObject);
				
			}
		}
	}*/
}
