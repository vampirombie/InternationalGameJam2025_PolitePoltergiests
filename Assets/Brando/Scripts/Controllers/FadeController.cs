using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class FadeController : MonoBehaviour
{
    public static FadeController instance;
    [SerializeField] CanvasGroup cg;
    [SerializeField] float fadeDuration = 1f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void FadeIn()
    {
        cg.gameObject.SetActive(true);
        cg.interactable = true;
        cg.blocksRaycasts = true;

        // Fade in to alpha 1
        cg.DOFade(1, fadeDuration).OnComplete(() =>
        {
            // After fade in completes, fade out back to 0
            FadeOut();
        });

    }
    public void FadeOut()
    {
        cg.DOFade(0, fadeDuration).OnComplete(() =>
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
            cg.gameObject.SetActive(false);
        });
    }
}
