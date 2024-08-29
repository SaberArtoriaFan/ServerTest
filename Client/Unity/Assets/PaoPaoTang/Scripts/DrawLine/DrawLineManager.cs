using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineManager : MonoBehaviour
{
    public Camera UICamera;
    public LineRenderer lineRendererPF;
    public LineRenderer curLineRenderer;
    public Transform lineRendererParent;
    public UIButton btnClear;
    private List<LineRenderer> lineRenderers;

    private void Awake()
    {
        lineRenderers = new List<LineRenderer>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(curLineRenderer != null && lineRenderers.Contains(curLineRenderer) == false)
            {
                lineRenderers.Add(curLineRenderer);
                curLineRenderer = null;
            }
            curLineRenderer = GameObject.Instantiate(lineRendererPF) as LineRenderer;
            curLineRenderer.transform.parent = lineRendererParent;
            curLineRenderer.positionCount = 0;
            curLineRenderer.gameObject.SetActive(true);
        }        
        if(Input.GetMouseButtonUp(0))
        {
            if (curLineRenderer != null && lineRenderers.Contains(curLineRenderer) == false)
            {
                if(curLineRenderer.positionCount <= 2 && Vector2.Distance(curLineRenderer.GetPosition(0), curLineRenderer.GetPosition(1)) < 0.02f)
                {
                    Destroy(curLineRenderer.gameObject);
                    curLineRenderer = null;
                    return;
                }
                lineRenderers.Add(curLineRenderer);
                curLineRenderer = null;
            }
        }
        if(Input.GetMouseButton(0)) 
        {
            if (curLineRenderer != null)
            {
                Vector2 pos = UICamera.ScreenToWorldPoint(Input.mousePosition);
                if(curLineRenderer.positionCount > 1)
                {
                    if (Vector2.Distance(curLineRenderer.GetPosition(curLineRenderer.positionCount - 1), pos) < 0.02f)
                        return;
                }
                curLineRenderer.positionCount++;
                curLineRenderer.SetPosition(curLineRenderer.positionCount - 1, pos);
                if (curLineRenderer.positionCount > 1)
                {
                    Vector2 point1 = curLineRenderer.GetPosition(curLineRenderer.positionCount - 2);
                    Vector2 point2 = curLineRenderer.GetPosition(curLineRenderer.positionCount - 1);
                    GameObject collGO = new GameObject("Coll");
                    collGO.transform.parent = curLineRenderer.transform;
                    collGO.transform.localPosition = (point1 + point2) / 2;
                    BoxCollider2D coll = collGO.AddComponent<BoxCollider2D>();
                    coll.size = new Vector2((point1 - point2).magnitude, curLineRenderer.endWidth);
                    coll.transform.right = (point1 - point2).normalized;
                }
            }
        }
    }

    public void ClearAllLine()
    {
        int lineCount = lineRendererParent.childCount;
        for(int i = lineCount - 1; i >= 0; i--)
        {
            Transform line = lineRendererParent.GetChild(i);
            GameObject.Destroy(line.gameObject);
        }
    }
}
