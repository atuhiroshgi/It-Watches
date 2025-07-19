using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("�`�F�b�N�|�C���g�̐ݒ�")]
    [SerializeField] private Renderer topObjectRenderer;
    [SerializeField] private Material completedMaterial;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float gaugeFillRate = 1f;
    [SerializeField] private float gaugeThreshold = 10f;

    private ProgressPanel progressPanel;
    private float currentGauge = 0f;
    private bool isPlayerTouching = false;
    private bool isGaugeFilled = false;

    public void GameLoopUpdate()
    {
        if (isPlayerTouching && Input.GetKey(KeyCode.E))
        {
            currentGauge += gaugeFillRate * Time.deltaTime;

            // �Q�[�W���ڕW�l��B������}�e���A����ύX
            if(currentGauge >= gaugeThreshold && !isGaugeFilled)
            {
                currentGauge = gaugeThreshold;
                isGaugeFilled = true;
                ChangeMaterial();
                NotifyComplete();
            }
        }
    }

    private void ChangeMaterial()
    {
        if(topObjectRenderer != null && completedMaterial != null)
        {
            topObjectRenderer.material = completedMaterial;
        }
    }

    private void NotifyComplete()
    {
        progressPanel?.AdvanceProgressIcon();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("������");
        if (IsInLayerMask(collision.gameObject, playerLayerMask))
        {
            isPlayerTouching = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, playerLayerMask))
        {
            isPlayerTouching = false;
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }

    public void SetProgressPanel(ProgressPanel progressPanel)
    {
        this.progressPanel = progressPanel;
    }
}
