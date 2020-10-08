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
using System;
using System.IO;


namespace RamblerTest
{
    class Program
    {
        static void Main(string[] args)
        {

            FileService.Process("D:/test.htm");
        }
    }


    public static  class FileService
    {

        private static IFileProcessor[] processors;
        
        static FileService()
        {
            processors = new[] {   
                new ProcessText() ,
                new ProcessHtml() as IFileProcessor
            };
        }

        public static void Process(string filePath)
        {
            bool flag = false;
            foreach (IFileProcessor fileProcessor in processors)
            {
                if (fileProcessor.CatchMatch(filePath))
                {
                    fileProcessor.ProcessFile(filePath);
                    flag = true;
                    break;
                }
            }
            if (!flag)
                Console.WriteLine("Неизвестный формат файла");
        }
    }


    public  interface IFileProcessor
    {
        public bool CatchMatch(string path);
        public void ProcessFile(string path);
    }


    public class ProcessHtml : IFileProcessor
    {
        public bool CatchMatch(string filePath)
        {
            FileInfo fileInf = new FileInfo(filePath);      
            if(fileInf.Extension == ".htm")  
                return true;    
            return false;
        }

        public void ProcessFile(string filePath)
        {
            var streamReader = new StreamReader(File.OpenRead(filePath));
            var text = streamReader.ReadToEnd();
            ///...
            Console.WriteLine("Обработан Html файл");
        }
    }

    public class ProcessText : IFileProcessor
    {
        public bool CatchMatch(string filePath)
        {
            FileInfo fileInf = new FileInfo(filePath);
            if (fileInf.Extension == ".txt")
                return true;
            return false;
        }

        public void ProcessFile(string filePath)
        {
            var streamReader = new StreamReader(File.OpenRead(filePath));
            var text = streamReader.ReadToEnd();
            ///...
            Console.WriteLine("Обработан TXT файл");
        }
    }
}

