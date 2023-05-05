using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class pipe : MonoBehaviour
{
    SpriteRenderer myRenderer;
    BoxCollider2D[] colliders;

    [Header("Sprites")]
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    [Header("General")]
    public bool canRotate = false;
    public bool energized = true;
    public bool genese = false;
    public bool receptor = false;
    public bool painted = false;

    public void applyActive()
    {
        myRenderer.sprite = activeSprite;
        energized = true;
    }

    public void applyInactive()
    {
        myRenderer.sprite = inactiveSprite;
        energized = false;
    }

    private void Awake()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (energized)
        {
            applyActive();
        }
        else
        {
            applyInactive();
        }

        colliders = gameObject.GetComponents<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        transform.Rotate(0, 0, -90);
    }
}
