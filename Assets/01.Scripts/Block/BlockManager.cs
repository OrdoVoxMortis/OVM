using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : SingleTon<BlockManager>
{
    private List<Block> blocks = new();

    public void RegisterBlock(Block block)
    {
        if (!blocks.Contains(block))
        {
            blocks.Add(block);
        }
    }

    public void UnregisterBlock(Block block)
    {
        if (blocks.Contains(block))
        {
            blocks.Remove(block);
        }
    }

    public List<Block> GetAllBlocks() => blocks;

    public Block FindBlockById(int id)
    {
        return blocks.Find(x => x.id == id);
    }
}
