using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    int currentScore = 0;

    [SerializeField]

    //TextmeshProUGUI scoreText;

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ModifyScore(int amount)
    {
        currentScore += amount;
        Debug.Log("Score updated:" + currentScore);
    }
}