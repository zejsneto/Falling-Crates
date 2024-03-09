using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private RectTransform scoreRectTransform;

    void Start()
    {
        // Hide Score from Main Menu
        scoreRectTransform.anchoredPosition = 
            new Vector2(scoreRectTransform.anchoredPosition.x,0);

        // Animate the Play Button Text
        GetComponentInChildren<TMPro.TextMeshProUGUI>().
            gameObject.LeanScale(new Vector3(1.2f, 1.2f), 0.4f)
            .setLoopPingPong();
    }

    public void Play()
    {
        // Transparent Transition to Main Menu
        GetComponent<CanvasGroup>().LeanAlpha(0, 0.3f)
            .setOnComplete(OnComplete);    
    }

    private void OnComplete()
    {
        // Brings score back to main screen
        scoreRectTransform.LeanMoveY(-58, 0.75f).setEaseOutBounce();

        gameManager.Enable();
        Destroy(gameObject);
    }
}
