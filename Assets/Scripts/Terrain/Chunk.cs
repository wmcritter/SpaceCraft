using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{

    public static List<Chunk> chunksWaiting = new List<Chunk>();
    public static List<Chunk> Chunks = new List<Chunk>();

    public static int _TextureMapRows = 1;
    public static int _TextureMapColumns = 5;

    public GameObject spawnerPrefab;

    public int TextureMapRows;
    public int TextureMapColumns;

    public BlockType[,,] map;
    public Mesh visualMesh;
    protected MeshRenderer meshRenderer;
    protected MeshCollider meshCollider;
    protected MeshFilter meshFilter;

    private GameObject player;
    private ItemDictionary itemDictionary;
    protected bool initialized = false;
    //private bool hasSpawner;
    //private static GameObject sun;
    private static Vector3 sunPosition = new Vector3(0, 0, 0);
    private SavedChunk _SavedChunk;

    //private static float maxNoise;
    //private static int lastBiomeIndex;
    public SavedChunk SavedChunk
    {
        get { return _SavedChunk; }
        set { _SavedChunk = value; }
    }

    static int _ChunkSize = 20;
    public static int ChunkSize
    {
        //get { return MyWorld.currentWorld.chunkSize; }
        get { return _ChunkSize; }
        set { _ChunkSize = value; }
    }

    void Awake()
    {
        _TextureMapRows = TextureMapRows;
        _TextureMapColumns = TextureMapColumns;

    }

    // Use this for initialization
    void Start()
    {
        Chunks.Add(this);

        player = GameObject.FindGameObjectWithTag("Player");// Tags.player);
        itemDictionary = GameObject.FindGameObjectWithTag("ItemDictionary").GetComponent<ItemDictionary>();

        //if (sun == null)
        //			sun = GameObject.FindGameObjectWithTag(Tags.sun);
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();

        chunksWaiting.Add(this);
        if (chunksWaiting[0] == this)
        {
            StartCoroutine(CalculateMapFromScratch());
        }
    }

    void OnDestroy()
    {
        Chunks.Remove(this);
    }


    public virtual IEnumerator CalculateMapFromScratch()
    {
        if (_SavedChunk != null && _SavedChunk.BrickMap.Count > 0)
        {
            map = _SavedChunk.GetBrickMap();
        }
        else
        {
            map = new BlockType[_ChunkSize, _ChunkSize, _ChunkSize];

            UnityEngine.Random.InitState(123);// GameController.currentController.seed;
            Vector3 grain0Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);
            //Vector3 grain1Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);
            //Vector3 grain2Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);

            for (int x = 0; x < _ChunkSize; x++)
            {
                for (int y = 0; y < _ChunkSize; y++)
                {
                    for (int z = 0; z < _ChunkSize; z++)
                    {
                        map[x, y, z] = GetTheoreticalBrick(new Vector3(x, y, z) + transform.position, grain0Offset/*, grain1Offset, grain2Offset*/);
                        /*if (map[x, y, z] == BlockType.MobSpawner)
                        {
                            if (!hasSpawner)
                            {
                                Instantiate(spawnerPrefab, transform.position, Quaternion.identity);
                                hasSpawner = true;
                            }
                        }*/
                    }
                }
            }
        }
        StartCoroutine(CreateVisualMesh());

        yield return 0;

        initialized = true;

        chunksWaiting.Remove(this);
        if (chunksWaiting.Count > 0)
            StartCoroutine(chunksWaiting[0].CalculateMapFromScratch());

        //Debug.Log("Max noise " + maxNoise);
        //float distanceFromSun = Vector3.Distance(transform.position, MyWorld.currentWorld.SunLocation);
        //Debug.Log("distance from sun " + distanceFromSun);
        //float clusterValue = CalculateNoiseValue(transform.position, grain2Offset, 0.04f);//0.02f);
        //Debug.Log("Last Biome " + lastBiomeIndex);
        //clusterValue *= distanceFromSun;
        //Debug.Log("biome cluster value " + Mathf.Abs(clusterValue));

        yield return 0;
    }

    public static BlockType GetTheoreticalBrick(Vector3 pos)
    {
        UnityEngine.Random.InitState(123);// GameController.currentController.seed;

        Vector3 grain0Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);
        //Vector3 grain1Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);
        //Vector3 grain2Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);

        return GetTheoreticalBrick(pos, grain0Offset/*, grain1Offset, grain2Offset*/);
    }

    public static BlockType GetTheoreticalBrick(Vector3 pos, Vector3 offset0)
    {
        BlockType result = BlockType.None;

        //float clusterValue = CalculateNoiseValue(pos, offset2, 0.02f);

        float noiseValue = CalculateNoiseValue(pos, offset0, 0.04f);

        if (noiseValue > 0.2f)
        {
            //float distanceFromSun = Vector3.Distance(pos, sunPosition);// sun.transform.position);
            //float clusterValue = CalculateNoiseValue(pos, offset2,  (distanceFromSun/50000));//0.02f);
            //int biomeIndex = Mathf.FloorToInt(clusterValue * MyWorld.currentWorld.Biomes.Length);
            //clusterValue
            float distanceFromSun = (pos - sunPosition).magnitude;

            int biomeIndex = 0; //not allowed
                                //if (distanceFromSun > MyWorld.currentWorld.hotBiomeDistance)
                                //biomeIndex = 0; //hot
            if (distanceFromSun > TerrainManager.NormalBiomeMin)//MyWorld.currentWorld.normalBiomeDistance)
                biomeIndex = 1; //medium
            if (distanceFromSun > TerrainManager.ColdBiomeMin)//MyWorld.currentWorld.coldBiomeDistance)
                biomeIndex = 2; //cold


            //if (lastBiomeIndex != biomeIndex)
            //lastBiomeIndex = biomeIndex;

            //if (clusterValue  > maxNoise)
            //maxNoise = clusterValue;

            //Biome b = MyWorld.currentWorld.Biomes[biomeIndex];

            BrickLayerCondition layer = BrickLayerCondition.GroundLevel;

            if (biomeIndex == 1)//warm
            {
                result = BlockType.Grass;
                //result = (ItemType)Mathf.FloorToInt(UnityEngine.Random.Range(1, (_TextureMapColumns * /*_TextureMapRows*/1) + 1));
                if (noiseValue > 0.21)
                {
                    layer = BrickLayerCondition.BelowGroundLevel;
                    result = BlockType.Dirt;
                }
                if (noiseValue > 0.23)
                {
                    layer = BrickLayerCondition.Middle;
                    int i = Mathf.FloorToInt(Random.Range(0, 3));
                    switch (i)
                    {
                        case 0:
                            result = BlockType.Stone;
                            break;
                        case 1:
                            result = BlockType.Copper;
                            break;
                        case 2:
                            result = BlockType.Bauxite;
                            break;
                    }
                }
                if (noiseValue > 0.28)
                {
                    layer = BrickLayerCondition.Core;
                    int i = Mathf.FloorToInt(Random.Range(0, 7));
                    switch (i)
                    {
                        case 0:
                            result = BlockType.Stone;
                            break;
                        case 1:
                            result = BlockType.Iron;
                            break;
                        case 2:
                            result = BlockType.Coal;
                            break;
                        case 3:
                            result = BlockType.Diamond;
                            break;
                        case 4:
                            result = BlockType.Gold;
                            break;
                        case 5:
                            result = BlockType.Iron;
                            break;
                        case 6:
                            result = BlockType.Iron;
                            break;
                    }
                }
            }
            else if (biomeIndex == 0)//hot
            {
                result = BlockType.Sand;
                //result = (ItemType)Mathf.FloorToInt(UnityEngine.Random.Range(1, (_TextureMapColumns * /*_TextureMapRows*/1) + 1));
                if (noiseValue > 0.21)
                {
                    layer = BrickLayerCondition.BelowGroundLevel;
                    result = BlockType.Sandstone;
                }
                if (noiseValue > 0.23)
                {
                    layer = BrickLayerCondition.Middle;
                    result = BlockType.Stone;
                }
                if (noiseValue > 0.28)
                {
                    layer = BrickLayerCondition.Core;
                    int i = Mathf.FloorToInt(Random.Range(0, 3));
                    switch (i)
                    {
                        case 0:
                            result = BlockType.Stone;
                            break;
                        case 1:
                            result = BlockType.Iron;
                            break;
                        case 2:
                            result = BlockType.Coal;
                            break;
                    }
                }
            }
            else if (biomeIndex == 2) //cold
            {
                result = BlockType.Ice;
                //result = (ItemType)Mathf.FloorToInt(UnityEngine.Random.Range(1, (_TextureMapColumns * /*_TextureMapRows*/1) + 1));
                if (noiseValue > 0.21)
                {
                    layer = BrickLayerCondition.BelowGroundLevel;
                    result = BlockType.Dirt;
                }
                if (noiseValue > 0.23)
                {
                    layer = BrickLayerCondition.Middle;
                    result = BlockType.Stone;
                }
                if (noiseValue > 0.28)
                {
                    layer = BrickLayerCondition.Core;
                    int i = Mathf.FloorToInt(Random.Range(0, 3));
                    switch (i)
                    {
                        case 0:
                            result = BlockType.Stone;
                            break;
                        case 1:
                            result = BlockType.Diamond;
                            break;
                        case 2:
                            result = BlockType.Coal;
                            break;
                    }
                }
            }
            //result = b.GetBrick(pos, noiseValue, layer);
            /*Biome b = MyWorld.currentWorld.Biomes[biomeIndex];
			result = b.GetBrick(pos, noiseValue, layer);
			{
				if (result == ItemType.None)
					Debug.Log(string.Format("No Brick found for Biome {0}, Layer {1}, Noise valuse {2}", b.name, layer, noiseValue));
			}*/
        }
        else
        {
            if (noiseValue < -0.29f)
            {
                //Debug.Log("noise value" + noiseValue);
                //instantiate a mob spawner
                //result = BlockType.MobSpawner;
            }
        }
        return result;
    }

    public static float CalculateNoiseValue(Vector3 pos, Vector3 offset, float scale)
    {
        float noiseX = Mathf.Abs((float)(pos.x + offset.x) * scale);
        float noiseY = Mathf.Abs((float)(pos.y + offset.y) * scale);
        float noiseZ = Mathf.Abs((float)(pos.z + offset.z) * scale);

        return SimplexNoise.noise(noiseX, noiseY, noiseZ);
    }

    public virtual IEnumerator CreateVisualMesh()
    {
        visualMesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        for (int x = 0; x < _ChunkSize; x++)
        {
            for (int y = 0; y < _ChunkSize; y++)
            {
                for (int z = 0; z < _ChunkSize; z++)
                {
                    //if (map[x,y,z] == 0) continue;

                    BlockType brick = (BlockType)map[x, y, z];
                    //if (brick == BlockType.None || brick == BlockType.MobSpawner) continue;

                    int maxtextures = TextureMapColumns * TextureMapRows;
                    if ((int)brick > maxtextures)
                    {
                        Debug.Log("brick out of range: " + ((int)brick).ToString());
                        continue;
                    }

                    //left
                    if (IsTransparent(x - 1, y, z))
                        BuildFace(brick, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
                    //right
                    if (IsTransparent(x + 1, y, z))
                        BuildFace(brick, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);

                    //bottom
                    if (IsTransparent(x, y - 1, z))
                        BuildFace(brick, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
                    //top
                    if (IsTransparent(x, y + 1, z))
                        BuildFace(brick, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);

                    //back
                    if (IsTransparent(x, y, z - 1))
                        BuildFace(brick, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
                    //front
                    if (IsTransparent(x, y, z + 1))
                        BuildFace(brick, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
                }
            }
        }

        visualMesh.vertices = verts.ToArray();
        visualMesh.uv = uvs.ToArray();
        visualMesh.triangles = tris.ToArray();
        visualMesh.RecalculateBounds();
        visualMesh.RecalculateNormals();

        meshFilter.mesh = visualMesh;
        meshCollider.sharedMesh = visualMesh;

        yield return 0;
    }

    public virtual void BuildFace(BlockType brick, Vector3 corner, Vector3 up, Vector3 right, bool reversed,
        List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {

        int index = verts.Count;
        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        float brickRow = 1;
        float brickColumn = 1;
        float rowHeight = (float)(1.0f / TextureMapRows);
        float columnWidth = (float)(1.0f / TextureMapColumns);

        GetBrickRowAndColumn(brick, out brickRow, out brickColumn);

        if (brickRow > TextureMapRows)
        {
            Debug.Log("Invalid brickRow: " + brickRow + ", brick=" + brick);
            brickRow = TextureMapRows;
        }
        if (brickColumn > TextureMapColumns)
        {
            Debug.Log("Invalid brickColumn: " + brickColumn + ", brick=" + brick);
            brickColumn = TextureMapColumns;
        }

        //Vector2 uvWidth = new Vector2(0.125f, 0.125f);
        //Vector2 uvCorner = new Vector2( (float)((brick - 1) * 0.125f), 0.875f);
        Vector2 uvCorner = new Vector2(
            ((brickColumn - 1.0f) * columnWidth),
            1.0f - (brickRow * rowHeight));

        /*uvs.Add (uvCorner); //0,0.875
		uvs.Add (new Vector2(uvCorner.x, uvCorner.y + uvWidth.y)); //0,1
		uvs.Add (new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));//0.125, 1
		uvs.Add (new Vector2( uvCorner.x + uvWidth.x, uvCorner.y)); //0.125, 0.875
		*/

        uvs.Add(uvCorner); //0,0.875
        uvs.Add(new Vector2(uvCorner.x, uvCorner.y + rowHeight)); //0,1
        uvs.Add(new Vector2(uvCorner.x + columnWidth, uvCorner.y + rowHeight));//0.125, 1
        uvs.Add(new Vector2(uvCorner.x + columnWidth, uvCorner.y)); //0.125, 0.875

        /*uvs.Add(new Vector2(0,0));
		uvs.Add(new Vector2(0,1));
		uvs.Add(new Vector2(1,1));
		uvs.Add(new Vector2(1,0));*/

        if (reversed)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 0);
        }
        else
        {
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 2);
            tris.Add(index + 0);
        }
    }

    private void GetBrickRowAndColumn(BlockType brick, out float brickRow, out float brickColumn)
    {
        brickRow = 1.0f;
        brickColumn = (int)brick;

        if (brickColumn <= TextureMapColumns)
        {
            return;
        }

        //brickRow = 0;
        float temp = brickColumn;
        while (true)
        {
            temp = temp - TextureMapColumns;
            if (temp > 0.0f)
            {
                brickRow++;
                brickColumn = temp;
            }
            else
                break;
        }

    }

    public virtual bool IsTransparent(int x, int y, int z)
    {
        BlockType brick = GetBrick(x, y, z);
        return brick == BlockType.None;// || brick == BlockType.MobSpawner;
        /*switch(brick)
		{
			case 0: return true;
			//case 1: return true;
			case 1: return false;
			default: return true;
		}*/
    }

    public virtual BlockType GetBrick(int x, int y, int z)
    {
        if ((x < 0) || (y < 0) || (z < 0) || (x >= _ChunkSize) || (y >= _ChunkSize) || (z >= _ChunkSize))
        {
            Vector3 worldPos = new Vector3(x, y, z) + transform.position;

            if (!initialized)
            {
                SavedChunk sc = null;// GameController.currentController.FindSavedChunkByWorldPos(worldPos);
                if (sc != null)
                {
                    return sc.GetBrick(worldPos);
                }
                else
                    return GetTheoreticalBrick(worldPos);
            }

            Chunk chunk = Chunk.FindChunk(worldPos);
            if (chunk == this) return BlockType.None;
            if (chunk == null)
            {
                //see if this is a saved chunk, and if so, get the brick from the saved map
                SavedChunk sc = null;// GameController.currentController.FindSavedChunkByWorldPos(worldPos);
                if (sc != null)
                {
                    return sc.GetBrick(worldPos);
                }
                else
                    return GetTheoreticalBrick(worldPos);
            }
            return chunk.GetBlock(worldPos);
        }
        return map[x, y, z];
    }

    public virtual BlockType GetBlock(Vector3 worldPos)
    {
        worldPos -= transform.position;
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.y);
        int z = Mathf.FloorToInt(worldPos.z);
        return GetBrick(x, y, z);
    }

    public static Chunk FindChunk(Vector3 pos)
    {
        for (int a = 0; a < Chunks.Count; a++)
        {
            Vector3 cpos = Chunks[a].transform.position;

            if ((pos.x < cpos.x) || (pos.y < cpos.y) || (pos.z < cpos.z) ||
               (pos.x >= cpos.x + _ChunkSize) || (pos.y >= cpos.y + _ChunkSize) || (pos.z >= cpos.z + _ChunkSize))
                continue;
            return Chunks[a];
        }

        return null;
    }

    /// <summary>
    /// Finds the chunk by it's transform position.
    /// </summary>
    /// <returns>The chunk by position.</returns>
    /// <param name="pos">Position.</param>
    public static Chunk FindChunkByPosition(Vector3 pos)
    {
        foreach (Chunk c in Chunks)
        {
            if (c.transform.position == pos)
                return c;
        }

        return null;
    }

    private void WorldPosToMapPos(Vector3 worldPos, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt(worldPos.x);
        y = Mathf.FloorToInt(worldPos.y);
        z = Mathf.FloorToInt(worldPos.z);
    }

    //will be called from command, so it should only run on server
    public void DestroyBlock(Vector3 worldPos)
    {
        Vector3 pos = (worldPos - transform.position);
        int x, y, z;
        WorldPosToMapPos(pos, out x, out y, out z);
        if (!IsValidMapLocation(x, y, z)) return;
        BlockType block = map[x, y, z];
        GameObject go = itemDictionary.GetDroppedItemPrefab(block);
        var cube = (GameObject)Instantiate(go,
                               new Vector3(Mathf.FloorToInt(worldPos.x) + 0.5f, Mathf.FloorToInt(worldPos.y) + 0.5f, Mathf.FloorToInt(worldPos.z) + 0.5f),
                     Quaternion.identity);
        
        cube.GetComponent<DroppedItem>().Item = new InventoryItem(ItemDictionary.currentDictionary.GetItemDefinition(block));

        //spawn the dropped item on clients
        NetworkServer.Spawn(go);

        SetBlock(BlockType.None, x, y, z);
        
    }

    private bool IsValidMapLocation(int x, int y, int z)
    {
        return !(x < 0) || (y < 0) || (z < 0) || (x >= _ChunkSize) || (y >= _ChunkSize) || (z >= _ChunkSize);
    }

    public bool SetBlock(BlockType brick, Vector3 worldPos)
    {
        //Debug.Log(string.Format("Set brick {0} at {1}", brick, worldPos));
        worldPos -= transform.position;
        return SetBlock(brick, Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), Mathf.FloorToInt(worldPos.z));
    }

    public bool SetBlock(BlockType brick, int x, int y, int z)
    {
        //Debug.Log(string.Format("Set brick at {0},{1},{2}", x,y,z));
        //if ( ( x < 0) || (y < 0) || (z < 0) || (x >= chunkWidth) || (y >= chunkHeight) || (z >= chunkDepth) )
        //{
        //return false;
        //}
        if (!IsValidMapLocation(x, y, z)) return false;

        if (map[x, y, z] == brick) return false;
        map[x, y, z] = brick;
        SaveBrick(brick, x, y, z);


        StartCoroutine(CreateVisualMesh());

        if (x == 0)
        {
            Chunk chunk = FindChunk(new Vector3(x - 2, y, z) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (x == _ChunkSize - 1)
        {
            Chunk chunk = FindChunk(new Vector3(x + 2, y, z) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (y == 0)
        {
            Chunk chunk = FindChunk(new Vector3(x, y - 2, z) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (y == _ChunkSize - 1)
        {
            Chunk chunk = FindChunk(new Vector3(x, y + 2, z) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (z == 0)
        {
            Chunk chunk = FindChunk(new Vector3(x, y, z - 2) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }
        if (z == _ChunkSize - 1)
        {
            Chunk chunk = FindChunk(new Vector3(x, y, z + 2) + transform.position);
            if (chunk != null)
                StartCoroutine(chunk.CreateVisualMesh());
        }

        return true;
    }

    private void SaveBrick(BlockType brick, int x, int y, int z)
    {
        if (_SavedChunk == null)
            _SavedChunk = new SavedChunk(this);
        if (_SavedChunk.BrickMap.Count == 0)
            _SavedChunk.SetBrickMap(this.map);
        _SavedChunk.SetBrick(brick, x, y, z);
    }
}

[System.Serializable]
public class SavedChunk
{
    public SavedChunk() { }//empty constructor for serialization
    public SavedChunk(Chunk chunk)
    {
        Position = chunk.transform.position;
        //BrickMap = chunk.map;
        SetBrickMap(chunk.map);
    }

    public void SetBrickMap(BlockType[,,] map)
    {
        MapSize = 20;// MyWorld.currentWorld.chunkSize;
        BrickMap = new List<int>();
        for (int z = 0; z < MapSize; z++)
        {
            for (int y = 0; y < MapSize; y++)
            {
                for (int x = 0; x < MapSize; x++)
                {
                    int i = (int)map[x, y, z];
                    BrickMap.Add(i);
                    //if (i > 0)
                    //{
                    //Debug.Log("brick " + (BrickMap.Count -1) + " = " + i + " at " + new Vector3(x, y, z));
                    //}
                }
            }
        }
    }
    public Vector3 Position;

    //[System.Xml.Serialization.XmlArray]
    //public ItemType[,,] BrickMap;

    public int MapSize;
    public List<int> BrickMap;

    public BlockType[,,] GetBrickMap()
    {
        BlockType[,,] map = new BlockType[MapSize, MapSize, MapSize];

        int i = 0;
        for (int z = 0; z < MapSize; z++)
        {
            for (int y = 0; y < MapSize; y++)
            {
                for (int x = 0; x < MapSize; x++)
                {
                    map[x, y, z] = (BlockType)BrickMap[i];
                    i++;
                }
            }
        }
        return map;
    }

    public void SetBrick(BlockType brick, int x, int y, int z)
    {
        //BrickMap[x, y, z] = brick;
        int index = x + (y * MapSize) + (z * (MapSize * MapSize));
        BrickMap[index] = (int)brick;
    }

    public BlockType GetBrick(Vector3 worldPos)
    {
        Vector3 pos = (worldPos - Position);
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        return GetBrick(x, y, z);
    }

    public BlockType GetBrick(int x, int y, int z)
    {
        int index = x + (y * MapSize) + (z * (MapSize * MapSize));
        return (BlockType)BrickMap[index];
    }
}

public enum BlockType
{
    None, Stone, Grass, Dirt, Ice, Lava, Iron, Copper, Diamond, Coal, Glass, Sand, Sandstone, Clay, Obsidian, Bauxite, Gold,
    Brick17, Brick18, Brick19, Brick20, Brick21, Brick22, Brick23, Brick24, Brick25, Brick26, Brick27, Brick28, Brick29, Brick30, Brick31, Brick32, Brick33,
    Brick34, Brick35, Brick36, Brick37, Brick38, Brick39, Brick40
};
