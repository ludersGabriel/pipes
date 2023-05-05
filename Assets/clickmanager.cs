using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickmanager : MonoBehaviour {

  // Update is called once per frame
  void Update() {
    if(Input.GetMouseButtonDown(0))
    {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousesPos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousesPos2D, Vector2.zero);

            if (hit.collider && hit.collider.gameObject.GetComponent("pipe"))
            {
                pipe p = hit.collider.gameObject.GetComponent<pipe>();

                if (p.canRotate)
                {
                    p.transform.Rotate(0, 0, -90);
                }

            }
    }

  }
}
