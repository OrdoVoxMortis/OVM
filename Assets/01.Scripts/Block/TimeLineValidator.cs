using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeLineValidator
{
    //TODO. 타임라인 배치 변경될 때마다 업데이트
    public static List<Block> placedBlocks = new();

    public static void ValidateCombinations()
    {
        for (int i = 0; i < placedBlocks.Count; i++)
        {
            Block current = placedBlocks[i];

            bool isSuccess = true;

            // 선행 규칙 검사
            if (current.PreCombineRule != null && current.PreCombineRule.RuleType != CombineType.None)
            {
                bool hasValidPrev = HasValidPreviousBlock(current, i);
                if (!hasValidPrev && BlockValidator.RequiresPrevBlock(current))
                {
                    isSuccess = false;
                }
            }

            // 후속 규칙 검사
            if (current.NextCombineRule != null && current.NextCombineRule.RuleType != CombineType.None)
            {
                bool hasValidNext = HasValidNextBlock(current, i);
                if (!hasValidNext && BlockValidator.RequiresNextBlock(current))
                {
                    isSuccess = false;
                }
            }

            //검사 결과에 따라 시퀀스 출력
            BlockManager.Instance.ShowSequenceByResult(current, isSuccess);

            Debug.Log($"[{current.BlockName}] 조합 결과: {(isSuccess ? "성공" : "실패")}");
        }
    }

    private static bool HasValidPreviousBlock(Block block, int index)
    {
        for (int i = 0; i < index; i++)
        {
            if (BlockValidator.CanCombineWithPrev(block, placedBlocks[i]))
                return true;
        }
        return false;
    }
    private static bool HasValidNextBlock(Block block, int index)
    {
        for (int i = index + 1; i < placedBlocks.Count; i++)
        {
            if (BlockValidator.CanCombineWithNext(block, placedBlocks[i]))
                return true;
        }
        return false;
    }
}
