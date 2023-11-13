using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{
    [SerializeField] private AudioVisualization _audioVisualition;

    public void TestC()
    {
        _audioVisualition.StopMusic();
        NativeGallery.GetAudioFromGallery(Au, "Selected");
    }

    private void Au(string filePath)
    {
        StartCoroutine(Callback($"file://{filePath}"));
    }

    private IEnumerator  Callback(string filePath)
    {
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(filePath,AudioType.MPEG))
        {
            
            yield return uwr.SendWebRequest();

            if(uwr.error == null)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                _audioVisualition.MusicFadeOut(clip);
            }
        }
    }
}

