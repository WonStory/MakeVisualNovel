using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CMD_DatabaseExtension_Example : CMD_DatabaseExtension // 작동하는 지 확인하기 위한 스크립트
{
    new public static void Extend(CommandDatabase database)
    {
        //매개변수 없이 커맨드를 넣겠다
        database.AddCommand("print", new Action(PrintDefaultMessage));
        database.AddCommand("print_1p", new Action<string>(PrintUserMessage));
        database.AddCommand("print_mp", new Action<string[]>(PrintLines));
        //매개변수 없이 람다를 넣겠다.
        database.AddCommand("lambda", new Action(()=> { Debug.Log("Printing a defalut message to console from lambda command"); }));
        database.AddCommand("lambda_1p", new Action<string>((arg) => { Debug.Log($"Log User Lambda Message: '{arg}'");}));
        database.AddCommand("lambda_mp", new Action<string[]>((args) => { Debug.Log(string.Join(", ", args)); }));
        //add 코루틴 with no parameters
        database.AddCommand("process", new Func<IEnumerator>(SimpleProcess));
        database.AddCommand("process_1p", new Func<string, IEnumerator>(LineProcess));
        database.AddCommand("process_mp", new Func<string[], IEnumerator>(MultiLineProcess));

        //스페셜 예제
        database.AddCommand("moveCharDemo", new Func<string, IEnumerator>(MoveCharacter));

    }

    private static void PrintDefaultMessage()
    {
        Debug.Log("Printing a defalut message to console");
    }

    private static void PrintUserMessage(string message)
    {
        Debug.Log($"User Message: '{message}'");
    }

    private static void PrintLines(string[] lines)
    {
        int i = 1;
        foreach (string line in lines)
        {
            Debug.Log($"{i++}. '{line}'");
        }
    }

    private static IEnumerator SimpleProcess()
    {
        for (int i = 1; i <= 5; i++)
        {
            Debug.Log($"Process Running... [{i}]");
            yield return new WaitForSeconds(1);
        }
    }

    private static IEnumerator LineProcess(string data)
    {
        if (int.TryParse(data, out int num))
        {
            for (int i = 1; i <= num; i++)
            {
                Debug.Log($"Process Running... [{i}]");
                yield return new WaitForSeconds(1);
            }
        }
        
    }

    private static IEnumerator MultiLineProcess(string[] data)
    {
        foreach (string line in data)
        {
            Debug.Log($"Process Message: '{line}'");
            yield return new WaitForSeconds(0.5f);
        }
    }

    private static IEnumerator MoveCharacter(string direction) //어느방향으로 이동할지 정하기 위해 스트링을 이용한다.
    {
        bool left = direction.ToLower() == "left";

        //겟 배리어블 필요하다. 
        Transform character = GameObject.Find("Image").transform;
        float moveSpeed = 15;


        //이미지 타겟 포지션을 계산
        float targetX = left ? -8 : 8;

        //이미지 현재 포지션을 계산
        float currentX = character.position.x;

        //타겟포지션으로 점차적으로 움직인다..
        while (Mathf.Abs(targetX - currentX) > 0.1f)
        {
            currentX = Mathf.MoveTowards(currentX, targetX, moveSpeed = Time.deltaTime);
            character.position = new Vector3(currentX, character.position.y, character.position.z);
            yield return null;
        }
    }

}
