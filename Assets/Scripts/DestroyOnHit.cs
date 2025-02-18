using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    private bool hitByCurrent = false;
    private bool hitByGhost = false;
    private bool hasChangedColor = false;

    private SpriteRenderer spriteRenderer;

    private Color hitOnceColor = new Color(182f / 255f, 135f / 255f, 108f / 255f);

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit(string bulletType)
    {
        if(bulletType == "Current")
        {
            hitByCurrent = true;
        }
        else if(bulletType == "Ghost")
        {
            hitByGhost = true; 
        }

        if (!hasChangedColor)  // hitted by current or ghost for the first time
        {
            ChangeColor();
            hasChangedColor = true;
        }

        if(hitByCurrent && hitByGhost)
        {
            Destroy(gameObject);
        }
    }

    private void ChangeColor()
    {
        if (spriteRenderer != null)
        {
            if (!IsColorEqual(spriteRenderer.color, hitOnceColor))
            {
                spriteRenderer.color = hitOnceColor;
            }
        }
    }


    private bool IsColorEqual(Color currentColor, Color targetColor, float tolerance = 0.01f)
    {
        return Mathf.Abs(currentColor.r - targetColor.r) < tolerance &&
               Mathf.Abs(currentColor.g - targetColor.g) < tolerance &&
               Mathf.Abs(currentColor.b - targetColor.b) < tolerance &&
               Mathf.Abs(currentColor.a - targetColor.a) < tolerance;
    }
}
