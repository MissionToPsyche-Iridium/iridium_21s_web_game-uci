using System.Collections;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public RectTransform rope;
    public GameObject secondPuzzle;
    public float dropDistance = 0.9f;
    public float dropSpeed = 6f;
    public float swingDuration = 3f;

    public IEnumerator StartPuzzleAnimation()
    {
        Vector2 initialPos = new Vector2(rope.anchoredPosition.x, Screen.height * 1.3f);
        rope.anchoredPosition = initialPos;

        Vector2 targetPos = rope.anchoredPosition - new Vector2(0, Screen.height * dropDistance);
        LeanTween.move(rope, targetPos, dropSpeed).setEase(LeanTweenType.easeOutQuart);

        yield return new WaitForSeconds(dropSpeed);

        LeanTween.rotateAroundLocal(rope.gameObject, Vector3.forward, 10f, swingDuration)
            .setLoopPingPong(2).setEaseInOutSine();

        yield return new WaitForSeconds(swingDuration);

        LeanTween.moveY(secondPuzzle, targetPos.y, 2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                LeanTween.moveY(secondPuzzle, targetPos.y + 10f, 2f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setLoopPingPong(3);
            });
        
        yield return new WaitForSeconds(3f);

    }
}
