using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum PipeColor
{
    Green,
    Orange,
    Red,
    Purple,
    None
}

[Serializable]
public struct Color
{
    public PipeColor color;
    public Sprite sprite;
}

public class pipe : MonoBehaviour
{
    SpriteRenderer myRenderer;

    [Header("Sprites")]
    public Sprite inactiveSprite;

    [Header("General")]
    public bool canRotate = false;
    public bool energized = true;
    public bool genese = false;
    public bool receptor = false;

    [Header("Color related")]
    public PipeColor currentCollor = PipeColor.None;
    public PipeColor targetColor = PipeColor.None;
    public Color[] colors = null;
    public Dictionary<PipeColor, Sprite> activeSprites = new Dictionary<PipeColor, Sprite>();

    private BoxCollider2D[] colliders = null;
    private pipe[] pipes = null;
    public PipeColor markedColor = PipeColor.None;

    public void applyActive(PipeColor color)
    {
        currentCollor = color;
        myRenderer.sprite = activeSprites[color];
        energized = true;
    }

    public void applyInactive()
    {
        myRenderer.sprite = inactiveSprite;
        energized = false;
        currentCollor = PipeColor.None;
    }

    private void Awake()
    {
        colliders = gameObject.GetComponents<BoxCollider2D>();
        pipes = FindObjectsOfType<pipe>().Where(p => p != this && !p.genese).ToArray();

        foreach (Color color in colors)
        {
            activeSprites.Add(color.color, color.sprite);
        }

        myRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (energized)
        {
            applyActive(currentCollor);
        }
        else
        {
            applyInactive();
        }

        
    }

    public void cleanColor()
    {
        foreach(pipe p in pipes)
        {
            if (p.genese || p.receptor) continue;

            p.applyInactive();
        }
    }

    public void markPath(pipe root)
    {
        if (root.markedColor != PipeColor.None) return;

        root.markedColor = this.currentCollor;

        foreach(BoxCollider2D collider in root.colliders)
        {
            Collider2D[] overlaps = new Collider2D[30];
            collider.OverlapCollider(new ContactFilter2D().NoFilter(), overlaps);

            foreach(Collider2D overlap in overlaps)
            {
                if (!overlap) continue;
                if(!(overlap is BoxCollider2D)) continue;

                pipe p = overlap.gameObject.GetComponent<pipe>();

                if (p.receptor && p.targetColor != this.currentCollor) continue;

                markPath(p);    
            }

        }
    }

    void paintPath()
    {
        foreach(pipe p in pipes)
        {
            if (p.genese) continue;
            if(p.markedColor == PipeColor.None) continue;
            if(p.markedColor != this.currentCollor) continue;

            
            p.applyActive(this.currentCollor);
        }
    }

    void cleanPath()
    {
        foreach(pipe p in pipes)
        {
            if (p.genese || p.receptor) continue;
            
            if (
                p.markedColor == PipeColor.None
                && p.currentCollor != PipeColor.None 
                && p.currentCollor == this.currentCollor
            ) {
                p.applyInactive();
            }

            if(p.markedColor != PipeColor.None && p.markedColor == this.currentCollor)
                p.markedColor = PipeColor.None;
        }
    }

    private void Update()
    {
        if (this.genese)
        {
            this.markedColor = PipeColor.None;
            cleanPath();
            markPath(this);
            paintPath();
        } 
    }

    private void OnMouseDown()
    {
        if(!canRotate) return;
        transform.Rotate(0, 0, -90);
    }
}
