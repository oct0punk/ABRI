using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cinematic : MonoBehaviour
{
    VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(HideOnEnd());
    }

    IEnumerator HideOnEnd()
    {
        yield return new WaitForSecondsRealtime((float)videoPlayer.clip.length);
        videoPlayer.enabled = false;
        GameManager.instance.Resume();
    }
}
