using UnityEngine;
using System;
using System.Collections;
using System.Xml;
using System.Runtime.Serialization;

public enum ItemType { None, StoneBlock, GrassBlock, DirtBlock, IceBlock, LavaBlock, IronBlock, CopperBlock, DiamondBlock, CoalBlock, GlassBlock, SandBlock, SandstoneBlock, ClayBlock,
    ObsidianBlock, BauxiteBlock, GoldBlock,
	Brick17,Brick18,Brick19,Brick20,Brick21,Brick22,Brick23,Brick24,Brick25,Brick26,Brick27,Brick28,Brick29,Brick30,Brick31,Brick32,Brick33,
	Brick34,Brick35,Brick36,Brick37,Brick38,Brick39,Brick40,
	WorkBench, Furnace, Armor, Booster, IronIngot, CopperIngot, AluminumIngot, GoldIngot, IronPlate, Smelter, StoneDrill, IronDrill, CopperWire,
	IronTube, Laser, IronDoor,Item19,Item20,Item21,Item22,Item23,Item24,Item25,Item26,Item27,Item28,Item29,Item30,Item31,Item32,Item33,
	Item34,Item35,Item36,Item37,Item38,Item39,Item40 };
	
[Serializable]
public class InventoryItem : ICloneable
{
	public float HitDamage = 2.0f; //amound of damage done to enemies if physically hitting it.
	public float Range = 2.0f;
	
	public ItemType _ItemType;
	public bool _IsBrick;
	public float _Durability;
	//bool _IsStackable;
	//public int _MaxCount;
	public int _Count = 1;
	//private int _MaxDurability;
	
	private PlayerIO playerIO;
	private ItemDefinition _Definition;
	
	public InventoryItem(){}//required for serialization
	public InventoryItem(ItemDefinition definition):this()
	{
		_ItemType = definition.ItemType;
		_IsBrick = definition.IsBrick;
		//_MaxDurability = definition.maxDurability;
		_Durability = definition.MaxDurability;
		//_IsStackable = definition.maxSize > 1;
		//_MaxCount = definition.maxSize;
		_Count = 1;
		_Definition = definition;
		
		if (HitDamage == 0)
			HitDamage = 1.0f;
		if (Range == 0)
			Range = 2.0f;
	}
	public InventoryItem(InventoryItem source)
	{
		_ItemType = source.ItemType;
		_IsBrick = source.IsBrick;
		_Durability = source.Durability;
		//_MaxDurability = source._MaxDurability;
		//_MaxCount = source._MaxCount;
		_Count = source.Count;
	}
	
	public ItemType ItemType
	{
		get{return _ItemType;}
	}
	
	public ItemDefinition Definition
	{
		get
		{
			if (_Definition == null && _ItemType != ItemType.None)
				_Definition = ItemDictionary.currentDictionary.GetItemDefinition(_ItemType);
			return _Definition;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether this instance is a brick.
	/// </summary>
	/// <value><c>true</c> if this instance is brick; otherwise, <c>false</c>.</value>
	public bool IsBrick
	{
		get{ return _IsBrick;}
	}
	
	/// <summary>
	/// Gets the durability.
	/// </summary>
	/// <value>The durability.</value>
	public float Durability //current durability
	{
		get{return _Durability;}
	}
	
	/// <summary>
	/// Gets the maximum number of this item that can be stacked.
	/// </summary>
	/// <value>The max count.</value>
	public int MaxCount
	{
		get
		{
			return Definition.MaxStackSize;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether this instance is stackable.
	/// </summary>
	/// <value><c>true</c> if this instance is stackable; otherwise, <c>false</c>.</value>
	public bool IsStackable
	{
		get
		{
			return Definition.MaxStackSize > 1;
		}
	}
	
	/// <summary>
	/// Gets the count of stacked items
	/// </summary>
	/// <value>The count.</value>
	public int Count 
	{
		get{return _Count;}
	}
	
	/// <summary>
	/// ONLY TO BE USED BY CRAFTING AND FURNACE TO MAKE SURE COUNTS ARE CORRECT
	/// </summary>
	/// <param name="count">Count.</param>
	public void SetCount(int count)
	{
		_Count = count;
	}
	
	public void AddItems(int count)
	{
		if (IsStackable && _Count < MaxCount)
			_Count += count;
	}
	
	public void RemoveItems(int count)
	{
		_Count-=count;
	}
	
	/// <summary>
	/// Call this every frame when the item is in use
	/// </summary>
	public virtual void Use()
	{
		//if special item like a weapon, then fire it, otherwise swing it and see if we hit an anemy
		if (playerIO == null)
			playerIO = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>();
		//playerIO.SwingItem(Range, HitDamage);
	}
	
	/// <summary>
	/// Stop using the item
	/// </summary>
	public virtual void StopUsing()
	{
		
	}
	
	#region ICloneable
	public object Clone ()
	{
		return new InventoryItem(this);
	}
	#endregion
}
