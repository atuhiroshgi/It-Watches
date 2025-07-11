using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class StartSignalManager : MonoBehaviour
{
    [Header("�\���pTextMeshProUGUI")]
    [SerializeField] private TMPro.TextMeshProUGUI messageText;

    [Header("���o����")]
    [SerializeField] private float readyShowDuraion = 1.5f;
    [SerializeField] private float readySliderDuration = 0.5f;
    [SerializeField] private float goShowDuration = 1.0f;
    [SerializeField] private float delayAfterSlide = 0.5f;

    private Vector3 readyStartPos;
    private Vector3 readyEndPos;
    private float slideAmount = 2000f;
    private bool isFinished = false;

    public bool IsFinished => isFinished;

    public async void Setup()
    {
        if(messageText == null)
        {
            enabled = false;
            return;
        }

        readyStartPos = messageText.rectTransform.localPosition;
        readyEndPos = readyStartPos + new Vector3(slideAmount, 0f, 0f);

        await PlaySequenceAsync();
    }

    public async UniTask PlaySequenceAsync()
    {
        isFinished = false;

        // Ready�̕\��
        messageText.text = "Ready...";
        messageText.rectTransform.localPosition = readyStartPos;
        messageText.gameObject.SetActive(true);

        await UniTask.Delay((int)(readyShowDuraion * 1000));

        // Ready�̃X���C�h�A�E�g
        await SlideTextAsync(readyStartPos, readyEndPos, readySliderDuration);

        // �X���C�h��̑ҋ@����
        await UniTask.Delay((int)(delayAfterSlide * 1000));

        // Go�̕\��
        messageText.rectTransform.localPosition = readyStartPos;
        messageText.text = "Go!!";

        await UniTask.Delay((int)(goShowDuration * 1000));

        messageText.gameObject.SetActive(false);
        isFinished = true;
    }

    private async UniTask SlideTextAsync(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            messageText.rectTransform.localPosition = Vector3.Lerp(from, to, t);
            await UniTask.Yield();
        }

        messageText.rectTransform.localPosition = to;
    }
}
