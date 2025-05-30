using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockValidator
{
    public static bool RequiresNextBlock(Block block) // 후속 블럭 필요 여부
    {
        return block.Action == BlockAction.Collect ||
               (block.NextCombineRule != null && block.NextCombineRule.RuleType != CombineType.None);
    }

    public static bool RequiresPrevBlock(Block block) // 선행 블럭 필요 여부
    {
        return block.PreCombineRule != null && block.PreCombineRule.RuleType != CombineType.None;
    }

    public static bool CanCombineWithNext(Block block, Block next) // 후속 조합 검사
    {
        if (block.NextCombineRule == null || next == null) return false;
        switch (block.NextCombineRule.RuleType)
        {
            case CombineType.AllowByType:
                return block.NextCombineRule.AllowedType == next.Type;
            case CombineType.AllowSpecific:
                return block.NextCombineRule.AllowedBlocksIds.Contains(next.id);
            default:
                return false;
        }
    }

    public static bool CanCombineWithPrev(Block block, Block prev) // 선행 조합 검사
    {
        if (block.PreCombineRule == null || prev == null) return false;
        switch (block.PreCombineRule.RuleType)
        {
            case CombineType.AllowByType:
                return block.PreCombineRule.AllowedType == prev.Type;
            case CombineType.AllowSpecific:
                Debug.Log($"AllowSpecific 검사: {prev.id} ∈ [{string.Join(",", block.PreCombineRule.AllowedBlocksIds)}]");
                return block.PreCombineRule.AllowedBlocksIds.Contains(prev.id);
            default:
                return false;
        }
    }
}
