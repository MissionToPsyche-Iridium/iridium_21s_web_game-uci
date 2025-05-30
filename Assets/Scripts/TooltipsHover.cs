using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TooltipsHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Coroutine visibleTimer;
    public GameObject tooltip;
    public GameObject tooltipArrow;
    private float delay = 0.5f;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        visibleTimer = StartCoroutine(ShowTooltip(delay));
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (visibleTimer != null)
        {
            StopCoroutine(visibleTimer);
            visibleTimer = null;
        }

        TooltipsManager.instance.TooltipInvisible(tooltip);
        TooltipsManager.instance.TooltipInvisible(tooltipArrow);
    }

    IEnumerator ShowTooltip(float delay)
    {
        yield return new WaitForSeconds(delay);
        TooltipsManager.instance.TooltipVisible(tooltip);
        TooltipsManager.instance.TooltipVisible(tooltipArrow);
    }
}
