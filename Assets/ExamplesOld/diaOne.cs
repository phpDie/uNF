
using System.Collections.Generic;
using UnityEngine;




public enum EnumDiaMainIf
{
Нет, ХорошаяРепутация,ПлохаяРепутация, СредняяРепутация, МалоХп, ИгрокМалоХп,
ИгрокАрмияБольшая,
ИгрокАрмияМаленькая,
    ИгрокАрмииНет,
    ИгрокАрмияСредняя,
    ИгрокАрмияМаленькаяИлиНет,

    //эти варианты срабатывают только раз в час, в текущем мобе(названиеGo_roomName). 
    //Запись работает на всего моба. То есть все ответы и if они в одной копилке
    //однако для час,полЧ,5сек - копилки разные, в рамках одного бота!
    РазВЧас,
    РазВПолЧаса,
    РазВ5Сек,
    РепутацияЗавоевателя,
    ЕстьКвест,
    ЕстьКвестВыполненный,
}

public enum EnumDiaMainFunctionType
{
    Нет,НапарникИнформация,
}
public enum EnumDiaOtvType
{
    Обычный = 0,
    Взятка = 1,
    ВызовНаБой = 2,
    Торговля = 3,
    Назад = 4,
    Выход = 5,
    ВступитьВГруппу = 6,
    Убеждение = 7,
    СледСцена = 8,
    Напарник = 9,
    НапарникУдалить = 10,
    НапарникВещи = 11,
    НапарникUse= 12,
    Квест = 13,
    КвестСдать = 14,
}



[System.Serializable]
public class SDiaVariants
{
    [TextArea]
    public string text;


    [Header("Тип ответа")]
    public EnumDiaOtvType type; //otv giveMoney


    [Header("Условие пока ответа")]
    public EnumDiaMainIf functionIf;

    [Header("Сумма взятки")]
    public int val= 0; //колв денег например

    public int reactionValue;

    public DiaOne linkNext;

    public bool hotKeyEsc =false;
        

}

[CreateAssetMenu(fileName = "New Dia")]
public class DiaOne : ScriptableObject
{

    [TextArea]
    public string textMain = "Привет, чё как?";


    [Header("Условие диалога")]
    public EnumDiaMainIf functionIf;


    [Header("Доп функция диалога")]
    public EnumDiaMainFunctionType functionType;

     


    [Header("Ответы")]
    public List<SDiaVariants> listVarz = new List<SDiaVariants>();

  
    [Header("Кнопки иконки")]
    public Dictionary<KeyCode, string> IconsKey = new Dictionary<KeyCode, string>();
    /*

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
