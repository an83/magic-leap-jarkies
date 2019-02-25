using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Singleton { get; private set; }

    private AudioSource bgm;
    private AudioSource aliens;

    public AudioClip bgmclip;
    public AudioClip yay;
    public AudioClip aww;

    private bool towerCollapsed = false;

    // Use this for initialization
    void Start() {
        if (Singleton != null) {
            Destroy(this);
            return;
        }
        Singleton = this;
        bgm = gameObject.AddComponent<AudioSource>();
        bgm.loop = true;
        bgm.volume = 0.3f;

        aliens = gameObject.AddComponent<AudioSource>();
        
    }

    public void StartGame() {
        bgm.Play();
    }
    
    public void TowerCollapse() {
        if (towerCollapsed) {
            return;
        }
        towerCollapsed = true;
        aliens.clip = aww;
        aliens.Play();
        bgm.Stop();
    }

    public void BridgePlaced() {
        aliens.clip = yay;
        aliens.Play();
    }
}
