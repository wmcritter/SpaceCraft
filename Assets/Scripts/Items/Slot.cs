using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Slot : MonoBehaviour, IPointerClickHandler 
{
	public UnityEngine.UI.Text stackText; //the text UI element that will display the count of items
	public Image itemSprite; //the image that will display the item's sprite
	public Sprite slotEmpty; //the sprite that will be displayed when the slot is neutral
	public UIBarScript durabilityBar; //the bar that will display an item's durability
	//public Sprite slotHighlight; //the sprite that will be displayed when the slot is highlighted
	public ItemType[] itemFilter; //only accept items that match this list
	public bool canDrop = true; //can you drop items into this slot? must be false for craft result slots
	public Inventory myInventory;
	//public ItemDefinition ItemDefinition;
	
	//private Stack<InventoryItem> _items;
	private InventoryItem _Item;
	public InventoryItem Item
	{
		get{return _Item;}
	}
	
	public ItemDefinition ItemDefinition
	{
		get{return _Item == null ? null : _Item.Definition;}
	}
	/*public Stack<InventoryItem> Items
	{
		get{return _items;}
		set
		{
			_items = value;
			if (_items != null && _items.Count > 0)
			{
				ItemDefinition = ItemDictionary.currentDictionary.GetItemDefinition(_items.Peek().ItemType);
				ChangeSprite(ItemDefinition.sprite);
			}
			else
				ChangeSprite(slotEmpty);
			
			UpdateStackText();
		}
	}*/
	
	public bool IsEmpty
	{
		//get{return Items == null || Items.Count == 0;}
		get{return _Item == null;}
	}
	
	/*public InventoryItem CurrentItem
	{
		get {return (Items == null || Items.Count == 0) ? null : Items.Peek(); }
	}*/
	
	public bool IsAvailable
	{
		//get{ return ItemDefinition.maxSize > Items.Count;}
		get{return IsEmpty || _Item.Count < _Item.MaxCount;}
	}
	
	void Awake()
	{
		//_items = new Stack<InventoryItem>();
	}
	
	void Start () 
	{		
		RectTransform slotRect = GetComponent<RectTransform>();
		RectTransform txtRect = stackText.GetComponent<RectTransform>();
		
		int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60f); //text will be 60% of slot
		stackText.resizeTextMaxSize = txtScaleFactor;
		stackText.resizeTextMinSize = txtScaleFactor;
		
		txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
		txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
	}
	
	public void SetItem(InventoryItem item)
	{
		_Item = item;
		ChangeSprite(_Item == null ? slotEmpty : ItemDefinition.Sprite);	
		UpdateStackText();
	}
	
	public void AddItem(InventoryItem item)
	{
		if (item == null) return;
		//if (Items == null)
			//_items = new Stack<InventoryItem>();
		if (_Item == null) //currently empty
		{
			_Item = item;
			//ItemDefinition = ItemDictionary.currentDictionary.GetItemDefinition(item.ItemType);			
		}
		//Items.Push (item);//put on top
		else //not empty
		{
			if (_Item.ItemType != item.ItemType)//item types don't match, log error
			{
				Debug.Log("Inventory Item(AddItem): item types don't match");
				return;
			}
			
			if (_Item.IsStackable)
			{
				_Item.AddItems(item.Count);
			}
		}
								
		//if (ItemDefinition.itemType != item.ItemType)
		//{
			//ItemDefinition = ItemDictionary.currentDictionary.GetItemDefinition(item.ItemType);			
		//}
		//if (ItemDefinition == null)
			//ItemDefinition = ItemDictionary.currentDictionary.GetItemDefinition(_Item.ItemType);
		ChangeSprite(ItemDefinition.Sprite);	
		UpdateStackText();
	}
	
	public void AddItemsToStack(int count)
	{
		if (_Item == null)
		{
			Debug.Log("Slot.AddItemToStack: no item present, use AddItem() instead");
			return;
		}
		_Item.AddItems(count);
		UpdateStackText();
	}
	
	/*public void AddItems(Stack<InventoryItem> items)
	{
		this.Items = new Stack<InventoryItem>(items);
	}*/
	
	private void ChangeSprite(Sprite neutral)
	{
		if (itemSprite != null)
			itemSprite.sprite = neutral;
	}
	
	//private void UseItem()
	private void ConsumeItem()
	{
		if (!IsEmpty)
		{
			//Items.Pop().Use();//remove item from top of list and use it
			_Item.RemoveItems(1);
			if (_Item.Count == 0)
				_Item = null;
			
			UpdateStackText();
			
			if (IsEmpty)
			{
				ChangeSprite(slotEmpty); //revert to standard sprites.
				//myInventory.EmptySlots++;
			}
		}
	}
	
	public void PopItem()
	{
		//if (Items != null && Items.Count > 0)
		if (!IsEmpty)
		{
			//Items.Pop ();
			_Item.RemoveItems(1);
			if (_Item.Count == 0)
				_Item = null;
			UpdateStackText();
			if (IsEmpty)
				ChangeSprite(slotEmpty);
		}
	}
	
	public void UpdateStackText()
	{
		//stackText.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;	
		stackText.text = (_Item != null && _Item.Count > 1) ? _Item.Count.ToString() : string.Empty;
	}
	
	public void ClearSlot()
	{
		//if (Items != null)
		if (!IsEmpty)
		{
			//Items.Clear();
			_Item = null;
			ChangeSprite(slotEmpty);
			UpdateStackText();
		}
	}
	
	private bool MatchesFilter(ItemType matchType)
	{
		if (this.itemFilter.Length == 0) return true;
		
		foreach(ItemType t in itemFilter)
		{
			if (t == matchType)
				return true;
		}
		
		return false;
	}
	
	public bool CanDrop(ItemType dropType)
	{
		return canDrop && MatchesFilter(dropType);
	}
	
	#region IPointerClickHandler
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.clickCount == 2)
		{
		}
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (ItemDragDrop.currentItemDragDrop.IsDragging)
			{
				ItemDragDrop.currentItemDragDrop.DropItems(this, false);
			}
			else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
			{
				//this.myInventory.MoveToNextInventory(this, this.myInventory);
			}
			else
			{
				//if (Items.Count > 0)
				if (_Item != null && _Item.Count > 0)
					ItemDragDrop.currentItemDragDrop.PickUpItems(this, false);
			}
		}
		else if(eventData.button == PointerEventData.InputButton.Right) 
		{
			if (ItemDragDrop.currentItemDragDrop.hoverObject != null)
			{
				ItemDragDrop.currentItemDragDrop.DropItems(this, true);
				//this.AddItem(ItemDragDrop.from.items.Pop ());
				//if (ItemDragDrop.from.items.Count == 0)
					//ItemDragDrop.EndHover();
			}
			else //pick up half
			{
				//if (Items.Count > 0)
				if (_Item != null && _Item.Count > 0)
					ItemDragDrop.currentItemDragDrop.PickUpItems(this, true);
			}
			//else	
				//UseItem();
		}
	}
	#endregion
}
