//3*. Проанализируй реализацию и предложи рефакторинг, позволяющий расширять форматы обрабатываемых файлов.
//Комментарий: Хорошо, что убраны лишние вызовы обработки содержимого файлов.
//Но как предложенный вариант рефакторинга позволит добавить обработку json, xml, csv и например css?
//Каждый раз переписывать метод Process, расширять swich, добавлять приватные методы ProcessJson, ProcessXml  и т.п.? 
public class FileService
{
    enum TextType
    {
    	HtmlCode,
    	RawText
    }
	
    public void Process(string filePath)
    {
    	var streamReader = new StreamReader(File.OpenRead(filePath));
    	var text = streamReader.ReadToEnd();
   	 
    	if (text.IndexOf("<html") != -1)
    	{
            ProcessHtmlCode(text);
    	}
    	else
    	{
            ProcessRawText(text);
    	}
    	ProcessText(
            text,
            text.IndexOf("<html") != -1
            ? TextType.HtmlCode
            : TextType.RawText);
   
    	streamReader.Close();
    }
	
    private void ProcessText(string text, TextType textType)
    {
    	switch (textType)
    	{
            case TextType.HtmlCode:
                ProcessHtmlCode(text);
                break;
            case TextType.RawText:
                ProcessRawText(text);
                break;
            default:
                throw new Exception("Unknown file format");
    	}
    }
	
    private void ProcessHtmlCode(string content){/*реализация метода*/}
	
    private void ProcessRawText(string content){/*реализация метода*/}
}

/*Ответ
Ответ: В данном коде мне не нравится следующее: 
1. Методы, в которых содержаться чтение файлов, лучше сделать асинхроными и использовать SteamReaderAsync
2. Метод Process определяет формат неправильно, так как нельзя определить формат файла по его содержимому
3. Определение параметра через тернарный оператор – это очень плохо, если учитывать, что форматов может быть много.
4.  В методе Process вызывается 2 раза метод обработки файла: в блоке if-else и в вызываемом методе ProcessText (зачем? непонятно…)
5. ProcessText вообще не нужен, если учитывать, что методы, обрабатывающие данные, везде принимают просто строку
6.Здесь можно обойтись и без enum.
*/
//Решение:
public static class FileService
{     
    public  static void Process(string filePath)
    {
         FileInfo fileInf = new FileInfo(filePath);
         var streamReader = new StreamReader(File.OpenRead(filePath));
         var text = streamReader.ReadToEnd();

         switch (fileInf.Extension)
         {
             case ".htm":
                 ProcessHtmlCode(text);
                 break;
             сase ".txt":
                 ProcessRawText(text);
                 break;
             default:
                 throw new Exception("Unknown file format");
	 }
         streamReader.Close();
    }

    private static void ProcessHtmlCode(string content)
    {
        Console.WriteLine("Обработан Html файл");
    }

    private static void ProcessRawText(string content)
    {
        Console.WriteLine("Обработан Txt файл");
    }
}

/*возможное Решение:

(Идея пришла в голову 4 октября 23:50 )В данном случаее стоит использовать Фабричный метод,  который определяет интерфейс для создания объектов некоторого класса, 
но непосредственное решение о том, объект какого класса создавать происходит в подклассах. То есть данный паттерн предполагает,
что базовый класс делегирует создание объектов классам-наследникам.*/
