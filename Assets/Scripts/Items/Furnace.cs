using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Furnace : MonoBehaviour 
{

	public int currentFuel; //number of seconds left of fuel
	public int maxFuel;
	public int cookTimeRemaining; //number of seconds left until material is cooked
	public int totalCookTime;

	InventoryItem fuelStack;
	InventoryItem materialsStack;
	InventoryItem resultStack;
	
	ItemType resultType;
    //FurnacePanel furnacePanel;
	
	public void ShowInterface()
	{
		/*GameObject furnacePanelObject = (GameObject)GameObject.FindGameObjectWithTag(Tags.furnacePanel);
		furnacePanel = furnacePanelObject.GetComponent<FurnacePanel>();
		furnacePanel.furnace = this;
		InventoryScreenController.current.ShowInventoryScreen(furnacePanelObject);*/
	}
	
	public void SetFuel(InventoryItem item)
	{
		if (item == null)
			fuelStack = null;
		else
		{
			if (fuelStack == null)
				fuelStack = item;
			else
				fuelStack.SetCount(item.Count);
		}
	}
	
	public void SetMaterials(InventoryItem item)
	{
		if (item == null)
		{
			materialsStack = null;
			resultType = ItemType.None;
		}
		
		if (item != null && materialsStack != null &&
			item.ItemType == materialsStack.ItemType)
		{
			materialsStack.SetCount(item.Count);
		}
		else
		{
			materialsStack = item;
			if (materialsStack != null && materialsStack.Count > 0)
			{
				resultType = RecipeList.current.GetItemCookResult(materialsStack.ItemType);
			}
		}
	}
	
	public void ClearResult()
	{
		resultStack = null;
	}
	
	void Start()
	{

	}
	
	float nextTime;
	
	void Update()
	{
		if (currentFuel > 0)//we are burning fuel
		{
			//if one second has passed since last check
			if (Time.time > nextTime)
			{
				currentFuel--;
				if (cookTimeRemaining > 0)
					cookTimeRemaining--;
				
				//if (furnacePanel != null)
					//furnacePanel.UpdateCookingBars();
				
				if (cookTimeRemaining == 0 && materialsStack != null && materialsStack.Count > 0)//finished cooking
				{
					ItemDefinition def = ItemDictionary.currentDictionary.GetItemDefinition(resultType);

					if (resultStack == null)
						resultStack = new InventoryItem(def);
					else
						resultStack.AddItems(1);
					//if (furnacePanel != null)
						//furnacePanel.SetResults(resultStack);
						
					materialsStack.RemoveItems(1);
					if (materialsStack.Count == 0)
						materialsStack = null;
					//if (furnacePanel != null)
						//furnacePanel.SetMaterials(materialsStack);
					
					if (materialsStack != null && materialsStack.Count > 0)
					{
						//totalCookTime = RecipeList.current.GetItemCookTime(materialsStack.ItemType);
						cookTimeRemaining = totalCookTime;
						//if (furnacePanel != null)
							//furnacePanel.SetCookTime(cookTimeRemaining);
					}
				}
			
				if (currentFuel == 0 && materialsStack != null && fuelStack != null && fuelStack.Count > 0)
				{
					//maxFuel = RecipeList.current.GetFuelCookTime(fuelStack.ItemType);
					currentFuel = maxFuel;

					fuelStack.RemoveItems(1);
					if (fuelStack.Count == 0)
						fuelStack = null;
					//if (furnacePanel != null)
						//furnacePanel.SetFuel(fuelStack);
				}
				nextTime = Time.time + 1;
			}
		}
		else //not currently cooking
		{
			if (fuelStack != null && fuelStack.Count > 0)//we have fuel
			{
				if(materialsStack != null && materialsStack.Count > 0) //we have material
				{
					//if result stack is not the same as result of materials, then exit
					if (resultStack != null && resultStack.Count > 0)
					{
						ItemType resultType = RecipeList.current.GetItemCookResult(materialsStack.ItemType);
						if (resultStack.ItemType != resultType) return;
					}
					
					if (currentFuel == 0) //no fuel burning
					{
						maxFuel = RecipeList.current.GetFuelCookTime(fuelStack.ItemType);
						currentFuel = maxFuel;
						//use a fuel
						fuelStack.RemoveItems(1);
						if (fuelStack.Count == 0)
							fuelStack = null;
						//if (furnacePanel != null)
							//furnacePanel.SetFuel(fuelStack);
					}
					
					if (cookTimeRemaining == 0) //no material being cooked right now
					{
						totalCookTime = RecipeList.current.GetItemCookTime(materialsStack.ItemType);
						cookTimeRemaining = totalCookTime;
						//if (furnacePanel != null)
							//furnacePanel.SetCookTime(cookTimeRemaining);
					}
				}
			}
		}
	}
}
