using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AtlasManager))]
public class MasterManager : MonoBehaviour
{

    private List<IManager> _managerList = new List<IManager>();

    public static AtlasManager atlasManager { get; private set; }

    private void Awake()
    {
        atlasManager = GetComponent<AtlasManager>();
        _managerList.Add(atlasManager);

        StartCoroutine(BootAllManagers());
    }

    private IEnumerator BootAllManagers()
    {
        foreach(IManager manager in _managerList)
        {
            manager.BootSequence();
        }
        yield return null;
    }
}
