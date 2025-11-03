

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

class CTextParser
{
    private byte[] _buffer;
    private int _bufferPos = 0;

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
	while (true) {
		if (_buffer[_bufferPos] == ' ' || _buffer[_bufferPos] == '\n' || _buffer[_bufferPos] == 0x09 || _buffer[_bufferPos] == 0x08)
		{
            _bufferPos++;
		}
		else if (_buffer[_bufferPos] == '/' && (_buffer[_bufferPos] + 1) == '/')
		{
			while (_buffer[_bufferPos] != '\n') {
				_bufferPos++;
			}

		}
		else if (_buffer[_bufferPos] == '/' && _buffer[_bufferPos+ 1] == '*') {
			while (_buffer[_bufferPos - 1] != '*' || _buffer[_bufferPos] != '/')
			{
				_bufferPos++;

				if (_bufferPos >= _buffer.Length - 1) {
                        // 주석 해제 문자가 없는데 버퍼의 끝에 도달하면 오류로 간주
                        Debugger.Break();
					return;
				}
			}
			_bufferPos++;
		}
		else {
			break;
		}
	}
    }


}