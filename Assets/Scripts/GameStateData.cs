using UnityEngine;

[CreateAssetMenu(fileName = "GameStateData", menuName = "Scriptable Objects/GameStateData")]
public class GameStateData : ScriptableObject
{
    [SerializeField] private int selectedCharacterIndex;
    [SerializeField] private float clearTime;
    [SerializeField] private bool isCleared;

    public int SelectedCharacterIndex => selectedCharacterIndex;
    public float ClearTime => clearTime;
    public bool IsCleared => isCleared;

    public void SetSelectedCharacterIndex(int selectedCharacterIndex)
    {
        this.selectedCharacterIndex = selectedCharacterIndex;
    }

    public void SetClearTime(float clearTime)
    {
        this.clearTime = clearTime;
    }

    public void SetIsCleared(bool isCleared)
    {
        this.isCleared = isCleared;
    }
}
