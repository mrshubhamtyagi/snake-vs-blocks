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
    public Color[] colors;

    private List<Block> blocks = new List<Block>();
    private float prevPositionInY = 20;
    private int prevPositionInX = 0;
    int[] allotedPlacedX;
    int allotedPlacedIndex;

    void Start()
    {
        for (int i = 0; i < 10; i++)
            SpawnBlock();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SpawnBlock();
    }

    public void SpawnBlock()
    {
        // reset block reference , alloted places and index
        Block block = null;
        allotedPlacedX = new int[blocksPerRow];
        allotedPlacedIndex = 0;

        // spawn n numbers of block in each row
        for (int i = 0; i < blocksPerRow; i++)
        {
            block = Instantiate(blockPrefab);
            blocks.Add(block);
            block.transform.SetParent(blocksParent);
            Vector3 _pos = GetBlockPosition();
            block.transform.position = _pos;
            block.gameObject.SetActive(true);

            block.SetNumber(UnityEngine.Random.Range(2, 10));
            block.GetComponent<SpriteRenderer>().color = colors[UnityEngine.Random.Range(0, colors.Length)];
        }
        //print("================================================");
        prevPositionInY = block.transform.position.y + 10;
    }

    private Vector3 GetBlockPosition()
    {
        Vector3 position = Vector3.zero;
        position.y = prevPositionInY;
        float[] range = new float[5] { -2.5f, -1.5f, 0.5f, 1.5f, 2.5f };

        int randomNo = -1;
        bool flag = false;
        while (!flag)
        {
            randomNo = UnityEngine.Random.Range(0, range.Length);
            for (int i = 0; i < allotedPlacedX.Length; i++)
            {
                if (randomNo == allotedPlacedX[i])
                {
                    flag = false;
                    break;
                }
                else
                    flag = true;
            }
        }
        allotedPlacedX[allotedPlacedIndex++] = randomNo;
        position.x = range[randomNo];
        return position;
    }
}
