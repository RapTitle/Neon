using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private MusicPlayer player;
    private void Start()
    {
        player.AudioPlay();
    }
}

