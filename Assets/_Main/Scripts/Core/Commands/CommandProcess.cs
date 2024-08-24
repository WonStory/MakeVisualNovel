using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


public class CommandProcess
{
    public Guid ID;
    public string processName;
    public Delegate command; //명령 대리자
    public CoroutineWrapper runningProcess; //실행중인 프로젝트
    public string[] args; //인수들

    public UnityEvent onTerminateAction; //강제 종료 같은 이벤트

    public CommandProcess(Guid id, string processName, Delegate command, CoroutineWrapper runningProcess, string[] args, UnityEvent onTerminateAction = null) //기본적으로 종료이벤트는 널로 설정
    {
        ID = id;
        this.processName = processName;
        this.command = command;
        this.runningProcess = runningProcess;
        this.args = args;
        this.onTerminateAction = onTerminateAction;
    }
}
