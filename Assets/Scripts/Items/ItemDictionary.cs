using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemDictionary : MonoBehaviour
{
	public static ItemDictionary currentDictionary;
	
	public GameObject heldBrickPrefab;
	
	public ItemDefinition[] definitions;
	
	void Awake()
	{
		currentDictionary = this;
	}
	
	public ItemDefinition GetItemDefinition(ItemType itemtype)
	{
		foreach(ItemDefinition iDef in definitions)
		{
			if (iDef.ItemType == itemtype) 
				return iDef;
		}
		return null;
	}

    public ItemDefinition GetItemDefinition(BlockType block)
    {
        foreach (ItemDefinition iDef in definitions)
        {
            if (iDef.BlockType == block)
                return iDef;
        }
        return null;
    }

    public bool IsBrick(ItemType itemType)
	{
		ItemDefinition def = GetItemDefinition(itemType);
		if (def != null)
			return def.IsBrick;
		else
			throw new System.ArgumentOutOfRangeException();
	}
	
	public GameObject GetDroppedItemPrefab(ItemType item)
	{
		ItemDefinition def = GetItemDefinition(item);
		if (def!= null)
			return def.DroppedPrefab;
		else
			throw new System.ArgumentOutOfRangeException(item.ToString());
	}

    public GameObject GetDroppedItemPrefab(BlockType block)
    {
        ItemDefinition def = GetItemDefinition(block);
        if (def != null)
            return def.DroppedPrefab;
        else
            throw new System.ArgumentOutOfRangeException(block.ToString());
    }
}



