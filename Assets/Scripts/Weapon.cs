using System;
using UnityEditor;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Переменные для дебага
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip shotSound;
    [SerializeField] private AudioSource shotAudioSource;

    // Переменные внутреннего состояния
    private Vector2 currentFirePos;

    private void Awake()
    {
        if (firePoint == null)
        {
            firePoint = transform.gameObject.GetComponentInChildren<Transform>().Find("FirePoint");
        }

        if (shotAudioSource == null)
        {
            shotAudioSource = transform.gameObject.GetComponentInChildren<Transform>().Find("ShotAudioSource")
                .GetComponentInChildren<AudioSource>();
        }

        if (bulletPrefab == null)
        {
            bulletPrefab = AssetDatabase.LoadAssetAtPath<Bullet>("Assets/Prefabs/Bullet.prefab").gameObject;
        }

        if (shotSound == null)
        {
            shotSound = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Musics/AkmShootSound.mp3");
        }
    }

    private void Update()
    {
        // Эта строчка нужна для обновления текущей позиции firePoint
        // Из-за реализации стрельбы через OnClick в кнопке
        // без этой строчки пуля будет спавниться в одном и том же месте
        currentFirePos = firePoint.position;
    }

    public void Shoot()
    {
        // Если объекты для стрельбы не инициализированы
        if (firePoint == null || bulletPrefab == null) return;

        //TODO: возможно, сделать задержку стрельбы
        PlayShotSound();
        Instantiate(bulletPrefab, currentFirePos, firePoint.rotation);
    }

    private void PlayShotSound()
    {
        // Если объекты для проигрывания звука не инициализированы
        if (shotAudioSource == null || shotSound == null) return;

        shotAudioSource.PlayOneShot(shotSound);
    }
}