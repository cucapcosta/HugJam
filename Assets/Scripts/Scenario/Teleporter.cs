using UnityEngine;
using UnityEngine.UI; // Make sure to include this for Image
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Vector2 endPosition;
    public GameObject thisCamera;
    public GameObject nextCamera;
    public Image fadeImage; // Reference to a UI Image that covers the entire screen
    public float fadeDuration = 1f;
    public string nextRoom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(TeleportWithFade(collision.gameObject)); 
        }
    }

    private IEnumerator TeleportWithFade(GameObject player)
    {
        // Fade out
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);

        // Teleport and switch cameras
        player.transform.position = endPosition;
        thisCamera.SetActive(false);
        nextCamera.SetActive(true);

        // Fade in
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0f);
        player.GetComponent<KaosBehaviour>().currentRoom = nextRoom;
    }
}