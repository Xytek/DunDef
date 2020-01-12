using UnityEngine;

public class StarsUI : MonoBehaviour
{
   public void ViewStars(int nr)
    {
        for(int i = 0; i<nr; i++)
            gameObject.transform.GetChild(i).gameObject.SetActive(true);
    }
}
