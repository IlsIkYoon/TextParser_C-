

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

    public bool SearchData<T>(out T dest, string textName) where T : struct
    {
        _bufferPos = 0;
        int targetLen = _buffer.Length;

        while (true)
        {
            if (IsBufferPosEnd())
            {
                dest = default(T);
                return false;
            }

            SkipNoneCommand();
            string saveBuf = "";
            while (true)
            {
                //문자열을 받아서 저장하고 
                saveBuf += (char)_buffer[_bufferPos];

                _bufferPos++;

                if (IsNotChar())
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

                if (IsNumberData()) //첫 문자가 정수의 범위에 들어있다면 숫자로 간주
                {
                    string numBuf = "";
                    while (IsNumberData())
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


    public bool SearchData(out string dest, string textName)
    {
        _bufferPos = 0;
        int targetLen = _buffer.Length;

        while (true)
        {
            if (IsBufferPosEnd())
            {
                dest = null;
                return false;
            }

            SkipNoneCommand();
            string saveBuf = "";
            while (true)
            {
                //문자열을 받아서 저장하고 
                saveBuf += (char)_buffer[_bufferPos];
                _bufferPos++;

                if (IsNotChar())
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

                if (_buffer[_bufferPos] == '"')
                {
                    string strBuf = "";
                    _bufferPos++;
                    while (_buffer[_bufferPos] != '"')
                    {
                        if (IsBufferPosEnd()) //도중에 버퍼에 끝에 도달했다면
                        {
                            Debugger.Break();
                            dest = null;
                            return false;
                        }

                        strBuf += (char)_buffer[_bufferPos];
                        _bufferPos++;
                    }
                    dest = strBuf;
                    return true;
                }
            }

        }

        return true;
    }

    /// <summary>
    /// 유효한 문자가 나올 때 까지 커서를 이동시켜 줌
    /// </summary>
    private void SkipNoneCommand()
    {
        while (true)
        {
            if (_buffer[_bufferPos] == ' ' || _buffer[_bufferPos] == '\n' || _buffer[_bufferPos] == 0x09 || _buffer[_bufferPos] == 0x08)
            {
                _bufferPos++;
            }
            else if (_buffer[_bufferPos] == '/' && (_buffer[_bufferPos] + 1) == '/')
            {
                while (_buffer[_bufferPos] != '\n')
                {
                    _bufferPos++;
                }

            }
            else if (_buffer[_bufferPos] == '/' && _buffer[_bufferPos + 1] == '*')
            {
                while (_buffer[_bufferPos - 1] != '*' || _buffer[_bufferPos] != '/')
                {
                    _bufferPos++;

                    if (_bufferPos >= _buffer.Length - 1)
                    {
                        // 주석 해제 문자가 없는데 버퍼의 끝에 도달하면 오류로 간주
                        Debugger.Break();
                        return;
                    }
                }
                _bufferPos++;
            }
            else
            {
                break;
            }
        }
    }
    /// <summary>
    /// 버퍼 포인터가 끝에 도달했는지를 반환
    /// </summary>
    /// <returns></returns>
    private bool IsBufferPosEnd()
    {
        if (_bufferPos >= _buffer.Length - 1)
        {
            return true;
        }

        return false;
    }
    /// <summary>
    /// 문자가 숫자에 해당하는지를 반환
    /// </summary>
    /// <returns></returns>
    private bool IsNumberData()
    {
        if (_buffer[_bufferPos] >= 0x30 && _buffer[_bufferPos] <= 0x39)
        {
            return true;
        }

        return false;
    }
    /// <summary>
    /// BufferPos가 문자를 가리키지 않다면 true
    /// </summary>
    /// <returns></returns>
    private bool IsNotChar()
    {
        if (_buffer[_bufferPos] == ' ' || _buffer[_bufferPos] == '\n' || _buffer[_bufferPos] == 0x09 || _buffer[_bufferPos] == 0x08 ||
                   IsBufferPosEnd() ||
                   (_buffer[_bufferPos] == '/' && _buffer[_bufferPos + 1] == '/') || (_buffer[_bufferPos] == '/' && _buffer[_bufferPos + 1] == '*'))
        {
            return true;
        }

        return false;
    }

    private bool IsSemiColon()
    {
        if (_buffer[_bufferPos] == ';')
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}