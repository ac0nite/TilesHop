using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : SingletoneGameObject<UIManager>
{
    private List<UIBasePanel> _panels = null;
    protected override void Awake()
    {
        base.Awake();
        _panels = GetComponentsInChildren<UIBasePanel>().ToList();
       // UIManager.Instance.GetPanel(UITypePanel.Start).EventStart += 
    }
    
    void Start()
    {
        ShowPanel(UITypePanel.Start);
    }

    public UIBasePanel GetPanel(UITypePanel type)
    {
        foreach (var panel in _panels)
        {
            if(panel.Type == type)
                return panel;
        }

        return null;
    }
    public void ShowPanel(UITypePanel type)
    {
        foreach (var panel in _panels)
        {
            panel.gameObject.SetActive(type == panel.Type);
        }
    }
}
