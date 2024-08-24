using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CoroutineWrapper
{
    private MonoBehaviour owner;
    private Coroutine coroutine;

    public bool IsDone = false; //코루틴이 완료되었는지 여부를 알려줌

    public CoroutineWrapper(MonoBehaviour owner, Coroutine coroutine)
    {
        this.owner = owner;
        this.coroutine = coroutine;
    }

    public void Stop()
    {
        owner.StopCoroutine(coroutine);
        IsDone = true;
    }
}    


