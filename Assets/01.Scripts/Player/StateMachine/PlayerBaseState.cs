using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;
    private float timer;

    //공중판정을 유예할 시간
    protected float coyoteTime = 0.2f;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }


    public virtual void Enter()
    {
        AddInputActionCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionCallbacks();
    }

    public virtual void AddInputActionCallbacks()
    {
        RemoveInputActionCallbacks();

        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.started += OnMovementStarted;
        input.playerActions.Movement.canceled += OnMovementCanceled;
        input.playerActions.Run.started += OnRunStarted;
        input.playerActions.Run.canceled += OnRunCanceled;
        //input.playerActions.Squat.started += OnSquatStarted;
        //input.playerActions.Squat.canceled += OnSquatCanceled;
        //input.playerActions.Jump.started += OnJumpStarted;
    }

    public virtual void RemoveInputActionCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.started -= OnMovementStarted;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Run.started -= OnRunStarted;
        input.playerActions.Run.canceled -= OnRunCanceled;
        //input.playerActions.Squat.started -= OnSquatStarted;
        //input.playerActions.Squat.canceled -= OnSquatCanceled;
        //input.playerActions.Jump.started -= OnJumpStarted;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void Update()
    {
        if (GameManager.Instance.Player.isSit)
            return;

        if (UIManager.Instance.isUIActive)
        {
            if (!(stateMachine.CurrentState() is PlayerIdleState))
                stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }


            Move();
    }

    protected virtual void OnMovementStarted(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnSquatStarted(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnSquatCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
        stateMachine.IsRunKeyHeld = true;
    }

    protected virtual void OnRunCanceled(InputAction.CallbackContext cntext)
    {
        if (UIManager.Instance.isUIActive) return;
        stateMachine.IsRunKeyHeld = false;
        // 만약 현재 상태가 RunState라면, 움직임 값에 따라 WalkState 또는 IdleState로 전이
        if (stateMachine.CurrentState() == stateMachine.RunState)
        {

            if (stateMachine.MovementInput != Vector2.zero)
                stateMachine.ChangeState(stateMachine.WalkState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    //protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    //{

    //}

    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, true);
    }

    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Player.Animator.SetBool(animatorHash, false);
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);
        Move(movementDirection);

    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCamTransform.forward;
        Vector3 right = stateMachine.MainCamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;

    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Player.Controller.Move(((direction * movementSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime);
    }

    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);

        }
    }

    public virtual float GetReaminingActionTime()
    {
        return timer;
    }

    public virtual void ResumeState(float remainingTime)
    {
        timer = remainingTime;
    }
}