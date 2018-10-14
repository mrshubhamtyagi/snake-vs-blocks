using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public Block blockPrefab;
    public Transform blocksParent;
    public float blockGapInX;
    public int blocksPerRow;

    private List<Block> blocks = new List<Block>();
    private float prevPositionInY;
    private float prevPositionInX;


    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnBlock();
    }

    public void SpawnBlock()
    {
        Block block = null;
        for (int i = 0; i < blocksPerRow; i++)
        {
            block = Instantiate(blockPrefab);
            blocks.Add(block);
            block.transform.SetParent(blocksParent);
            Vector3 _pos = GetBlockPosition();
            block.transform.position = _pos;

            block.SetNumber(UnityEngine.Random.Range(2, 10));
        }
        if (block != null)
            prevPositionInY = block.transform.position.x + 10;

    }

    private Vector3 GetBlockPosition()
    {
        Vector3 position = Vector3.zero;
        position.y = prevPositionInY;
        float[] range = new float[5] { -2.5f, -1.5f, 0.5f, 1.5f, 2.5f };
        int _x;

        do
        {
            _x = Mathf.FloorToInt(UnityEngine.Random.Range(0, range.Length));
        }
        while (range[_x] == prevPositionInX);

        prevPositionInX = range[_x];

        return position;
    }
}
