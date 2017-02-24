using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerIO : MonoBehaviour {
    GameObject inventory;
    GameObject reticle;
    GameObject flashLight;
    GameObject menuObject;
    Animator inventoryAnimator;
    PlayerMovement movement;
    MouseLook mouseLook;
    bool isMenuOpen = false;
    bool isInventoryOpen = false;
    bool isFlashlightOn = false;

    Inventory activeInventory;

    public float MaxRange = 5f;

    void Start()
    {
        menuObject = GameObject.FindGameObjectWithTag("GameMenu");
        //menuAnimator = menu.GetComponent<Animator>();

        inventory = GameObject.FindGameObjectWithTag("InventoryUI");
        inventoryAnimator = inventory.GetComponent<Animator>();
        activeInventory = inventory.GetComponent<Inventory>();

        reticle = GameObject.FindGameObjectWithTag("Reticle");
        flashLight = GameObject.FindGameObjectWithTag("Flashlight");

        //man = menu.GetComponent<PanelManager>();
        movement = gameObject.GetComponent<PlayerMovement>();
        mouseLook = gameObject.GetComponent<MouseLook>();
        
    }

    private int _CurrentInventoryIndex = 1;
    public int CurrentInventoryIndex
    {
        get { return _CurrentInventoryIndex; }
        set
        {
            _CurrentInventoryIndex = value;
            activeInventory.GetComponent<Inventory>().SelectSlot(_CurrentInventoryIndex - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if game menu is showing then close, else open
            if (isMenuOpen)
            {
                //close menu
                UIUtils.HideCanvasGroup(menuObject.GetComponent<CanvasGroup>());
                isMenuOpen = false;
                movement.Disabled = isInventoryOpen || isMenuOpen;
                mouseLook.Disabled = isInventoryOpen || isMenuOpen;
                reticle.GetComponent<UnityEngine.UI.Image>().enabled = true;

            }
            else
            {
                //open menu
                UIUtils.ShowCanvasGroup(menuObject.GetComponent<CanvasGroup>());
                isMenuOpen = true;
                movement.Disabled = isInventoryOpen || isMenuOpen;
                mouseLook.Disabled = isInventoryOpen || isMenuOpen;
                reticle.GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInventoryOpen)
            {
                //close inventory
                inventoryAnimator.enabled = true;
                inventoryAnimator.Play("PanelSlideDown");
                isInventoryOpen = false;
                movement.Disabled = isInventoryOpen || isMenuOpen;
                mouseLook.Disabled = isInventoryOpen || isMenuOpen;
                reticle.GetComponent<UnityEngine.UI.Image>().enabled = true;
            }
            else //open inventory
            {
                inventoryAnimator.enabled = true;
                inventoryAnimator.Play("PanelSlideUp");
                //inventoryAnimator.enabled = false;
                isInventoryOpen = true;
                movement.Disabled = isInventoryOpen || isMenuOpen;
                mouseLook.Disabled = isInventoryOpen || isMenuOpen;
                reticle.GetComponent<UnityEngine.UI.Image>().enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isInventoryOpen && !isMenuOpen)
            {
                if (isFlashlightOn)
                {
                    flashLight.GetComponent<Light>().enabled = false;
                    isFlashlightOn = false;
                }
                else
                {
                    flashLight.GetComponent<Light>().enabled = true;
                    isFlashlightOn = true;
                }
            }
        }


        if (!isInventoryOpen && !isMenuOpen)
        {
            //mouse wheel
            var wheel = Input.GetAxisRaw("Mouse ScrollWheel");
            if (wheel != 0)
            {
                if (wheel < 0)
                {
                    if ((CurrentInventoryIndex + 1) > 9)
                        CurrentInventoryIndex = 1;
                    else
                        CurrentInventoryIndex++;
                }
                else
                {
                    if ((CurrentInventoryIndex - 1) < 1)
                        CurrentInventoryIndex = 9;
                    else
                        CurrentInventoryIndex--;
                }
            }

            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
                CurrentInventoryIndex = 1;
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
                CurrentInventoryIndex = 2;
            if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
                CurrentInventoryIndex = 3;
            if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
                CurrentInventoryIndex = 4;
            if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
                CurrentInventoryIndex = 5;
            if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
                CurrentInventoryIndex = 6;
            if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
                CurrentInventoryIndex = 7;
            if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8))
                CurrentInventoryIndex = 8;
            if (Input.GetKeyDown(KeyCode.Keypad9) || Input.GetKeyDown(KeyCode.Alpha9))
                CurrentInventoryIndex = 9;
        }

        //if mouse button clicked
        if ((!isInventoryOpen && !isMenuOpen) && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            bool leftClick = Input.GetMouseButtonDown(0);
            if (leftClick)
            {
                //ArmReachTest t = player.GetComponent<ArmReachTest>();
                //t.Reach();
                //Vector3.Lerp(arm.position, (arm.forward += new Vector3(0f,0f,0.3f)), 4 * Time.deltaTime);
            }

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            RaycastHit hit;

            //check if we are holding a usable item
            //Inventory inv = activeInventory.GetComponent<Inventory>();
            InventoryItem item = activeInventory.GetItemFromSlot(CurrentInventoryIndex - 1);

            if (Physics.Raycast(ray, out hit, MaxRange))
            {
                if (!leftClick)
                {
                    //if we right-click a usable world item (chest, furnace, etc), use it
                    /*WorldItem wItem = hit.collider.gameObject.GetComponent<WorldItem>();
                    if (wItem != null)
                    {
                        if (wItem.canStack)
                        {
                            //Inventory inv = activeInventory.GetComponent<Inventory>();
                            //InventoryItem item = inv.GetItemFromSlot(CurrentInventoryIndex - 1);
                            if (item != null)
                            {
                                ItemDefinition def = ItemDictionary.currentDictionary.GetItemDefinition(item.ItemType);
                                if (def.worldPrefab != null)
                                {
                                    Vector3 p = wItem.gameObject.transform.position;
                                    p += hit.normal;
                                    GameObject.Instantiate(def.worldPrefab, p, Quaternion.identity);
                                }
                                inv.RemoveItem(item);
                            }
                        }
                        wItem.Use();
                        return;
                    }
                    else //we didn't hit a usable world item, use item in hand
                    {
                        if (item != null)
                        {
                            item.Use();
                        }
                    }*/
                }
                else //left click
                {
                    //we hit a world item
                    /*WorldItem wItem = hit.collider.gameObject.GetComponent<WorldItem>();
                    if (wItem != null)
                    {
                        ItemDefinition def = ItemDictionary.currentDictionary.GetItemDefinition(wItem.type);
                        GameObject.Instantiate(def.droppedPrefab, wItem.transform.position, Quaternion.identity);
                        Destroy(wItem.gameObject);
                    }
                    else if (item != null)
                        SwingItem(item.Range, item.HitDamage);
                    else //swing empty hand
                        SwingItem(handHitRange, emptyHandHitDamage);*/
                }

                //did we hit a chunk
                Chunk c = hit.transform.GetComponent<Chunk>();
                if (c != null)
                {
                    Vector3 p = hit.point;

                    if (leftClick) //use or destory block
                    {
                        //if (selectedInventory == 0) //nothing in hand, so delete
                        //{
                        p -= hit.normal / 4;
                        //BlockType brick = c.GetBlock(p);
                        //if (brick != BlockType.None)
                        //{
                            //CmdHitBlock(p);
                            //c.DestroyBrick(brick, p);
                            //c.CreateVisualMesh();
                        //}
                    }
                    else //right click, place brick
                    {
                        p += hit.normal / 4;
                        //c = Chunk.FindChunk(p);
                        //c.SetBrick(BlockType.Dirt, p);
                        //CmdSetBlock(p, BlockType.Dirt);

                            //Inventory inv = inventory.GetComponent<Inventory>();
                            //Inventory inv = activeInventory.GetComponent<Inventory>();
                            //InventoryItem item = inv.GetItemFromSlot(CurrentInventoryIndex - 1);
                        /*if (item != null)
                        {
                            ItemDefinition def = ItemDictionary.currentDictionary.GetItemDefinition(item.ItemType);
                            p += hit.normal / 4;
                            //change in position could put it into next chunk
                            c = AsteroidChunk.FindChunk(p);
                            if (def.isBrick)
                            {
                                c.SetBrick(item.ItemType, p);
                                inv.RemoveItem(item);
                            }
                            else if (def.worldPrefab != null)//can be placed in world (chest, furnace, etc)
                            {

                                Vector3 newP = new Vector3(Mathf.FloorToInt(p.x), Mathf.FloorToInt(p.y), Mathf.FloorToInt(p.z));
                                newP += new Vector3(0.5f, 0.5f, 0.5f);

                                //see if we have room
                                bool occupied = IsOccupied(newP, def);

                                //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                //go.transform.position = newP;
                                if (!occupied)
                                {
                                    GameObject.Instantiate(def.worldPrefab, newP, Quaternion.identity);
                                    inv.RemoveItem(item);
                                }
                            }
                        }*/
                    }
                    return;
                }
            }
            else
            {
                //Debug.Log ("clicked, but nothign hit");
                //if holding a usable item and right clicked, then use item
                /*if (!leftClick && item != null)
                {
                    item.Use();
                }*/
            }
        }
    }


    /*[Command]//called by client, run on server
    void CmdHitBlock(Vector3 position)
    {
        Chunk c = Chunk.FindChunk(position);
        c.DestroyBlock(position);
        //c.CreateVisualMesh();
    }

    [Command]
    void CmdSetBlock(Vector3 position, BlockType block)
    {
        Chunk c = Chunk.FindChunk(position);
        c.SetBlock(block, position);
    }*/
}
