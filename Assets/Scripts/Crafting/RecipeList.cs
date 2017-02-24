using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RecipeList : MonoBehaviour
{
	public Recipe[] Recipies;
	public static RecipeList current;
	//List<Recipe> _Recipies;
	RecipeList()
	{
		//_Recipies = new List<Recipe>();
		
		//_Recipies.Add(new Recipe(ItemType.Furnace, 1, true, 
			//ItemType.Stone, ItemType.Stone, ItemType.None,
			//ItemType.Stone, ItemType.Stone, ItemType.None,
			//ItemType.None, ItemType.None, ItemType.None));
	}
	
	void Start()
	{
		current = this;
	}
	
	public Recipe FindRecipe(CraftingPattern submittedPattern)
	{
		foreach(Recipe rec in Recipies)
		{
			if (rec.MatchesPattern(submittedPattern))
				return rec;
		}
		return null;
	}
	
	public int GetFuelCookTime(ItemType itemType)
	{
		switch(itemType)
		{
			case ItemType.CoalBlock: return 20;
			default : return 0; 
		}
	}
	
	public int GetItemCookTime(ItemType itemType)
	{
		switch(itemType)
		{
			case ItemType.IronBlock : return 5;
			case ItemType.CopperBlock : return 4;
			case ItemType.BauxiteBlock : return 10;
			case ItemType.GoldBlock: return 4;
			case ItemType.SandBlock: return 4;
			default : return 0;
		}
	}
	
	public ItemType GetItemCookResult(ItemType itemType)
	{
		switch(itemType)
		{
			case ItemType.IronBlock: return ItemType.IronIngot;
			case ItemType.CopperBlock : return ItemType.CopperIngot;
			case ItemType.BauxiteBlock : return ItemType.AluminumIngot;
			case ItemType.GoldBlock: return ItemType.GoldIngot;
			case ItemType.SandBlock: return ItemType.GlassBlock;
			default : throw new System.ArgumentOutOfRangeException();
		}
	}
	//last minemech = 53
}
