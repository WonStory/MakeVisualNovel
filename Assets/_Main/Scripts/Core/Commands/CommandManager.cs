using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;

namespace COMMANDS
{
    public class CommandManager : MonoBehaviour
    {
        public static CommandManager Instance { get; private set;} //인스턴스를 검색하기 위해 공개로 만들고 웨이크 내부에 할당하기 위해 비공개로 한다

        private CommandDatabase database;

        private List<CommandProcess> activeProcesses = new List<CommandProcess>(); //한번에 하나의 프로젝트를 하는것이 아님
        private CommandProcess topProcess => activeProcesses.Last(); //목록에서 최상위 프로젝트, 만족하는 첫번째 프로젝트를 반환하거나 디폴트값을 반환 => 였다가 라스트를 호출해야된다고 수정, 바로 직전에 만들어진 마지막 프로세스

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

        public CoroutineWrapper Execute(string commandName, params string[] args) //실행할 함수, params는 가변인수를 전달하게 해준다
        {
            Delegate command = database.GetCommand(commandName);

            if (command is null)
            {
                return null;
            }

            return StartProcess(commandName, command, args);
        }

        private CoroutineWrapper StartProcess(string commandName, Delegate command, string[] args)
        {
            System.Guid processID = System.Guid.NewGuid(); //고유 넘버를 생성한다
            CommandProcess cmd = new CommandProcess(processID, commandName, command, null, args, null);
            activeProcesses.Add(cmd);

            Coroutine co = StartCoroutine(RunningProcess(cmd));

            cmd.runningProcess = new CoroutineWrapper(this, co);

            return cmd.runningProcess;
        }

        public void StopCurrentProcess()
        {
            if (topProcess != null)
            {
                KillProcess(topProcess);
            }
        }

        public void StopAllProcesses()
        {
            foreach (var c in activeProcesses)
            {
                if (c.runningProcess != null && !c.runningProcess.IsDone) //러닝중이고 아직 완료 전이라면 스탑한다.
                {
                    c.runningProcess.Stop();
                }

                c.onTerminateAction?.Invoke(); //호출할 작업이 있으면 호출
            }

            activeProcesses.Clear();
        }

        private IEnumerator RunningProcess(CommandProcess process) //한번에 통합해서 넘길거라 명령과 인수로 넘기지 않음
        {
            yield return WaitingForProcessToComplete(process.command,process.args);

            KillProcess(process); //프로세스를 널이라고 하는게 아닌 킬프로세스해버림
        }

        public void KillProcess(CommandProcess cmd) //종료하려는 명령프로세스를 전달받고 리무브해버림
        {
            activeProcesses.Remove(cmd);

            if (cmd.runningProcess != null && !cmd.runningProcess.IsDone) //러닝프로세스가 있고 아직 안끝났으면 바로 스탑시킴
            {
                cmd.runningProcess.Stop();
            }

            cmd.onTerminateAction?.Invoke(); //종료할때 실행하도록 설정된 작업이 있으면(널이 아니면) 종료하고 계속해서 호출함
        }

        private IEnumerator WaitingForProcessToComplete(Delegate command, string[] args)
        {
            if (command is Action)
            {
                command.DynamicInvoke(); //구독자목록을 순차적으로 실행하면서 null아니어야 엑세스가 가능
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

        public void AddTerminationActionToCurrentProcess(UnityAction action)
        {
            CommandProcess process = topProcess;

            if (process == null) //최상위 프로세스가 없으면 아무것도 반환안함
            {
                return;
            }

            process.onTerminateAction = new UnityEvent();
            process.onTerminateAction.AddListener(action);
        }


    }

}
