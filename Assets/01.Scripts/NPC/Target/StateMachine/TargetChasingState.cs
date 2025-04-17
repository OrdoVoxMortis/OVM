using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChasingState : TargetBaseState
{
    private const float ArrivalThreshold = 1f;

    public TargetChasingState(TargetStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if (stateMachine.Target.BlockNumber >= stateMachine.Blocks.Length)
            stateMachine.Target.BlockNumber = 0;
        var blockInfo = stateMachine.Blocks[stateMachine.Target.BlockNumber].GetComponent<TargetBlockInfo>();
        float speed = (blockInfo != null && blockInfo.moveSpeed > 0f)
            ? blockInfo.moveSpeed : groundData.BaseSpeed;

        stateMachine.Target.Agent.speed = speed;
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Target.AnimationData.GroundParameterHash);
        StartAnimation(stateMachine.Target.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Target.AnimationData.GroundParameterHash);
        StopAnimation(stateMachine.Target.AnimationData.WalkParameterHash);
    }

    public override void Update()
    {
        base.Update();
        int idx = stateMachine.Target.BlockNumber;
        if (idx < 0 || idx > stateMachine.Blocks.Length) return;

        Vector3 dest = stateMachine.Blocks[idx].transform.position;
        float distanceToBlock = Vector3.Distance(stateMachine.Target.transform.position, dest);

        if (distanceToBlock <= ArrivalThreshold)
        {
            TargetBlockInfo blockInfo = stateMachine.Blocks[idx].GetComponent<TargetBlockInfo>();
            if (blockInfo != null)
            {
                switch (blockInfo.blockStateType)
                {
                    case TargetBlockStateType.Idle:
                        var idle = stateMachine.IdleState;
                        idle.SetDuration(blockInfo.stateDuration);
                        stateMachine.ChangeState(idle);
                        return;

                    case TargetBlockStateType.Interaction:
                        var inter = stateMachine.InteractionState;
                        inter.SetDuration(blockInfo.stateDuration);
                        stateMachine.ChangeState(inter);
                        return;

                    default:
                        stateMachine.Target.BlockNumber++;
                        stateMachine.ChangeState(stateMachine.ChasingState);
                        return;
                }
            }
            else
            {
                stateMachine.Target.BlockNumber++;
                stateMachine.ChangeState(stateMachine.ChasingState);
            }
        }


    }

}
