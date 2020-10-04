//Задание 3:
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

*/
