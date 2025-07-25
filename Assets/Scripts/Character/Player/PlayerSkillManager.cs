using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("ゲームデータの参照")]
    [SerializeField] private GameStateData gameStateData;

    [Header("キャラクターの設定")]
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material[] characterMaterials;

    [Header("デフォルトスキルの設定")]
    [SerializeField] private Light directionalLight;

    private SkillBase currentSkill;
    private PlayerInputManager playerInputManager;
    private SkillGauge skillGauge;
    private int selectedCharacterIndex;

    public void Setup()
    {
        selectedCharacterIndex = gameStateData.SelectedCharacterIndex;
        CharacterSetup();
        SkillSetup();
    }

    public void GameLoopUpdate()
    {
        if (playerInputManager.SkillInput)
        {
            skillGauge.DecreaseSkillCount(currentSkill.Activate());
        }
    }

    private void CharacterSetup()
    {
        skinnedMeshRenderer.material = characterMaterials[selectedCharacterIndex];
    }

    private void SkillSetup()
    {
        switch (selectedCharacterIndex)
        {
            case 0:
                currentSkill = new DefaultSkill(directionalLight);
                break;

            default:
                Debug.LogError("サポートされていないインデックスが選択されています");
                break;
        }
    }

    public void SetPlayerInputManager(PlayerInputManager playerInputManager)
    {
        this.playerInputManager = playerInputManager;
    }

    public void SetSkillGauge(SkillGauge skillGauge)
    {
        this.skillGauge = skillGauge;
    }
}