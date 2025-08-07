using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private ResourcesManager resourcesManager;

    private void Awake()
    {
        
    }

    private void Start()
    {
        resourcesManager.OnStart();
    }

    private void Update()
    {
        
    }
}
