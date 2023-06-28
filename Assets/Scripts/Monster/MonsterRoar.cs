using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRoar : MonoBehaviour
{
    [SerializeField] float cooldown = 3;
    [SerializeField] VictimAI victim;
    PlayerInputs inputs;
    PlayerMovement movement;
    AudioManager audioManager;
    void Awake()
    {
        inputs = GetComponent<PlayerInputs>();
        audioManager = GetComponentInChildren<AudioManager>();
        movement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        StartCoroutine(Roar());
    }
    IEnumerator Roar()
    {
        while (true)
        {
            yield return new WaitUntil(() => inputs.RoarPress);
            if (movement.frozen)
                continue;
            audioManager.PlaySFX(0, 1.5f);
            victim.DetectMonsterRoar();
            yield return new WaitForSeconds(cooldown);
        }
    }
}
