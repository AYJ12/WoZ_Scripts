using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndPoint : MonoBehaviour
{
    private NpcInteractAI _npcInteractAI;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == Player.Instance.gameObject)
        {
            _npcInteractAI = NpcObject.Instance.GetComponent<NpcInteractAI>();
            if (_npcInteractAI.isActive)
                Manager.Scene.LoadScene(Define.Scene.EndScene);
        }
    }
}
