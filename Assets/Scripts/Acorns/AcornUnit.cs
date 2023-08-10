using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;
using static UnityEditor.Rendering.CameraUI;

public class AcornUnit : MonoBehaviour, IInteractable
{
    public static event EventHandler onAnyAcornSpawned;

    public event EventHandler onAcornDespawned;

    private IObjectPool<GameObject> pool;

    private bool onClicked;
    private float despawnTimer;
    private float autoDespawnTimer;

    private void Start()
    {
        onAnyAcornSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (transform.position.y < -10f)
        {
            ReleaseObject();
        }

        autoDespawnTimer -=Time.deltaTime;

        if(autoDespawnTimer < 0 && !onClicked)
        {
            DisableAcorn();
            onAcornDespawned?.Invoke(this, EventArgs.Empty);
        }

        if (!onClicked)
        {
            return;
        }

        despawnTimer -= Time.deltaTime;

        if(despawnTimer <= 0f)
        {
            onClicked = false;
            ReleaseObject();
        }

    }

    private void OnEnable()
    {
        autoDespawnTimer = 15f;
    }

    private void DisableAcorn()
    {
        onClicked = true;
        despawnTimer = 0.75f;
    }

    public void ClickInteraction()
    {
        DisableAcorn();
        onAcornDespawned?.Invoke(this, EventArgs.Empty);
    }

    public IObjectPool<GameObject> Pool 
    { 
        set { pool = value; }
    }

    public void ReleaseObject() => pool.Release(gameObject);

}
