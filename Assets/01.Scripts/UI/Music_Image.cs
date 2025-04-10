using UnityEngine;
using UnityEngine.UI;

public class Music_Image : MonoBehaviour
{
    [SerializeField] private Image musicImage;

    public void SetImage(Sprite newSprite)
    {
       if(musicImage != null)
        {
            musicImage.sprite = newSprite;  
        }
    }
}
