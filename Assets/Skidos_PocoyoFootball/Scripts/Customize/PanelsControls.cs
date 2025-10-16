using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions_Football;

public class PanelsControls : MonoBehaviour
{
    public List<RectTransform> PanelBgs;
    public List<GameObject> PanelContents;
    public List<Image> PanelImages;
    public Color DisableColor;
    public UIPositionTween ColorPanel;

    private void Start()
    {
        preIndex = 0;

        //foreach (GameObject panel in PanelContents)
        //    panel.SetActive(false);

        PanelBgs[preIndex].SetAsLastSibling();
        //PanelContents[preIndex].SetActive(true);
    }

    int preIndex = 0;
    public void SetPanel(int index)
    {
        PanelBgs[index].SetAsLastSibling();

        //PanelContents[preIndex].SetActive(false);
        //PanelContents[index].SetActive(true);

        //index 0 or 2 no color
        //index 1 or 3 color
        if (index % 2 != 0 && preIndex % 2 == 0)
            ColorPanel.PlayTween();

        if (index % 2 == 0 && preIndex % 2 != 0)
            ColorPanel.PlaybackTween();
        
        preIndex = index;
    }

    public void HideColorPanel()
    {
        ColorPanel.PlaybackTween();
    }

    public void ShowColorPanel()
    {
        ColorPanel.PlayTween();
    }

    public void EnablePanels()
    {
        foreach (Image img in PanelImages)
            img.color = Color.white;
    }

    public void DisablePanels()
    {
        foreach (Image img in PanelImages)
            img.color = DisableColor;
    }
}
