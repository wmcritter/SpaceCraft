using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public enum InventoryType {PlayerMain, PlayerActive, Crafting}

[Serializable]
public class Inventory : MonoBehaviour 
{
	public int rows = 5;
	public int slots = 10;
	public float slotPaddingLeft, slotPaddingTop = 2;
	public float slotSize;
	public GameObject slotPrefab;
	//public GameObject iconPrefab;
	//public Canvas canvas;
	//public EventSystem eventSystem;
	public GameObject selectedSlot;
	public InventoryType inventoryType;
	//private RectTransform inventoryRect;	
	//private float inventoryWidth, inventoryHeight; 	
	protected List<GameObject> allSlots;
	protected List<Slot> slotScripts;
	
	public int EmptySlots
	{
		//get{return emptySlots;}
		get
		{
			int count = 0;
			foreach(GameObject obj in allSlots)
			{
				if (obj.GetComponent<Slot>().IsEmpty)
					count++;
			}
			return count;
		}
		//set{emptySlots = value;}
	}
	
	public void SetAlpha(int alpha)
	{
		CanvasGroup cg = GetComponent<CanvasGroup>();
		cg.alpha = alpha;
	}
	
	void Start()
	{
		CreateLayout();
		
		/*if (GameController.currentController.SavedGame != null)
		{
			switch(inventoryType)
			{
				case InventoryType.PlayerMain:
					LoadInventoryItems(GameController.currentController.SavedGame.PlayerInventoryItems);
					break;
				case InventoryType.PlayerActive:
					LoadInventoryItems(GameController.currentController.SavedGame.ActiveInventoryItems);
					break;
			}
		}*/
	}
	
	private void CreateLayout()
	{
		//Instantiates the allSlot's list
		allSlots = new List<GameObject>();
		//emptySlots = slots;	
	
		if (inventoryType == InventoryType.Crafting) return;//don't create layout for crafting 
		
		if (inventoryType == InventoryType.PlayerActive)
		{
			//create selected slot sprite
			selectedSlotGameObject = (GameObject)GameObject.Instantiate(selectedSlot);
			selectedSlotGameObject.transform.SetParent(this.transform);
			RectTransform selectedSlotRect = selectedSlotGameObject.GetComponent<RectTransform>();
			//Sets the slots position
			selectedSlotRect.anchoredPosition = new Vector3(0,0);
		
			//Sets the size of the slot
			selectedSlotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize + (slotPaddingLeft*2));
			selectedSlotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize + (slotPaddingTop*2));
            selectedSlotRect.localScale = new Vector3(1, 1, 1);
		}
		
		//Calculates the width of the inventory
		/*inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
		
		//Calculates the highs of the inventory
		inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;
		
		//Creates a reference to the inventory's RectTransform
		inventoryRect = GetComponent<RectTransform>();
		
		//Sets the with and height of the inventory.
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);*/
		
		//Calculates the amount of columns
		int columns = slots / rows;
		
		for (int y = 0; y < rows; y++) //Runs through the rows
		{
			for (int x = 0; x < columns; x++) //Runs through the columns
			{   
				//Instantiates the slot and creates a reference to it
				GameObject newSlot = (GameObject)Instantiate(slotPrefab);
				//set myInventory property
				newSlot.GetComponent<Slot>().myInventory = this;
				
				//Makes a reference to the rect transform
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();
				
				//Sets the slots name
				newSlot.name = "Slot" + x + y;
				
				//Sets the canvas as the parent of the slots, so that it will be visible on the screen
				
				newSlot.transform.SetParent(this.transform/*.parent*/);
				
				//Sets the slots position
				slotRect.anchoredPosition = /*inventoryRect.localPosition +*/ new Vector3(slotPaddingLeft * (x + 1) + (slotSize * x), -slotPaddingTop * (y + 1) - (slotSize * y));
				
				//Sets the size of the slot
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                slotRect.localScale = new Vector3(1, 1, 1);

				//Adds the new slots to the slot list
				allSlots.Add(newSlot);
				
			}
		}
	}
	
	public bool AddItem(InventoryItem item)
	{
		ItemDefinition def = ItemDictionary.currentDictionary.GetItemDefinition(item.ItemType);
		if (def.MaxStackSize <= 1)
		{
			return PlaceEmpty(item);
			//return true;
		}
		else //item is stackable
		{
			foreach(GameObject slot in allSlots)
			{
				Slot tmp = slot.GetComponent<Slot>();
				if (!tmp.IsEmpty)
				{
					if (tmp.Item.ItemType == def.ItemType && tmp.IsAvailable)
					{
						while (tmp.IsAvailable && item.Count > 0)
						{
							tmp.AddItemsToStack(1);//(items.Pop());
							item.RemoveItems(1);
						}
						//we filled up current stack, if we have more items try to add them to another slot
						if (item.Count > 0)
							return AddItem(item);
						else
							return true;
					}
				}
			}
			
			if (EmptySlots > 0)
			{
				return PlaceEmpty(item);
				//return true;
			}
		}
		
		return false;
	}
	
	/*public bool AddItems(Stack<InventoryItem> items)
	{
		ItemDefinition def = ItemDictionary.currentDictionary.GetItemDefinition(items.Peek().ItemType);
		if (def.maxSize <= 1)
		{
			//if maxsize is 1 then items can only have one, and it must be placed in an empty slot
			return PlaceEmpty(items.Pop ());
		}
		else
		{
			foreach(GameObject slot in allSlots)
			{
				Slot tmp = slot.GetComponent<Slot>();
				if (!tmp.IsEmpty)
				{
					if (tmp.Item.ItemType == def.itemType && tmp.IsAvailable)
					{
						while (tmp.IsAvailable && items.Count > 0)
						{
							tmp.AddItem(items.Pop());
						}
						//we filled up current stack, if we have more items try to add them to another slot
						if (items.Count > 0)
							return AddItems (items);
						else
							return true;
					}
				}
			}
			
			if (EmptySlots > 0)
			{
				return PlaceEmpty(items);
				//return true;
			}
		}
		
		return false;
	}*/
	
	public void RemoveItem(InventoryItem item)
	{
		foreach(GameObject slot in allSlots)
		{
			Slot tmp = slot.GetComponent<Slot>();
			if (tmp.IsEmpty)
				continue;
			if (tmp.Item.ItemType == item.ItemType)
			{
				if (tmp.Item.Count > 1)
				{
					tmp.PopItem ();
					//tmp.UpdateStackText();
				}
				else
					tmp.ClearSlot();
				return;
			}
		}
	}
	
	public void RemoveFromSlot(int index, InventoryItem item)
	{
		allSlots[index].GetComponent<Slot>().PopItem();
	}
	
	private bool PlaceEmpty(InventoryItem item)
	{
		if (EmptySlots > 0)
		{
			foreach(GameObject slot in allSlots)
			{
				Slot tmp = slot.GetComponent<Slot>();
				
				if (tmp.IsEmpty)
				{
					tmp.AddItem(item);
					//emptySlots--;
					return true;
				}
			}
		}
		return false;
	}
	
	/*private bool PlaceEmpty(Stack<InventoryItem> items)
	{
		if (EmptySlots > 0)
		{
			foreach(GameObject slot in allSlots)
			{
				Slot tmp = slot.GetComponent<Slot>();
				
				if (tmp.IsEmpty)
				{
					//tmp.AddItems(items);
					tmp.AddItem(item);
					return true;
				}
			}
		}
		return false;
	}*/
	
	public virtual void DoCrafting()
	{
	}
	
	public virtual void PickupCraftedItems()
	{}
	
	public InventoryItem GetItemFromSlot(int slotIndex)
	{		
		if (slotIndex < allSlots.Count)
		{
			Slot slot = allSlots[slotIndex].GetComponent<Slot>();
			if (!slot.IsEmpty)
				return slot.Item;
		}
		else
			throw new System.ArgumentOutOfRangeException();
			
		return null;
	}
	
	/*public Stack<InventoryItem> GetItemsFromSlot(int slotIndex)
	{
		if (slotIndex < allSlots.Count)
		{
			Slot slot = allSlots[slotIndex].GetComponent<Slot>();
			return slot.Items;
		}
		else
			throw new System.ArgumentOutOfRangeException();
			
		return null;
	}*/
	
	GameObject selectedSlotGameObject;
	GameObject itemInHand;
	public void SelectSlot(int slotIndex)
	{
		//clear last selected item
		if (itemInHand != null)
		{
			Destroy (itemInHand);
			itemInHand = null;
		}
		
		if (slotIndex < allSlots.Count)
		{
			Slot slot = allSlots[slotIndex].GetComponent<Slot>();
			
			RectTransform rect = selectedSlotGameObject.GetComponent<RectTransform>();
			RectTransform slotRect = slot.GetComponent<RectTransform>();
			rect.position = (slotRect.position + new Vector3((slotPaddingLeft * -1), (slotPaddingTop)));
			
			////place item in hand
			//if (slot.Item != null)
			if (!slot.IsEmpty)
			{
				Transform player = GameObject.FindGameObjectWithTag("Player").transform;
				Transform prefabTransform = null;
				
				if (slot.ItemDefinition.IsBrick || slot.ItemDefinition.HeldPrefab != null)
				{
					if (slot.ItemDefinition.IsBrick)
					{
						itemInHand = (GameObject)Instantiate(ItemDictionary.currentDictionary.heldBrickPrefab, Vector3.zero, Quaternion.identity);
						prefabTransform = ItemDictionary.currentDictionary.heldBrickPrefab.transform;
						//if brick, then set brick texture
						MeshRenderer mr = itemInHand.GetComponent<MeshRenderer>();
						Material mat = mr.sharedMaterials[0];
						//itemInHand.GetComponent<MeshRenderer>().materials[0] = 
							//slot.ItemDefinition.droppedPrefab.GetComponent<MeshRenderer>().materials[0];
						MeshRenderer smr = slot.ItemDefinition.DroppedPrefab.GetComponent<MeshRenderer>();
						Material smat = smr.sharedMaterials[0];
						
						mr.sharedMaterial = smat; 
					}
					else
					{
						itemInHand = (GameObject)Instantiate(slot.ItemDefinition.HeldPrefab, Vector3.zero, Quaternion.identity);
						prefabTransform = slot.ItemDefinition.HeldPrefab.transform;
					}
						
					itemInHand.transform.SetParent(player);
					itemInHand.transform.localPosition = new Vector3(-0.681f, -0.547f, 1.57f);
					itemInHand.transform.localRotation = prefabTransform.localRotation;
				}
			}
		}
	}
	
	public void Clear()
	{
		foreach(GameObject obj in allSlots)
		{
			obj.GetComponent<Slot>().ClearSlot();
		}
	}
	
	//public void AddToSlot(int index, Stack<InventoryItem> items)
	public void AddToSlot(int index, InventoryItem item)
	{
		allSlots[index].GetComponent<Slot>().AddItem(item);
	}
	
	public void CopyToInventory(Inventory destination)
	{
		//clear destination slots
		destination.Clear();
		//copy slots to destination
		for(int i = 0; i < allSlots.Count; i++)
		{
			GameObject obj = allSlots[i];
			Slot slot = obj.GetComponent<Slot>();		
			destination.AddToSlot(i, slot.Item);
		}
	}
	
	public virtual void MoveToNextInventory(Slot fromSlot, Inventory target)
	{
	}
	
	public void PopSlots()
	{
		foreach(GameObject obj in allSlots)
		{
			Slot s = obj.GetComponent<Slot>();
			if (s.canDrop)
				s.PopItem();
		}
	}
	
	public List<InventoryItem> GetInventoryItems()
	{
		List<InventoryItem> result = new List<InventoryItem>();
		foreach(GameObject obj in allSlots)
		{
			result.Add( obj.GetComponent<Slot>().Item);
		}
		return result;
	}
	
	public void LoadInventoryItems(List<InventoryItem> list)
	{
		for(int i = 0; i < list.Count; i++)
		{
			//List<InventoryItem> stack = list[i];
			InventoryItem item = list[i];
			Slot slot = allSlots[i].GetComponent<Slot>();
			//foreach(InventoryItem item in stack)
			//{
				slot.AddItem(item);
			//}
		}
	}
}
