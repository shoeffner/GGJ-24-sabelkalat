using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdReaction : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private List<AudioClip> comedianSounds;

    [SerializeField] private List<AudioClip> goodJokeSounds;
    [SerializeField] private List<AudioClip> deadJokeSounds;

    [SerializeField] private List<AudioClip> laughingSounds;
    [SerializeField] private List<AudioClip> booingSounds;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        GameOrganizer.Instance.OnScoreChanged += React;
    }

    private void React(int score)
    {
        StartCoroutine(PlayCrowdSounds(score));
    }

    private IEnumerator PlayCrowdSounds(int score)
    {
        audioSource.clip = comedianSounds[Random.Range(0, comedianSounds.Count)];
        audioSource.Play();

        // Wait for the joke to be told
        CountdownTimer maxWaitTimer = new CountdownTimer(5);
        maxWaitTimer.Start();
        yield return new WaitUntil(() => !audioSource.isPlaying || maxWaitTimer.IsFinished);
        yield return new WaitForSeconds(0.3f);

        var reactionSound = score > 0 ? goodJokeSounds[Random.Range(0, goodJokeSounds.Count)] : deadJokeSounds[Random.Range(0, deadJokeSounds.Count)];
        audioSource.PlayOneShot(reactionSound);
        yield return new WaitForSeconds(1f);

        //only laughing sounds for now
        if (score > 0)
        {
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                audioSource.PlayOneShot(laughingSounds[Random.Range(0, laughingSounds.Count)]);
                yield return new WaitForSeconds(Random.Range(0, 0.1f));
            }

        }
    }
}