using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourcesManager : Singleton<ResourcesManager>
{
    #region Quản lí máu của tài nguyên

    [SerializeField] private List<ResourceHealth> resourceHealthList = new List<ResourceHealth>();   
    public List<ResourceHealth> ResourceHealthList => resourceHealthList;

    public void OnStart()
    {
        foreach (var resource in resourceHealthList)
        {
            resource.Init();
        }
    }

    public void CheckResourceIsDead(ResourceHealth obj)
    {
        if (obj.IsDead)
        {
            Removelist(obj);
        }
    }

    public void Addlist(ResourceHealth obj)
    {
        resourceHealthList.Add(obj);    
    }

    public void Removelist(ResourceHealth obj)
    {
        resourceHealthList.Remove(obj);
    }


    #endregion
}
