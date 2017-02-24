using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour {

    public Chunk ChunkPrefab;
    public float ViewRange = 120;
    public static float MinDistFromSun = 200;
    public static float MaxDistFromSun = 600;
    public static float NormalBiomeMin = 334;
    public static float ColdBiomeMin = 466;

    GameObject player;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;
        }

        Vector3 pPos = player.transform.position;

        for (int a = 0; a < Chunk.Chunks.Count; a++)
        {
            Vector3 pos = Chunk.Chunks[a].transform.position;
            Vector3 delta = pos - player.transform.position;
            delta.y = 0;
            if (delta.magnitude < ViewRange + Chunk.ChunkSize * 2) continue;

            //GameController.currentController.SaveChunk(AsteroidChunk.Asteroids[a].SavedChunk);
            Destroy(Chunk.Chunks[a].gameObject);
        }

        for (float x = pPos.x - ViewRange; x < pPos.x + ViewRange; x += Chunk.ChunkSize)
        {
            for (float y = pPos.y - ViewRange; y < pPos.y + ViewRange; y += Chunk.ChunkSize)
            {
                for (float z = pPos.z - ViewRange; z < pPos.z + ViewRange; z += Chunk.ChunkSize)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    pos.x = Mathf.Floor(pos.x / (float)Chunk.ChunkSize) * Chunk.ChunkSize;
                    pos.y = Mathf.Floor(pos.y / (float)Chunk.ChunkSize) * Chunk.ChunkSize;
                    pos.z = Mathf.Floor(pos.z / (float)Chunk.ChunkSize) * Chunk.ChunkSize;

                    //use circle instad of square
                    Vector3 delta = pos - player.transform.position;
                    if (delta.magnitude > ViewRange)
                    {
                        continue;
                    }

                    //check distance to sun, if too close or too far, don't draw
                    delta = pos - Vector3.zero;
                    if (delta.magnitude < MinDistFromSun ||
                        delta.magnitude > MaxDistFromSun)
                        continue;

                    //Debug.Log(string.Format("Find Chunk at {0},{1},{2}", pos.x, pos.y, pos.z));
                    Chunk c = Chunk.FindChunk(pos);
                    if (c != null) continue; //found a chunk, so don't make a new one

                    //Debug.Log("Create new Asteroid");
                    c = (Chunk)Instantiate(ChunkPrefab, pos, Quaternion.identity);
                    //c.SavedChunk = GameController.currentController.FindSavedChunkByTransformPos(pos);
                }
            }
        }
    }
}
