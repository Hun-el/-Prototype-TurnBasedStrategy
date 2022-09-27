using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private bool show;

    public void Show(Material material)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = material;
        show = true;

        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(Vector3.one, 0.25f);
    }

    public void Hide()
    {
        show = false;

        transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            if (show) return;

            meshRenderer.enabled = false;
        });
    }
}
