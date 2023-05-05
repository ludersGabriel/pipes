using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Manager : MonoBehaviour
{
    pipe genese;
    pipe receptor;
    pipe[] pipes;

    int[] rotations = { 0, 90, 180, 270 };

    private void Awake()
    {   
        pipes= FindObjectsOfType<pipe>();

        genese = pipes.Where(c => c.genese).ToArray()[0];
        receptor = pipes.Where(c => c.receptor).ToArray()[0];


        foreach(pipe p in pipes)
        {
            if (p.genese || p.receptor) continue;
            p.transform.Rotate(0, 0, rotations[Random.Range(0, rotations.Length)]);
        }
    }


    public void paintPipes(pipe genese)
    {
        if (genese.painted) return;

        genese.painted = true;
        
        BoxCollider2D[] genesesColliders = genese.GetComponents<BoxCollider2D>();
        CircleCollider2D genesesCircle = genese.GetComponent<CircleCollider2D>();

        foreach(BoxCollider2D col in genesesColliders) {
            Collider2D[] overlaps = new Collider2D[20];
            col.OverlapCollider(new ContactFilter2D().NoFilter(), overlaps);


            foreach(Collider2D overlap in overlaps)
            {
                if (overlap && (overlap is BoxCollider2D))
                {
                    pipe overlapPipe = overlap.gameObject.GetComponent<pipe>();

                    overlapPipe.applyActive();
                    paintPipes(overlapPipe);
                }
            }
        }
    }

    public void cleanPaint()
    {
        foreach(pipe p in pipes)
        {
            p.painted = false;
            if(p != genese && p != receptor)
                p.applyInactive();
        }
    }

    private void Start()
    {
        cleanPaint();
        paintPipes(genese);
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            cleanPaint();
            paintPipes(genese);

            if (receptor.energized)
            {
                Debug.Log("You win!");
            }
        }
    }
}
