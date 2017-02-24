using UnityEngine;
using System.Collections;

[System.Serializable]
public class ItemDefinition 
{
	public string Name {get{return ItemType.ToString ();}}
	
	public ItemType ItemType;
    public BlockType BlockType;
	public bool IsBrick { get { return BlockType != BlockType.None; } }
	public Sprite Sprite;
	public int MaxStackSize;
	public GameObject DroppedPrefab;
	public GameObject WorldPrefab;
	public GameObject HeldPrefab;
	public int MaxDurability = 0;
	public float WorldHeight = 1.0f;
	public float WorldWidth = 1.0f;
	
	public override string ToString ()
	{
		return ItemType.ToString ();
	}
}
