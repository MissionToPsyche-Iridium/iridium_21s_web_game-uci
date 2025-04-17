using System.Collections;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public RectTransform rope;
    public GameObject secondPuzzle;
    public float dropSpeed = 6f;
    public float swingDuration = 2f;

    public float initial = 1.3f;
    public float target = 0.25f;
    
    private Canvas parentCanvas;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public IEnumerator StartPuzzleAnimation()
    {
        // Get Canvas Height from the parent canvas
        float canvasHeight = parentCanvas.GetComponent<RectTransform>().rect.height;

        // Set initial position and target position of the rope
        float initialPosX = rope.anchoredPosition.x;
        float initialPosY = canvasHeight * initial;
        float targetPosY = canvasHeight * target;

        // Start the animation
        Vector2 initialPos = new Vector2(initialPosX, initialPosY);
        rope.anchoredPosition = initialPos;

        Vector2 targetPos = new Vector2(initialPosX, targetPosY);
        LeanTween.move(rope, targetPos, dropSpeed).setEase(LeanTweenType.easeOutQuart);

        // Wait for the drop animation to finish
        yield return new WaitForSeconds(5f);

        // Start the swing animation
        LeanTween.rotateAroundLocal(rope.gameObject, Vector3.forward, 10f, swingDuration)
            .setLoopPingPong(2).setEaseInOutSine();

        yield return new WaitForSeconds(swingDuration);

        // Set values and start the second puzzle animation
        float puzzlePosY = Screen.height * 0.4f;
        float secondPosY = puzzlePosY + targetPosY*0.3f;

        LeanTween.moveY(secondPuzzle, puzzlePosY, 2f)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                LeanTween.moveY(secondPuzzle, secondPosY, 2f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setLoopPingPong(3);
            });

        yield return new WaitForSeconds(3f);

    }
}
