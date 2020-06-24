using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/**
 * Script to stream a video on an image (rawImage in this case)
 */
public class StreamVideo : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    
    private IEnumerator video;

    public void Play() 
    {
        video = PlayVideo();
        StartCoroutine(video);
    }

    public void Stop()
    {
        videoPlayer.Stop();
        StopCoroutine(video);
    }

    private IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        var waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
}