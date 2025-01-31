using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private CanvasGroup image;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private float transitionTimeSpeed;

    public void LoadScene(int sceneLoadNumber)
    {
        fadeCanvas.SetActive(true);
        StartCoroutine(Transition(sceneLoadNumber));
    }

    private IEnumerator Transition(int sceneNumber)
    {
        // While the image is not fully visible, then it fades in.
        while(image.alpha != 1f)
        {
            image.alpha += transitionSpeed;

            if(image.alpha > 1f)
            {
                image.alpha = 1f;
            }

            yield return new WaitForSeconds(transitionTimeSpeed);
        }

        SceneManager.LoadScene(sceneNumber);
    }
}