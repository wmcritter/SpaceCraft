using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour
{
    public ItemType type; //what type of item is this
                          //public GameObject inventory_type_prefab; //if this item is destroyed, what inventory item will it drop
    public bool CanStack; //can we attach other bricks onto this one?

    public void Use()
    {
        switch (type)
        {
            case ItemType.Furnace:
                Furnace f = gameObject.GetComponent<Furnace>();
                f.ShowInterface();
                break;
            case ItemType.WorkBench:
                WorkBench w = gameObject.GetComponent<WorkBench>();
                /*GameObject workBenchPanelObject = (GameObject)GameObject.FindGameObjectWithTag(Tags.workBenchPanel);
                WorkBenchPanel workBenchPanel = workBenchPanelObject.GetComponent<WorkBenchPanel>();
                workBenchPanel.workBench = w;
                workBenchPanel.GetWorkBenchInventory();
                InventoryScreenController.current.ShowInventoryScreen(workBenchPanelObject);*/
                break;
            case ItemType.Smelter:
                Furnace s = gameObject.GetComponent<Furnace>();
                s.ShowInterface();
                break;
            case ItemType.IronDoor://iron door
                /*Door d = gameObject.GetComponent<Door>();
                if (d.IsOpen)
                    d.Close();
                else
                    d.Open();*/
                break;
        }
    }
}
