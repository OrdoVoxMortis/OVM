using UnityEngine;

public class StageWall : MonoBehaviour
{
    [SerializeField] private GameObject stageWall;
    private void OnEnable()
    {
        GameManager.OnSelectedBGMSet += WallDeactive; // 이벤트 구독
    }

    private void OnDisable()
    {
        GameManager.OnSelectedBGMSet -= WallDeactive; // 이벤트 해제
    }

    private void Start()
    {
        if(GameManager.Instance.SelectedBGM != null)
        {
            WallDeactive();
        }
        
    }
    private void WallDeactive()
    {
    
            stageWall.gameObject.SetActive(false);
            Debug.Log("투명벽 비활성화됨!");
      
    }
}
