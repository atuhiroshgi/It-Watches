using UnityEngine;

public class TimerManager : EntityBase
{
    [Header("�J�E���g�_�E���J�n����")]
    [SerializeField] private int startMinutes = 3;
    [SerializeField] private int startSeconds = 0;

    [Header("�\���pTMPGUI")]
    [SerializeField] private TMPro.TextMeshProUGUI timerText;

    private float remainingTime;
    private bool isCountingDown = false;

    public bool IsCountingDown => isCountingDown;

    public override void GameStart()
    {
        remainingTime = startMinutes * 60f + startSeconds;
        isCountingDown = true;
        UpdateTimerText();
    }

    public void GameLoopUpdate()
    {
        if (!isCountingDown) return;

        remainingTime -= Time.deltaTime;
        if(remainingTime <= 0f)
        {
            remainingTime = 0f;
            isCountingDown = false;
            UpdateTimerText();
            OnCountdownFinished();
        }
        else
        {
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:0}:{seconds:00}";
    }

    private void OnCountdownFinished()
    {
        Debug.Log("�J�E���g�_�E���I��");
    }
}
