using UnityEngine;
using UnityEngine.UI;

public class Mask3D : MonoBehaviour
{
    private Transform[] Points;
    private Renderer[] Renderers;
    private RectTransform[] Grids;
    private GridLayoutGroup group;
    
    private void Awake()
    {
        Grids = new RectTransform[transform.GetChild(1).childCount];
        for(int i=0;i<Grids.Length;++i)
        {
            Grids[i] = transform.GetChild(1).GetChild(i).GetComponent<RectTransform>();
            Debug.Log(Grids[i].position);
        }

        //Vector3[] vertices = GetBorder(rectTransform);
        //Renderers = GetComponentsInChildren<Renderer>();
    }

    bool IsInBorder(Vector3 point,Vector3[] vertices)
    {

        Vector3 projectionPoint = new Vector3(point.x, point.y, 0);
        Debug.Log(point);

        if (projectionPoint.x > vertices[0].x && projectionPoint.x > vertices[2].x || projectionPoint.x < vertices[0].x && projectionPoint.x < vertices[2].x
        || projectionPoint.y > vertices[0].y && projectionPoint.y > vertices[1].y || projectionPoint.y < vertices[0].y && projectionPoint.y < vertices[1].y)
        {
            Debug.Log(false);
            return false;
        }
        else
        {
            Debug.Log(true);
            return true;
        }
    }

    Vector3[] GetBorder(RectTransform rectTransform)
    {
        Vector3 scale = GetScale(rectTransform);
        Vector3[] vertices = new Vector3[4];
        vertices[0] = (rectTransform.position + rectTransform.rotation*new Vector3(-rectTransform.rect.width * scale.x, rectTransform.rect.height * scale.y, 0) * 0.5f);
        vertices[1] = (rectTransform.position + rectTransform.rotation*new Vector3(-rectTransform.rect.width * scale.x, -rectTransform.rect.height * scale.y,0) * 0.5f);
        vertices[2] = (rectTransform.position + rectTransform.rotation*new Vector3(rectTransform.rect.width * scale.x, -rectTransform.rect.height * scale.y, 0) * 0.5f);
        vertices[3] = (rectTransform.position + rectTransform.rotation*new Vector3(rectTransform.rect.width * scale.x, rectTransform.rect.height * scale.y, 0) * 0.5f);
        for(int i=0;i<vertices.Length;++i)
        {
            vertices[i].z = 0;
        }
        return vertices;
    }

    Vector3 GetScale(RectTransform rectTransform)
    {
        Transform parent = rectTransform.parent;
        Vector3 scale = rectTransform.localScale;
        while (parent!=null)
        {            
            scale = new Vector3(scale.x*parent.localScale.x,scale.y*parent.localScale.y,scale.z*parent.localScale.z);
            parent = parent.parent;
        }
        return scale;
    }
}
