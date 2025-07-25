using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class TitleFlowManager : MonoBehaviour
{
    [Header("クラスの参照")]
    [SerializeField] private TitleAnimationManager titleAnimationManager;
    [SerializeField] private CharacterSelectManager characterSelectManager;
    [SerializeField] private string mainSceneName = "Game";


    private enum TitleFlowState
    {
        WaitingForAnyKey,
        PlayingExitAnimation,
        CharacterSelecting,
        Finished,
    }

    private TitleFlowState currentState = TitleFlowState.WaitingForAnyKey;
    private bool anyKeyPressed = false;

    private void Start()
    {
        StartTitleFlow().Forget();
    }

    private void Update()
    {
        switch (currentState)
        {
            case TitleFlowState.WaitingForAnyKey:
                if (Input.anyKeyDown)
                {
                    anyKeyPressed = true;
                }
                break;

            case TitleFlowState.CharacterSelecting:
                break;

            default:
                break;
        }
    }

    private async UniTaskVoid StartTitleFlow()
    {
        // 待機状態
        currentState = TitleFlowState.WaitingForAnyKey;
        await WaitForAnyKeyAsync();

        // アニメーション再生
        currentState = TitleFlowState.PlayingExitAnimation;
        await titleAnimationManager.PlayExitSequenceAsync();

        // キャラ選択開始
        currentState = TitleFlowState.CharacterSelecting;
        characterSelectManager.Open();
        
        int selectedIndex = await characterSelectManager.WaitForSelectionAsync();

        currentState = TitleFlowState.Finished;
        SceneManager.LoadScene(mainSceneName);
    }

    private async UniTask WaitForAnyKeyAsync()
    {
        while (!anyKeyPressed)
        {
            await UniTask.Yield();
        }
    }
}
