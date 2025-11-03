

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

class CTextParser
{
    private byte[] _buffer;

    public bool OpenTextFile(string fileName)
    {
        if (File.Exists(fileName) == false)
        {
            Debugger.Break();
            return false;
        }

        _buffer = File.ReadAllBytes(fileName);

        return true;
    }

    public void CloseTextFile()
    {
    //파일 스트림 종료
    
    }


    public bool SearchData<T>(out T dest, string textName)
    {
        

    }

    private void SkipNoneCommand()
    {

    }


}