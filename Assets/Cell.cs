using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool alive;

    public Color targetColor;
    public float colorChangeSpeed = 3.0f;
    public float colorChangeTimer = 0.0f;

    SpriteRenderer spriteRenderer;

    public void UpdateStatus()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = alive;

        // Set the target color based on the cell's state
        targetColor = alive ? new Color32(70, 40, 40, 100) : Color.red;

        // Reset the color change timer
        colorChangeTimer = 0.0f;
    }
    public void Update()
    {
        // Gradually change the color over time
        if (spriteRenderer.color != targetColor)
        {
            colorChangeTimer += Time.deltaTime;
            float t = Mathf.Clamp01(colorChangeTimer / colorChangeSpeed);
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, t);
        }
    }
}