using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestStats : MonoBehaviour
{
    private SpriteRenderer stats;


    [Inject]
    private void GetResourceService(ResourceService resourceService)
    {
        stats = GetComponent<SpriteRenderer>();

        //stats.sprite = resourceService.Icons.GetBy(0);  

    }
}
