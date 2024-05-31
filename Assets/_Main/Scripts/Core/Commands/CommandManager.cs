using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using Unity.VisualScripting;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set;} //인스턴스를 검색하기 위해 공개로 만들고 웨이크 내부에 할당하기 위해 비공개로 한다
    private static Coroutine process = null;
    public static bool isRunningProcess => process != null;

    private CommandDatabase database;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            database = new CommandDatabase(); //데이터 베이스가 새 데이터 베이스와 같다고 해본다.

            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] extensionTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(CMD_DatabaseExtension))).ToArray(); //유형배열을 만들어야 된다.
        
            foreach (Type extension in extensionTypes)
            {
                MethodInfo extendMethod = extension.GetMethod("Extend"); //우리가 지정한 함수이름을 적는다.
                extendMethod.Invoke(null, new object[] { database }); //코드에서 null인지 확인한다.
            }
        
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public Coroutine Execute(string commandName, params string[] args) //실행할 함수, params는 가변인수를 전달하게 해준다
    {
        Delegate command = database.GetCommand(commandName);

        if (command is null)
        {
            return null;
        }

        return StartProcess(commandName, command, args);
    }

    private Coroutine StartProcess(string commandName, Delegate command, string[] args)
    {
        StopCurrentProcess();

        process = StartCoroutine(RunningProcess(command, args));

        return process;
    }

    private void StopCurrentProcess()
    {
        if (process != null)
        {
            StopCoroutine(process);
        }
        
        process = null;
    }

    private IEnumerator RunningProcess(Delegate command, string[] args)
    {
        yield return WaitingForProcessToComplete(command,args);


        process = null;
    }

    private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
    {
        if (command is Action)
        {
            command.DynamicInvoke();
        }
        else if (command is Action<string>)
        {
            command.DynamicInvoke(args[0]);
        }
        else if (command is Action<string[]>)
        {
            command.DynamicInvoke((object)args);
        }
        else if (command is Func<IEnumerator>)
        {
            yield return ((Func<IEnumerator>)command)();
        }
        else if (command is Func<string, IEnumerator>)
        {
            yield return ((Func<string, IEnumerator>)command)(args[0]);
        }
        else if (command is Func<string[], IEnumerator>)
        {
            yield return ((Func<string[], IEnumerator>)command)(args);
        }
    }




}
