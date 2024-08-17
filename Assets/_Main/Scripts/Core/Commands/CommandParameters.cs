using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace COMMANDS
{
    public class CommandParameters //파라미터를 만들어서 동일한 형식으로 효율적이게 읽어내려함
    {
        private const char PARAMETER_IDENTIFIER = '-';//일종의 파라미터 시작을 의미

        private Dictionary<string, string> parameters = new Dictionary<string,string>();
        private List<string> unlabledParameters = new List<string>(); //파라미터를 생략했을 때 이쪽방향으로 가고 싶다.

        public CommandParameters(string[] parameterArray)
        {
            for (int i = 0; i < parameterArray.Length; i++)
            {
                if (parameterArray[i].StartsWith(PARAMETER_IDENTIFIER) && !float.TryParse(parameterArray[i], out _)) //식별자부터 시작하는지 판별, &&를 붙여서 파라미터가 생략된 플로트일수도 있는지 판별한다.
                {
                    string pName = parameterArray[i];
                    string pValue = "";

                    if (i + 1 < parameterArray.Length && !parameterArray[i+1].StartsWith(PARAMETER_IDENTIFIER)) //어레이 길이랑 판별해보고 파라미터로 시작하는지 같이본다.
                    {
                        pValue = parameterArray[i + 1];
                        i++; //다음 항목까지 위에서 추가해버렸기때문에 ++을 해서 넘겨준다.
                    }

                    parameters.Add(pName, pValue);
                }
                else
                {
                    unlabledParameters.Add(parameterArray[i]);
                }
            }
        }

        public bool TryGetValue<T>(string parameterName, out T value, T defaultValue = default(T)) => TryGetValue(new string[] { parameterName }, out value, defaultValue); //어떻게 입력될지 모르는 밸류값을 다양하게 받기위함.
        
        public bool TryGetValue<T>(string[] parameterNames, out T value, T defaultValue = default(T))
        {
            foreach (string parameterName in parameterNames)
            {
                if (parameters.TryGetValue(parameterName, out string parameterValue))
                {
                    if (TryCastParameter(parameterValue, out value))
                    {
                        return true; //우리가 찾고있던걸 찾았기 때문에 트루를 반환하도록한다.
                    }
                }
            }

            //만약 우리가 여기에 도달하면 정의된 파라미터가 없이 언레이블된 커맨드를 만나게된것.
            foreach (string parameterName in unlabledParameters)
            {
                if (TryCastParameter(parameterName, out value))
                {
                    unlabledParameters.Remove(parameterName);
                    return true; //우리가 찾고있던걸 찾았기 때문에 트루를 반환하도록한다.
                }
            }

            value = defaultValue; //다 찾아봤는데 없으면 디폴드밸류로 반환
            return false;
        }

        private bool TryCastParameter<T>(string parameterValue, out T value) //제네릭을 사용하면 object자료형을 쓰지 않아도 되고 여러 자료형을 생각할 수 있다
        {//=>함수에 전달할 매개변수를 찾으려고 노력을 했다.
            if (typeof(T) == typeof(bool)) //T값이 불이면
            {
                if (bool.TryParse(parameterValue, out bool boolValue))
                {
                    value = (T)(object)boolValue; //T값을 오브젝트로 변환해야된다.
                    return true;
                }
            }
            else if (typeof(T) == typeof(int)) //인트형
            {
                if (int.TryParse(parameterValue, out int intValue))
                {
                    value = (T)(object)intValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(float)) //플로트형
            {
                if (float.TryParse(parameterValue, out float floatValue))
                {
                    value = (T)(object)floatValue;
                    return true;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                value = (T)(object)parameterValue;
                return true;
            }

            value = default(T);
            return false;
        }

    }

}
