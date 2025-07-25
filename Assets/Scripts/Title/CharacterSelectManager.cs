using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("ゲームデータの参照")]
    [SerializeField] private GameStateData gameStateData;

    [Header("UIの参照")]
    [SerializeField] private GameObject characterSelectPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RectTransform readyBannerTransform;
    [SerializeField] private CharacterCursor characterCursor;

    [Header("キャラ選択UI")]
    [SerializeField] private Outline[] characterIconOutlines;
    [SerializeField] private string[] characterDescriptions;

    [Header("確認バナーの設定")]
    [SerializeField] private Vector2 readyHiddenPosition = new Vector2(-1920f, 0f);
    [SerializeField] private Vector2 readyVisiblePosition = new Vector2(0f, 0f);
    [SerializeField] private float readySlideDuration = 0.5f;

    private int selectIndex = 0;
    private bool isConfirming = false;
    private bool isSliding = false;
    
    private void Start()
    {
        characterCursor.OnHoverChanged += UpdateSelectionIndex;
        readyBannerTransform.anchoredPosition = readyHiddenPosition;
        readyBannerTransform.gameObject.SetActive(false);
        Close();
    }

    private void UpdateSelectionIndex(int newIndex)
    {
        selectIndex = newIndex;
        UpdateSelectionUI();
    }

    private void UpdateSelectionUI()
    {
        for(int i = 0; i < characterIconOutlines.Length; i++)
        {
            if(characterIconOutlines[i] != null)
            {
                characterIconOutlines[i].enabled = (i == selectIndex);
                characterIconOutlines[i].effectColor = (i == selectIndex) ? Color.red : new Color(0, 0, 0, 0);
            }
        }

        descriptionText.text = (selectIndex >= 0 && selectIndex < characterDescriptions.Length)
            ? characterDescriptions[selectIndex]
            : "";
    }

    public async UniTask<int> WaitForSelectionAsync()
    {
        isConfirming = false;

        while (true)
        {
            if (!isConfirming && Input.GetKeyDown(KeyCode.Space))
            {
                isConfirming = true;
                isSliding = true;
                await SlideBannerAsync(true);
                isSliding = false;
            }
            else if (isConfirming && Input.GetKeyDown(KeyCode.Space) && !isSliding)
            {
                // 確定 → 選択完了
                gameStateData.selectedCharacterIndex = selectIndex;
                return selectIndex;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isConfirming && !isSliding)
            {
                // キャンセル → バナーを引っ込める
                isSliding = true;
                await SlideBannerAsync(false);
                isSliding = false;
                isConfirming = false;
            }

            await UniTask.Yield();
        }
    }

    private async UniTask SlideBannerAsync(bool slideIn)
    {
        float t = 0f;
        Vector2 startPosition = slideIn ? readyHiddenPosition : readyVisiblePosition;
        Vector2 endPosition = slideIn ? readyVisiblePosition : readyHiddenPosition;

        readyBannerTransform.anchoredPosition = startPosition;
        readyBannerTransform.gameObject.SetActive(true);

        while (t < readySlideDuration)
        {
            t += Time.deltaTime;
            readyBannerTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t / readySlideDuration);
            await UniTask.Yield();
        }

        readyBannerTransform.anchoredPosition = endPosition;

        if (!slideIn) readyBannerTransform.gameObject.SetActive(false);
    }


    public void Open()
    {
        characterSelectPanel.SetActive(true);
        descriptionText.gameObject.SetActive(true);
        characterCursor.gameObject.SetActive(true);
        UpdateSelectionUI();
    }

    public void Close()
    {
        characterSelectPanel.SetActive(false);
        descriptionText.gameObject.SetActive(false);
        characterCursor.gameObject.SetActive(false);
    }
}
