using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private Outline outline;

    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.OutlineWidth = 30;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }
    
    public CardValues steps;

    public void Highlight() => Highlight(new Color(0.78f, 0.41f, 0.39f));
    public void Highlight(Color color)
    {
        if (outline == null) Start();
        outline.enabled = true;
        outline.OutlineColor = color;
    }

    public void Dim()
    {
        if (outline == null) Start();
        outline.enabled = false;
    }

}
