using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set;} //인스턴스를 검색하기 위해 공개로 만들고 웨이크 내부에 할당하기 위해 비공개로 한다
    
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

    public void Execute(string commandName) //실행할 함수
    {
        Delegate command = database.GetCommand(commandName);

        if (command != null)
        {
            command.DynamicInvoke();
        }

    }









}
