using UnityEngine;
using TMPro;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private PauseController pauseController;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int maxLevelSecondsTimer;
    [SerializeField] private int timeScore;

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
            StopTimer();
            victoryUI.SetActive(true);
            pauseController.Pause(true,true);
            scoreManager.UpdateScore(timeScore * (Mathf.Max(0, maxLevelSecondsTimer - timeSecondsCounter)));
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeSecondsCounter / 60f);
        int seconds = Mathf.FloorToInt(timeSecondsCounter % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private System.Collections.IEnumerator UpdateTimer()
    {
        while (isRunning)
        {
            UpdateTimerText();

            yield return new WaitForSeconds(1f);

            timeSecondsCounter += 1;
        }
    }
}
