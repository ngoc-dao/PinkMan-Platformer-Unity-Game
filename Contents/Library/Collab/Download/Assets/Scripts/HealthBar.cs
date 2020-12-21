using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public const int numHearts = 5;
    public Image redHeartPrefab;
    public Sprite redHeart;
    public Sprite greyHeart;
    Image[] hearts = new Image[numHearts];
    public HitPoints hitPoints;
    string[] heartColors = new string[numHearts];

    private void Start()
    {
        CreateHealthBar();
    }

    private void CreateHealthBar()
    {
        // Create numHearts red hearts from right to left on top right corner
        if (redHeartPrefab != null)
        {
            for (int i = 0; i < numHearts; i++)
            {
                Image newHeart = Instantiate(redHeartPrefab);
                newHeart.name = "Heart_" + i;
                newHeart.transform.SetParent(gameObject.transform.GetChild(0).transform);
                hearts[i] = newHeart;
                heartColors[i] = "red";
            }
        }
    }

    public bool Decrement(int damage)
    {
        hitPoints.value -= damage;
        
        for (int i = 0; i < numHearts; i++)
        {
            if (heartColors[i] == "red")
            {
                hearts[i].sprite = greyHeart;
                heartColors[i] = "grey";
                if (hitPoints.value <= 0)
                {
                    return false;
                }
                return true;
            }
        }
        return false;
    }
}
