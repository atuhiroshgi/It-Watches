using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("ゲームデータの参照")]
    [SerializeField] private GameStateData gameStateData;

    [Header("キャラクターの設定")]
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private Material[] characterMaterials;

    [Header("デフォルトスキル")]
    [SerializeField] private Light directionalLight;

    [Header("隠れるスキル")]
    [SerializeField] private SkinnedMeshRenderer playerSkin;
    [SerializeField] private Material hiddenMaterial;

    private SkillBase currentSkill;
    private PlayerManager playerManager;
    private PlayerInputManager playerInputManager;
    private PlayerLocomotionManager playerLocomotionManager;
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
        if (playerInputManager.SkillInput && skillGauge.DecreaseSkillCount(currentSkill.GetSkillCost()))
        {
            currentSkill.Activate();
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
                currentSkill = new DefaultSkill(1, directionalLight);
                break;

            case 1:
                currentSkill = new HiddenSkill(2, playerManager, skinnedMeshRenderer, hiddenMaterial, playerLocomotionManager, 5f);
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

    public void SetPlayerLocomotionManager(PlayerLocomotionManager playerLocomotionManager)
    {
        this.playerLocomotionManager = playerLocomotionManager;
    }

    public void SetSkillGauge(SkillGauge skillGauge)
    {
        this.skillGauge = skillGauge;
    }

    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }
}