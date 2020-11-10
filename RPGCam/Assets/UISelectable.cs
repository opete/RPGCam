using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum SelectableType
{
    Background,
    NPC,
    Hider,
    Muter,
    Focus,
    Dark,
    BackgroundHider,
    Attack,
    Damage,
    Refresh
}

public class UISelectable : MonoBehaviour, IScrollHandler
{
    public SelectableType selectableType;

    LoadAssets loadAssets;
    RawImage rawImage;
    ScrollRect scrollRect;

    void Start()
    {
        loadAssets = FindObjectOfType<LoadAssets>();
        rawImage = GetComponent<RawImage>();
        scrollRect = GetComponentInParent<ScrollRect>();

        switch (selectableType)
        {
            case SelectableType.Hider:
                rawImage.color = new Color(1, 1, 1, loadAssets.IsNPCHidden ? 0.5f : 1);
                break;
            case SelectableType.Focus:
                rawImage.color = new Color(1, 1, 1, loadAssets.IsFocus ? 0.5f : 1);
                break;
            case SelectableType.Dark:
                rawImage.color = new Color(1, 1, 1, loadAssets.IsDark ? 1 : 0.5f);
                break;
            case SelectableType.Muter:
                rawImage.color = new Color(1, 1, 1, loadAssets.IsMuted ? 0.5f : 1);
                break;
            case SelectableType.BackgroundHider:
                rawImage.color = new Color(1, 1, 1, loadAssets.IsBackgroundHidden ? 0.5f : 1);
                break;
            case SelectableType.Attack:
                rawImage.color = new Color(1, 1, 1, loadAssets.IsBackgroundHidden ? 0.5f : 1);
                break;
            case SelectableType.Damage:
                rawImage.color = new Color(1, 1, 1, loadAssets.IsBackgroundHidden ? 0.5f : 1);
                break;
        }
    }

    UISelectable GetSelectable(SelectableType selectableType)
    {

        foreach (UISelectable uISelectable in FindObjectsOfType<UISelectable>())
        {
            if (uISelectable.selectableType == selectableType)
            {
                return uISelectable;
            }
        }

        return null;
    }

    void Update()
    {
        
    }

    public void OnPointerClick()
    {
        switch (selectableType)
        {
            case SelectableType.Background:
                foreach (Background background in loadAssets.lstBackgrounds)
                {
                    if (background.obj == gameObject)
                    {
                        loadAssets.rendBackground.material.mainTexture = background.texture;
                    }
                }
                if (loadAssets.IsBackgroundHidden) {
                    GetSelectable(SelectableType.BackgroundHider).OnPointerClick();
                }
                break;
            case SelectableType.NPC:
                foreach (NPC npc in loadAssets.lstNPCs)
                {
                    if (npc.obj == gameObject)
                    {
                        loadAssets.textureHandlerNPC.SetNPC(npc);
                    }
                }
                if (loadAssets.IsNPCHidden)
                {
                    GetSelectable(SelectableType.Hider).OnPointerClick();
                }
                break;
            case SelectableType.Hider:
                loadAssets.IsNPCHidden = !loadAssets.IsNPCHidden;
                rawImage.color = new Color(1, 1, 1, loadAssets.IsNPCHidden ? 0.5f : 1);
                break;
            case SelectableType.Focus:
                loadAssets.IsFocus = !loadAssets.IsFocus;
                rawImage.color = new Color(1, 1, 1, loadAssets.IsFocus ? 0.5f : 1);
                break;
            case SelectableType.Dark:
                loadAssets.IsDark = !loadAssets.IsDark;
                rawImage.color = new Color(1, 1, 1, loadAssets.IsDark ? 1 : 0.5f);
                break;
            case SelectableType.Muter:
                loadAssets.IsMuted = !loadAssets.IsMuted;
                rawImage.color = new Color(1, 1, 1, loadAssets.IsMuted ? 0.5f : 1);
                break;
            case SelectableType.BackgroundHider:
                loadAssets.IsBackgroundHidden = !loadAssets.IsBackgroundHidden;
                rawImage.color = new Color(1, 1, 1, loadAssets.IsBackgroundHidden ? 0.5f : 1);
                break;
            case SelectableType.Attack:
                loadAssets.textureHandlerNPC.transform.localScale = Vector3.one * 1.25f;
                rawImage.color = new Color(1, 1, 1, loadAssets.IsBackgroundHidden ? 0.5f : 1);
                loadAssets.rendFlash.material.color = Color.white;
                break;
            case SelectableType.Damage:
                loadAssets.textureHandlerNPC.transform.localScale = Vector3.one * 0.9f;
                loadAssets.textureHandlerNPC.transform.position += new Vector3(0, -0.1f, 0);
                loadAssets.rendFlash.material.color = Color.red;
                rawImage.color = new Color(1, 1, 1, loadAssets.IsBackgroundHidden ? 0.5f : 1);
                break;
            case SelectableType.Refresh:
                loadAssets.LoadBackgrounds();
                loadAssets.LoadNPCs();
                break;

        }
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (scrollRect != null) {
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.scrollHandler);
        }
    }
}
