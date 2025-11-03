// See https://aka.ms/new-console-template for more information


//main에선 Parser 테스트 확인 정도 하기

CTextParser parser = new CTextParser();

parser.OpenTextFile("parsertest.txt");

int number;
int good;
parser.SearchData<int>(out number, "number");
parser.SearchData<int>(out good, "good");

Console.WriteLine($"number : {number}");
Console.WriteLine($"good : {good}");


