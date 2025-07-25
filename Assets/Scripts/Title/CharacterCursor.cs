using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

public class CharacterCursor : MonoBehaviour
{
    [Header("ÉJÅ[É\ÉãÇÃê›íË")]
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private Image[] characterIcons;
    [SerializeField] private float moveSpeed = 1000f;
    [SerializeField] private float maxSelectDistance = 200f;

    public event Action<int> OnHoverChanged;

    private int currentIndex = -1;

    private void Update()
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed * Time.deltaTime;
        cursorTransform.anchoredPosition += move;

        int closest = FindClosestIcon();

        if (closest != -1 && closest != currentIndex)
        {
            currentIndex = closest;
            OnHoverChanged?.Invoke(currentIndex);
        }
    }

    private int FindClosestIcon()
    {
        float closestDistance = float.MaxValue;
        int closestIndex = -1;

        Vector2 cursorPos = RectTransformUtility.WorldToScreenPoint(Camera.main, cursorTransform.position);

        for (int i = 0; i < characterIcons.Length; i++)
        {
            Vector2 iconPos = RectTransformUtility.WorldToScreenPoint(Camera.main, characterIcons[i].rectTransform.position);
            float distance = Vector2.Distance(cursorPos, iconPos);

            if (distance < closestDistance && distance <= maxSelectDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
