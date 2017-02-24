using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemDragDrop : MonoBehaviour
{
	public static ItemDragDrop currentItemDragDrop;
	//static properties
	public GameObject hoverObject;
	//public static Slot from, to;
	
	public EventSystem eventSystem;
	private Canvas canvas;
	public GameObject iconPrefab;
	
	private float hoverYOffset;
	//private Stack<InventoryItem> draggingItems;
	private InventoryItem draggingItem;
	
	public bool IsDragging
	{
		get{ return draggingItem != null ;}
	}
	
	void Awake()
	{
		currentItemDragDrop = this;
		hoverYOffset = 5f;//slotSize * 0.03f;
		
	}
	
	void Update()
	{
		/*if (Input.GetMouseButtonUp(0)) //0 = left
		{
			//if not over a game object
			//if (!eventSystem.IsPointerOverGameObject(-1) && draggingItems != null)//mouse pointer = -1
			if (draggingItems != null)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);// new Vector3(0.5f, 0.5f, 0.5f));
				RaycastHit hit;
				//List<RaycastResult> results = new List<RaycastResult>();
				
				Graphic.raycast();
				
				//{
					//if (hit.collider != null && hit.collider.tag == Tags.background)
						//DropToWorld();
				//}
			}
		}*/
		
		if (hoverObject != null && draggingItem != null)
		{
			hoverObject.GetComponent<HoverIcon>().itemCount = draggingItem.Count;
			Vector2 position;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out position);
			position.Set(position.x + hoverYOffset, position.y - hoverYOffset);
			hoverObject.transform.position = canvas.transform.TransformPoint(position);
		}
	}
	
	public void DropToWorld()
	{
		if (draggingItem == null) return;
		//drop into world
		GameObject prefab = ItemDictionary.currentDictionary.GetDroppedItemPrefab(draggingItem.ItemType);
		
		//Vector3 dropPos = new Vector3();
		Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		Vector3 dropPos = (player.transform.position + player.transform.forward * 2);
		
		//Debug.Log("Dropping at " + dropPos);
		
		GameObject dropped = (GameObject)GameObject.Instantiate(prefab, dropPos, Quaternion.identity);
		dropped.GetComponent<DroppedItem>().Item = draggingItem;// = draggingItems.Count;
		
		draggingItem = null;
		//Transform trans = dropped.GetComponent<Transform>();
		
		//Debug.Log("dropped " + dropped.name);
		
		//from.GetComponent<Image>().color = Color.white;
		//from.ClearSlot();
		Destroy(GameObject.Find ("Hover"));
		//to = null;
		//from = null;
		hoverObject = null;
	}
	/*public void MoveItem(GameObject clicked)
	{			
		if (from == null)
		{
			if(!clicked.GetComponent<Slot>().IsEmpty)//if slot not empty
			{
				from = clicked.GetComponent<Slot>();
				
				//from.GetComponent<Image>().color = Color.gray;//grey out image clicked on
				
				hoverObject = (GameObject)Instantiate(iconPrefab);
				hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
				hoverObject.name = "Hover";
				
				RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
				RectTransform clickedTransform = clicked.GetComponent<RectTransform>();
				
				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
				hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
				
				hoverObject.transform.SetParent(GameObject.Find("PlayerUI").transform, true);//put it on the canvas
				hoverObject.transform.localScale = from.gameObject.transform.localScale;//apply local scale from clicked/from object
			}
		}
		else if(to == null)
		{
			Slot slot = clicked.GetComponent<Slot>();
			if (slot.canDrop && (slot.itemFilter == ItemType.None || slot.itemFilter == from.CurrentItem.type))
			{
				to = clicked.GetComponent<Slot>();
				Destroy(GameObject.Find("Hover"));
			}
		}
		
		if (to != null && from != null)
		{
			Stack<Item> tmpTo = new Stack<Item>(to.Items);
			if (to.IsEmpty)
				EmptySlots--;
			to.AddItems(from.Items);
			
			if (tmpTo.Count == 0)
			{
				from.ClearSlot();
			}
			else
			{
				from.AddItems(tmpTo);
			}
			
			from.GetComponent<Image>().color = Color.white; //reset slot so not greyed out
			to = null;
			from = null;
			hoverObject = null;
			
			if (inventoryType == InventoryType.Crafting)
			{
				DoCrafting();
			}
		}
	}*/
	
	public void DropItems(Slot toSlot, bool justOne)
	{
		if (toSlot.CanDrop(draggingItem.ItemType))
		{
			//if slot has items, pick them up
			//Stack<InventoryItem> existingItems;
			InventoryItem existingItem;
			//if (toSlot.Items.Count > 0)
			if (!toSlot.IsEmpty)
			{
				if (toSlot.Item.ItemType == draggingItem.ItemType)
				{
					//add to existing items
					if (justOne)
					{
						if (toSlot.Item.Count < toSlot.ItemDefinition.MaxStackSize)// Items.Peek().maxSize)
						{
							//toSlot.AddItem(draggingItem.Pop());// Items.Push(draggingItems.Pop());
							toSlot.AddItemsToStack(1);
							draggingItem.RemoveItems(1);
						}
					}
					else
					{
						int availableSpace = toSlot.ItemDefinition.MaxStackSize - toSlot.Item.Count;
						int toMove = draggingItem.Count < availableSpace ? draggingItem.Count : availableSpace;
						toSlot.AddItemsToStack(toMove);
						draggingItem.RemoveItems(toMove);
						//while (draggingItem.Count > 0)
						//{
							//if (toSlot.Item.Count < toSlot.ItemDefinition.maxSize)// Items.Peek().maxSize)
							//{
								//toSlot.AddItem(draggingItems.Pop());
								//toSlot.AddItemToStack();
								//draggingItem.RemoveItems(1);
							//}
							//else
								//break;
						//}
						//while (toSlot.Items.Count < toSlot.Items.Peek().maxSize)
						//{
							//toSlot.AddItem(draggingItems.Pop ());// Items.Push(draggingItems.Pop());
						//}
						if (draggingItem.Count == 0)
							draggingItem = null;
					}
					
				}
				else
				{
					//replace existing items
					existingItem = (InventoryItem)toSlot.Item.Clone();// new Stack<InventoryItem>(toSlot.Items);
					Sprite sprite = toSlot.ItemDefinition.Sprite;// Items.Peek().spriteNeutral;
					
					toSlot.ClearSlot();
					toSlot.AddItem(draggingItem);
					draggingItem = existingItem;
					//change hover sprite
					
					hoverObject.GetComponent<Image>().sprite = sprite;
				}
			}
			else
			{
				if (justOne)
				{
					//toSlot.AddItem(draggingItems.Pop());
					if (toSlot.IsEmpty)
						toSlot.AddItem(new InventoryItem(ItemDictionary.currentDictionary.GetItemDefinition(draggingItem.ItemType)));
					else
						toSlot.AddItemsToStack(1);
					draggingItem.RemoveItems(1);
					if (draggingItem.Count == 0)
					{
						draggingItem = null;
					}
				}
				else
				{
					toSlot.AddItem(draggingItem);
					draggingItem = null;
				}
			}
			
			if (draggingItem == null)//.Count == 0)
			{
				//draggingItems = null;
				Destroy(GameObject.Find("Hover"));
			}
			
			if (toSlot.myInventory.inventoryType == InventoryType.Crafting)
			{
				toSlot.myInventory.DoCrafting();
			}
		}
	}
	
	public void PickUpItems(Slot fromSlot, bool justHalf)
	{
		if (fromSlot.Item.Count == 0) return;
		
		//Sprite sprite = fromSlot.gameObject.GetComponent<Image>().sprite;//grab sprite before slot is cleared
		Sprite sprite = fromSlot.itemSprite.sprite;
		
		draggingItem = null;// fromSlot.Item;// new Stack<InventoryItem>();
		if (justHalf)
		{
			//create new inventory item
			draggingItem = new InventoryItem(fromSlot.ItemDefinition);
			int toMove = Mathf.CeilToInt(fromSlot.Item.Count / 2.0f);
			//for(int i = 0; i < toMove; i++)
				//draggingItems.Push(fromSlot.Items.Pop());
			draggingItem.AddItems(toMove-1);
			fromSlot.Item.RemoveItems(toMove);
			fromSlot.UpdateStackText();
		}
		else
		{
			draggingItem = fromSlot.Item;
			//while(fromSlot.Item.Count > 0)
			//{
				//draggingItems.Push(fromSlot.Items.Pop ());
				//draggingItem.AddItems(fromSlot.Item.Count);
				//fromSlot.Item.RemoveItems(fromSlot.Item.Count);
			//}
				
			if (!fromSlot.canDrop)
			{
				fromSlot.myInventory.PickupCraftedItems();
			}			
				
			fromSlot.ClearSlot();
			fromSlot.myInventory.DoCrafting();
		}
		
		if (hoverObject == null)
		{
			hoverObject = (GameObject)Instantiate(iconPrefab);
			hoverObject.GetComponent<Image>().sprite = sprite;// fromSlot.gameObject.GetComponent<Image>().sprite;
			hoverObject.name = "Hover";
		
			RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
			RectTransform clickedTransform = fromSlot.gameObject.GetComponent<RectTransform>();
		
			hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clickedTransform.sizeDelta.x);
			hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clickedTransform.sizeDelta.y);
		
			if (fromSlot.myInventory.inventoryType == InventoryType.PlayerActive)
			{
				canvas = GameObject.Find("PlayerUI").GetComponent<Canvas>();
				hoverObject.transform.SetParent(canvas.transform, true);//put it on the canvas
			}
			else
			{
				canvas = GameObject.Find("InventoryCanvas").GetComponent<Canvas>();
				hoverObject.transform.SetParent(GameObject.Find("PlayerInventory").transform, true);
			}
				
			hoverObject.transform.localScale = fromSlot.gameObject.transform.localScale;//apply local scale from clicked/from object
		}
		//draggingItems = fromSlot.Items;
	}
	
	public void EndHover()
	{
		GameObject hov = GameObject.Find("Hover");
		if (hov != null)
			Destroy(hov);
		//to = null;
		//from = null;
		hoverObject = null;
	}
}
