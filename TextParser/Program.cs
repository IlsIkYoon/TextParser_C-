// See https://aka.ms/new-console-template for more information


//main에선 Parser 테스트 확인 정도 하기

using System.Diagnostics;

CTextParser parser = new CTextParser();

parser.OpenTextFile("parsertest.txt");

int number;
int good;
string str = "";
parser.SearchData<int>(out number, "number");
parser.SearchData<int>(out good, "good");
parser.SearchData(out str, "str");


Console.WriteLine($"number : {number}");
Console.WriteLine($"good : {good}");
Console.WriteLine($"str : {str}");

