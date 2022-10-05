using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private Button b1;
    [SerializeField] private Button b2;
    [SerializeField] private Text txt;
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] private ColorBlock cb;
    private float timePassed = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timePassed < 1f)
        {
            timePassed += Time.deltaTime;
            panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, fadeCurve.Evaluate(timePassed)*0.6f);
            cb.normalColor = new Color(cb.normalColor.r, cb.normalColor.g, cb.normalColor.b, fadeCurve.Evaluate(timePassed));
            b1.colors = cb;
            b2.colors = cb;
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, fadeCurve.Evaluate(timePassed));
        }
    }

    public void Reset()
    {
        timePassed = 0f;
        panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, fadeCurve.Evaluate(timePassed) * 0.6f);
        cb.normalColor = new Color(cb.normalColor.r, cb.normalColor.g, cb.normalColor.b, fadeCurve.Evaluate(timePassed));
        b1.colors = cb;
        b2.colors = cb;
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, fadeCurve.Evaluate(timePassed));
    }
}
