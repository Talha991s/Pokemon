//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.Audio;



//public class AudioManager : MonoBehaviour
//{
//    private AudioManager() { }

//    static AudioManager instance;
//    public AudioManager Instance
//    {
//        get 
//        {
//            if(instance == null)
//            {
//                instance =this;
//            }


//            return instance; 
//        }

//        private set { }

//    }



//    [SerializeField]
//    AudioSource musicScource;



//    [SerializeField]
//    AudioMixer masterMixer;

//    [SerializeField]
//    AudioClip[] musicTracks;

//    [SerializeField]
//    float volumeMax = 0.0f;

//    [SerializeField]
//    float volumeMin = -80.0f;

//    public enum Tracks
//    {
//        OverWorld,
//        Battle
//    }

//    // Start is called before the first frame update
//    public void Start()
//    {
//        //find other
//        //if they are not the original....Destroyit
//        AudioManager[] others = FindObjectsOfType<AudioManager>();
//        foreach(AudioManager mgr in others)
//        {
//            if (mgr != Instance)
//            {
//                Destroy(mgr.gameObject);
//            }
//        }
 
//        DontDestroyOnLoad(transform.root.gameObject);

//        //add music track to change listener
//        SpawnPoint.player.GetComponent<PlayerController>().onEnterEncounter.AddListener(EnterEncounterHandler);

//        SpawnPoint.player.GetComponent<PlayerController>().onExitEncounter.AddListener(OnExitEncounterhandler);
//    }


//    void EnterEncounterHandler()
//    {
//        FadeInTrack(Tracks.Battle);
//    }

//    void OnExitEncounterhandler()
//    {
//        FadeInTrack(Tracks.OverWorld);
//    }
//    /// <summary>
//    /// switch to selected track 
//    /// </summary>
//    /// <param name="trackId"></param>
//    public void PlayTrack(Tracks trackId)
//    {
//        musicScource.clip = musicTracks[(int)trackId];
//        musicScource.Play();
//    }

//    public void FadeInTrack(Tracks trackId)
//    {
//        musicScource.volume = 0;
//        PlayTrack(trackId);
//        StartCoroutine(RaiseVolume(3.0f));
//    }

//    IEnumerator  RaiseVolume(float transitionTime)
//    {
//        float timer = 0.0f;

//        while(timer < transitionTime)
//        {
//            timer += Time.deltaTime;
//            float normalizeTime = timer / transitionTime;
//            musicScource.volume = Mathf.SmoothStep(0, 1, normalizeTime);

//            yield return new WaitForEndOfFrame();
//        }
//    }

//    public void setMusicVolume(float normalizeVolume)
//    {
//        float dbVal = Mathf.Lerp(volumeMin, volumeMax, normalizeVolume);

//        masterMixer.SetFloat("Music Volume", normalizeVolume);
//    }
//}
