using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public abstract class CMD_DatabaseExtension //이 글래스는 사용할 수 없고 인스턴스로 사용가능, 상속받는 게 있어야된다.
    {
        public static void Extend(CommandDatabase database)
        {

        }

        public static CommandParameters ConvertDataToParameters(string[] data) => new CommandParameters(data); //데이터 배열을 가져온다음 새 커맨드 파라미터로 씀
        

    }
}

