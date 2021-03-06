//1. Объясни смысл следующего кода:
public sealed class Cinema
{
    private static volatile Cinema _cinema;
    private static readonly object SyncRoot = new object();
    private Cinema() { }

    public static Cinema GetInstance()
    {
        if (_cinema == null)
        {
            lock (SyncRoot)
            {
                if (_cinema == null)
                {
                    _cinema = new Cinema();
                }
            }
        }
        return _cinema;
    }
}

/*Ответ: в 1 задании реализован синглтон Cinema от которого нельзя наследоваться.
Внутри класса определены приватные поля: статическая ссылка на себя же и объект-заглушка, которая нужна для lock конструкции.
Конструктор помечен модификатором private – это значит что мы не можем привычным образом создавать экземпляры данного класса из вне.
Для создания экземпляра класса реализован метод GetInstance,
который во время  первого вызова вызывает конструктор класса Сinema а потом просто возвращает ссылку на уже существующий экземпляр.
При этом, даже если одновременно несколько потоков будут исполнять данный метод ,
конструктор внутри метода будет вызываться всего один раз благодаря конструкции lock(object) {} , таким образом данный синглтон является потокобезопасным.

Синглтон позволяет создать объект только при его необходимости. Если объект не нужен, то он не будет создан. 
