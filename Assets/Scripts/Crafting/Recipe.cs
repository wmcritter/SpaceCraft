using UnityEngine;
using System.Collections;

[System.Serializable]
public class CraftingPattern
{
	public CraftingPattern(){}
	
	public CraftingPattern(ItemType topLeft, ItemType topMid, ItemType topRight,
	                       ItemType midLeft, ItemType midMid, ItemType midRight,
	                       ItemType botLeft, ItemType botMid, ItemType botRight)
	{
		TopLeft = topLeft;
		TopMiddle = topMid;
		TopRight = topRight;
		MiddleLeft = midLeft;
		MiddleMiddle = midMid;
		MiddleRight = midRight;
		BottomLeft = botLeft;
		BottomMiddle = botMid;
		BottomRight = botRight;
	}
	
	//public ItemType[,] pattern = new ItemType[3,3]{ {ItemType.None, ItemType.None, ItemType.None}, {ItemType.None, ItemType.None, ItemType.None}, {ItemType.None, ItemType.None, ItemType.None} };	
	
	public ItemType TopLeft;
	//{
		//get{return pattern[0,0];}
		//set{pattern[0,0] = value;}
	//}
	public ItemType TopMiddle;
	//{
		//get{return pattern[0,1];}
		//set{pattern[0,1] = value;}
	//}
	public ItemType TopRight;
	//{
		//get{return pattern[0,2];}
		//set{pattern[0,2] = value;}
	//}
	public ItemType MiddleLeft;
	//{
		//get{return pattern[1,0];}
		//set{pattern[1,0] = value;}
	//}
	public ItemType MiddleMiddle;
	//{
		//get{return pattern[1,1];}
		//set{pattern[1,1] = value;}
	//}
	public ItemType MiddleRight;
	//{
		//get{return pattern[1,2];}
		//set{pattern[1,2] = value;}
	//}
	public ItemType BottomLeft;
	//{
		//get{return pattern[2,0];}
		//set{pattern[2,0] = value;}
	//}
	public ItemType BottomMiddle;
	//{
		//get{return pattern[2,1];}
		//set{pattern[2,1] = value;}
	//}
	public ItemType BottomRight;
	//{
		//get{return pattern[2,2];}
		//set{pattern[2,2] = value;}
	//}
	
	public ItemType GetPositionByIndex(int index)
	{
		switch(index)
		{
			case 0: return TopLeft;
			case 1: return TopMiddle;
			case 2: return TopRight;
			case 3: return MiddleLeft;
			case 4: return MiddleMiddle;
			case 5: return MiddleRight;
			case 6: return BottomLeft;
			case 7: return BottomMiddle;
			case 8: return BottomRight;
			default: throw new System.ArgumentOutOfRangeException();
		}
	}
	
	public ItemType GetItemTypeByXY(int x, int y)
	{
		switch(x)
		{
			case 0:
				switch(y)
				{
					case 0: return TopLeft;
					case 1: return MiddleLeft;
					case 2: return BottomLeft;
				}
				break;
			case 1:
				switch(y)
				{
					case 0: return TopMiddle;
					case 1: return MiddleMiddle;
					case 2: return BottomMiddle;
				}
				break;
			case 2:
				switch(y)
				{
					case 0: return TopRight;
					case 1: return MiddleRight;
					case 2: return BottomRight;
				}
				break;
		}
		return ItemType.None;
	}
	
	public int Width
	{
		get
		{
			int result = 0;
			if (TopLeft != ItemType.None || MiddleLeft != ItemType.None || BottomLeft != ItemType.None) result ++;
			if (TopMiddle != ItemType.None || MiddleMiddle != ItemType.None || BottomMiddle != ItemType.None) result ++;
			if (TopRight != ItemType.None || MiddleRight != ItemType.None || BottomRight != ItemType.None) result ++;
			return result;
		}
	}
	public int xOffset
	{
		get
		{
			if (TopLeft != ItemType.None || MiddleLeft != ItemType.None || BottomLeft != ItemType.None) return 0;
			if (TopMiddle != ItemType.None || MiddleMiddle != ItemType.None || BottomMiddle != ItemType.None) return 1;
			if (TopRight != ItemType.None || MiddleRight != ItemType.None || BottomRight != ItemType.None) return 2;
			return 0;
		}
	}
	public int Height
	{
		get
		{
			int result = 0;
			if (TopLeft != ItemType.None || TopMiddle != ItemType.None || TopRight != ItemType.None) result++;
			if (MiddleLeft != ItemType.None || MiddleMiddle != ItemType.None || MiddleRight != ItemType.None) result++;
			if (BottomLeft != ItemType.None || BottomMiddle != ItemType.None || BottomRight != ItemType.None) result++;
			return result;
		}
	}
	public int yOffset
	{
		get
		{
			if (TopLeft != ItemType.None || TopMiddle != ItemType.None || TopRight != ItemType.None) return 0;
			if (MiddleLeft != ItemType.None || MiddleMiddle != ItemType.None || MiddleRight != ItemType.None) return 1;
			if (BottomLeft != ItemType.None || BottomMiddle != ItemType.None || BottomRight != ItemType.None) return 2;
			return 0;
		}
	}
	public int IngredientCount //totol number of ingredients needed
	{
		get
		{
			int result = 0;
			if (TopLeft != ItemType.None) result++;
			if (TopMiddle != ItemType.None) result++;
			if (TopRight != ItemType.None) result++;
			if (MiddleLeft != ItemType.None) result++;
			if (MiddleMiddle != ItemType.None) result++;
			if (MiddleRight != ItemType.None) result++;
			if (BottomLeft != ItemType.None) result++;
			if (BottomMiddle != ItemType.None) result++;
			if (BottomRight != ItemType.None) result++;
			return result;
		}
	}
}

[System.Serializable]
public class Recipe 
{
	//public Recipe(){Pattern = new CraftingPattern();}
	public Recipe(ItemType result, int resultQuality, bool shapeMatch,
	              ItemType topLeft, ItemType topMid, ItemType topRight,
	              ItemType midLeft, ItemType midMid, ItemType midRight,
	              ItemType botLeft, ItemType botMid, ItemType botRight)
	{
		Result = result;
		ResultQuantity = resultQuality;
		ShapeMatch = shapeMatch;
		Pattern = new CraftingPattern(topLeft, topMid, topRight, 
			midLeft, midMid, midRight,
			botLeft, botMid, botRight);
	}
	
	public ItemType Result;
	public int ResultQuantity;
	public bool ShapeMatch;
	//public bool MustCook;
	//pattern must always start in top left
	public CraftingPattern Pattern;
	
	public bool MatchesPattern(CraftingPattern submitted)
	{
		if (submitted.IngredientCount == this.Pattern.IngredientCount)
		{
			if (submitted.Width == this.Pattern.Width &&
				submitted.Height == this.Pattern.Height)
			{
				if (this.ShapeMatch)
				{
					int xOffset = submitted.xOffset;
					int yOffset = submitted.yOffset;
					
					for(int x = 0; x < 3; x++)
					{
						if (x + xOffset > 2) continue;
						
						for(int y = 0; y < 3; y++)
						{
							if (y + yOffset > 2) continue;
								
							//if (submitted.pattern[x + xOffset, y + yOffset] != this.Pattern.pattern[x,y])
							if (submitted.GetItemTypeByXY(x + xOffset, y + yOffset) != this.Pattern.GetItemTypeByXY(x, y))
								return false;
						}
					}
					return true;
				}
				else //don't match shape, just ingredient list
				{
					int foundCount = 0;
					for(int i = 0; i < submitted.IngredientCount; i++)
					{
						ItemType typeByIndex = submitted.GetPositionByIndex(i);
						if (typeByIndex == ItemType.None)
							continue;
						for(int j = 0; j < 9; j++)
						{
							if ( typeByIndex == this.Pattern.GetPositionByIndex(j))
							{
								foundCount ++;
								break;
							}
						}
					}
					if (foundCount == submitted.IngredientCount)
						return true;
				}
			}
		}
		return false;
	}
	

}
