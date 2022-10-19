using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
/*
过场动画控制器，离开，暂停，播放
*/
public class PlayerAnimator : MonoBehaviour
{
    VideoPlayer player;

    private void Start()
    {
        player = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            SceneManager.LoadScene("Login");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (player.isPlaying)
            {
                player.Pause();
            }
            else
            {
                player.Play();
            }
        }
    }
}

