using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private PauseController pauseController;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreNumber;
    [SerializeField] private TextMeshProUGUI victoryTimerText;
    [SerializeField] private TextMeshProUGUI victoryScoreNumber;
    [SerializeField] private int maxLevelSecondsTimer;
    [SerializeField] private int timeScore;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private List<int> scoreRanks;
    [SerializeField] private List<GameObject> ranks; 

    private int timeSecondsCounter;
    private bool isRunning;

    private void Start()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        timeSecondsCounter = 0;
        isRunning = true;

        StartCoroutine(UpdateTimer());
    }

    private void OnTransformChildrenChanged()
    {
        // Check if there are no children
        if (transform.childCount == 0)
        {
            scoreManager.UpdateScore(timeScore * (Mathf.Max(0, maxLevelSecondsTimer - timeSecondsCounter)));
            StopTimer();
            CopyScoreTimer();
            GiveRank();
            victoryUI.SetActive(true);
            pauseController.Pause(true,true);
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeSecondsCounter / 60f);
        int seconds = Mathf.FloorToInt(timeSecondsCounter % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void CopyScoreTimer()
    {
        victoryTimerText.text = timerText.text;
        victoryScoreNumber.text = scoreNumber.text;

        timerText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    private void GiveRank()
    {
       
        int playerScore;
        Debug.Log(int.TryParse(scoreNumber.text, out playerScore));
        if (int.TryParse(scoreNumber.text, out playerScore))
        {
            int assignedRankIndex = 0;
            for (int i = 0; i < scoreRanks.Count; i++)
            {
                if (playerScore >= scoreRanks[i])
                {
                    assignedRankIndex = i;
                }
            }

            ranks[assignedRankIndex].SetActive(true);
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private System.Collections.IEnumerator UpdateTimer()
    {
        while (isRunning)
        {
            if (!pauseManager.Paused)
            {
                UpdateTimerText(); 
                timeSecondsCounter += 1;
            }

            yield return new WaitForSeconds(1f);

        }
    }
}
