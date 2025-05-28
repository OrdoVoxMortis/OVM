
using UnityEngine;

public class Trashcan : MonoBehaviour, IInteractable
{
    private UI_SaveLoad saveUI;
    float duration = 5f;
    private void Start()
    {
        saveUI = FindObjectOfType<UI_SaveLoad>();
    }
    public string GetInteractComponent()
    {
        if (GameManager.Instance.isClear)
            return "E키를 눌러 상호작용";
        else
            return " ";
    }

    public void OnInteract()
    {
        if (!UIManager.Instance.isUIActive && GameManager.Instance.isClear)
        {
            UIManager.Instance.ShowUI<UI_SaveLoad>("UI_SaveLoad");
            UIManager.Instance.UIActive();
            PlayerStateMachine sm = GameManager.Instance.Player.stateMachine;
            PlayerInteractionLockpick lockState = new PlayerInteractionLockpick(sm, this, duration);
            sm.ChangeState(lockState);
        }
    }

    public void Deactive()
    {
        throw new System.NotImplementedException();
    }

    public void SetInteractComponenet(string newText)
    {
        throw new System.NotImplementedException();
    }
}