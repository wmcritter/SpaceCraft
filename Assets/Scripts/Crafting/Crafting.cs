using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crafting : Inventory 
{
	public GameObject[] craftingSlots;
	public GameObject craftingResultSlot;
	
	protected Slot resultSlot;
	
	
	void Awake()
	{
		//force inventory type to crafting
		inventoryType = InventoryType.Crafting;
		slotScripts = new List<Slot>();
	}
	
	void Start()
	{
		allSlots = new List<GameObject>();
		//emptySlots = slots;	
		
		for(int i = 0; i < craftingSlots.Length; i++)
		{
			Slot s = craftingSlots[i].GetComponent<Slot>();
			//set myInventory
			s.myInventory = this;
			slotScripts.Add (s);
			
			allSlots.Add (craftingSlots[i]);
		}
			
		resultSlot = craftingResultSlot.GetComponent<Slot>();
		resultSlot.myInventory = this;
	}
	
	public override void DoCrafting()
	{
		//if (EmptySlots == allSlots.Count)
			resultSlot.ClearSlot();
		//else
		if (EmptySlots != allSlots.Count)
		{
			//get the current item pattern and submit it to see if it matches a recipe
			
			//creat crafting pattern	
			CraftingPattern pattern = null;
			if (slotScripts.Count == 4)
				pattern = new CraftingPattern(
					!slotScripts[0].IsEmpty ? slotScripts[0].Item.ItemType : ItemType.None,
					!slotScripts[1].IsEmpty ? slotScripts[1].Item.ItemType : ItemType.None,
					ItemType.None,
					!slotScripts[2].IsEmpty ? slotScripts[2].Item.ItemType : ItemType.None,
					!slotScripts[3].IsEmpty ? slotScripts[3].Item.ItemType : ItemType.None,
					ItemType.None,
					ItemType.None,
					ItemType.None,
					ItemType.None);
			else if (slotScripts.Count == 9)
				pattern = new CraftingPattern(
					slotScripts[0].IsEmpty ? ItemType.None : slotScripts[0].Item.ItemType,
					slotScripts[1].IsEmpty ? ItemType.None : slotScripts[1].Item.ItemType,
					slotScripts[2].IsEmpty ? ItemType.None : slotScripts[2].Item.ItemType,
					slotScripts[3].IsEmpty ? ItemType.None : slotScripts[3].Item.ItemType,
					slotScripts[4].IsEmpty ? ItemType.None : slotScripts[4].Item.ItemType,
					slotScripts[5].IsEmpty ? ItemType.None : slotScripts[5].Item.ItemType,
					slotScripts[6].IsEmpty ? ItemType.None : slotScripts[6].Item.ItemType,
					slotScripts[7].IsEmpty ? ItemType.None : slotScripts[7].Item.ItemType,
					slotScripts[8].IsEmpty ? ItemType.None : slotScripts[8].Item.ItemType);
				
			//submit crafting pattern and see if we get a result
			Recipe result = RecipeList.current.FindRecipe(pattern);
			if(result != null)
			{
				//populate result to result slot
				resultSlot.ClearSlot ();
				for(int i = 0; i < result.ResultQuantity; i++)
				{
					//GameObject go = (GameObject)GameObject.Instantiate(ItemDictionary.currentDictionary.GetDroppedItemPrefab(result.Result));
					//resultSlot.AddItem(go.GetComponent<Item>());
					ItemDefinition def = ItemDictionary.currentDictionary.GetItemDefinition(result.Result);
					resultSlot.AddItem(new InventoryItem(def));
				}
			}
		}
		
	}
	
	public override void PickupCraftedItems()
	{
		//take one off of each slot
		PopSlots();
		//try crafting again
		DoCrafting();
	}
}
