

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


    public bool SearchData<T>(out T dest, string textName) where T : struct
    {
        _bufferPos = 0;
        int targetLen = _buffer.Length;

        while (true)
        {
            if (_bufferPos >= targetLen - 1)
            {
                dest = default(T);
                return false;
            }

            SkipNoneCommand(); //유효한 문자열까지 점프
            string saveBuf = "";
            while (true)
            {
                //문자열을 받아서 저장하고 
                saveBuf += (char)_buffer[_bufferPos];

                _bufferPos++;

                if (_buffer[_bufferPos] == ' ' || _buffer[_bufferPos] == '\n' || _buffer[_bufferPos] == 0x09 || _buffer[_bufferPos] == 0x08 ||
                    _bufferPos >= targetLen - 1 ||
                    (_buffer[_bufferPos] == '/' && _buffer[_bufferPos + 1] == '/') || (_buffer[_bufferPos] == '/' && _buffer[_bufferPos + 1] == '*'))
                {
                    break;
                }
            }

            if (saveBuf != textName)
            {
                continue;
            }

            SkipNoneCommand();

            if (_buffer[_bufferPos] == '=')
            {
                _bufferPos++;
                SkipNoneCommand();

                if (_buffer[_bufferPos] >= 0x30 && _buffer[_bufferPos] <= 0x39) //첫 문자가 정수의 범위에 들어있다면 숫자로 간주
                {
                    string numBuf = "";
                    int i = 0;
                    while (_buffer[_bufferPos] >= 0x30 && _buffer[_bufferPos] <= 0x39)
                    {
                        numBuf += (char)_buffer[_bufferPos];
                        _bufferPos++;
                    }

                    int number;
                    if (int.TryParse(numBuf, out number) == false)
                    {
                        Debugger.Break();
                        dest = default(T);
                        return false;
                    }

                    dest = (T)Convert.ChangeType(number, typeof(T));
                    return true;
                }
            }
        }

        return true;
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