using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite threeFourthsHeart;
    public Sprite halfHeart;
    public Sprite oneFourthHeart;
    public Sprite emptyHeart;
    public int numHearts = 1;

    private int health = 4;
    private int numFullHearts = 1;
    private int numHeartFourths = 0;

    void Start()
    {
        // TODO: Make health per heart changeable in editor
        Debug.Assert(PlayerHealthController.HEALTH_PER_HEART == 4, "Health per heart must be 4!");
        Debug.Assert(PlayerHealthController.Instance.maxHearts <= 10, "Max hearts cannot exceed 10!");
    }

    void Update()
    {
        if(PlayerHealthController.Instance != null)
        {
            numHearts = PlayerHealthController.Instance.maxHearts;
            health = PlayerHealthController.Instance.currentHealth;
        }

        numFullHearts = health / 4;
        numHeartFourths = health % 4;

        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < numFullHearts)
            {
                hearts[i].sprite = fullHeart;
            }
            else if(numHeartFourths != 0 && i == numFullHearts)
            {
                switch (numHeartFourths)
                {
                    case 1:
                        hearts[i].sprite = oneFourthHeart;
                        break;
                    case 2:
                        hearts[i].sprite = halfHeart;
                        break;
                    case 3:
                        hearts[i].sprite = threeFourthsHeart;
                        break;
                }
            }
            else { hearts[i].sprite = emptyHeart; }

            if(i < numHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
