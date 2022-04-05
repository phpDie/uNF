using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerParametrs;
using CarTunr;

public enum MathToEngineEqualsStruct
{
    upgrade,
    downgrade,
    none,
}

public struct MathToEngineResponseStruct
{
    public Dictionary<string, int> levels;
    public Dictionary<BaseAttribute, float> powerOriginal;
    public Dictionary<BaseAttribute, float> powerUpgrade;
    public Dictionary<BaseAttribute, float> deltaPower;
    public Dictionary<BaseAttribute, MathToEngineEqualsStruct> isUpgrade;
}


public class EngineMath : MonoBehaviour
{
    [Header("База данных")]
    public Project_DatabaseClass projectDatabaseClass;

    [Header("Тачка")]
    public CarConfigBase carConfigBase;


    public void Init(CarConfigBase _carConfigBase)
    {
		print("EngineMath init");
        carConfigBase = _carConfigBase;

    }
     


    public Dictionary<BaseAttribute, float> Read_EngineBaseAttibuteDic(List<EngineBaseAttibute> _list)
    {
        Dictionary<BaseAttribute, float> _result = new Dictionary<BaseAttribute, float>();

        for (int i = 0; i < _list.Count; i++)
        {
            _result.Add(_list[i].ind, _list[i].val);
        }

        return _result;
    }



    public MathToEngineResponseStruct MathToEngineLevelList(Dictionary<string, int> levels)
    {
        MathToEngineResponseStruct response = new MathToEngineResponseStruct();
        response.levels = levels;

        //записываем оригинальные и будущие характеристики
        response.powerOriginal = Read_EngineBaseAttibuteDic(carConfigBase.engineAttribute);
        response.powerUpgrade = Read_EngineBaseAttibuteDic(carConfigBase.engineAttribute);


        //прходимся по списку всех возможных тюнингов
        for (int i = 0; i < projectDatabaseClass.indEngineTune.list.Count; i++)
        {
            ItemTunePowerStruct Parametr = projectDatabaseClass.indEngineTune.list[i];

            //проверяем есть ли в списке левелов этот  тюнинг
            if (!levels.ContainsKey(Parametr.ind)) continue;

            int _theLevel = levels[Parametr.ind];


            if (_theLevel == 0) continue;
            if (_theLevel > Parametr.maxLevel)
            {
                print("ошибка! У тачки левел " + Parametr.ind + " больше максимума!");
                _theLevel = Parametr.maxLevel;
            }

            //  print("LEVEL TEST " + Parametr.ind + " = " + _theLevel.ToString());
            for (int j = 0; j < Parametr.boostForLevel.Count; j++)
            {
                BoosterTunePowerStruct _U = Parametr.boostForLevel[j];
                if (!response.powerUpgrade.ContainsKey(_U.ind)) continue;

                //  print("xr set " + _U.ind.ToString());
                // print("xr set " + _U.ind.ToString() + " = " + _theLevel.ToString());
                //  print(_U.boostCofficient);
                // print(response.powerOriginal[_U.ind]);
                response.powerUpgrade[_U.ind] += response.powerOriginal[_U.ind] * _U.boostCofficient * _theLevel;
            }

        }
        return response;
    }


    //словарь левелов это тупо Turbo=3 lev
    //вычисляем хр машины после апгрейда по словарю левелов
    public MathToEngineResponseStruct GetMathEngineToLevel_Delta(Dictionary<string, int> levelsOrig, Dictionary<string, int> levelsUpgrade)
    {
        MathToEngineResponseStruct response = new MathToEngineResponseStruct();
        response.levels = levelsUpgrade;


        MathToEngineResponseStruct responseOrig = MathToEngineLevelList(levelsOrig);
        MathToEngineResponseStruct responseNew = MathToEngineLevelList(levelsUpgrade);
         

        response.powerOriginal = responseOrig.powerUpgrade;
        response.powerUpgrade = responseNew.powerUpgrade;



        response.deltaPower = new Dictionary<BaseAttribute, float>();
        response.isUpgrade = new Dictionary<BaseAttribute, MathToEngineEqualsStruct>();

        //генерим делту и что стало лучше
        for (int i = 0; i < projectDatabaseClass.indEngineTune.engineAttribute.Count; i++)
        {
            BaseEngineAttributeStruct Attribute = projectDatabaseClass.indEngineTune.engineAttribute[i];
            BaseAttribute ind = Attribute.ind;

            response.deltaPower[ind] = response.powerUpgrade[ind] - response.powerOriginal[ind];
            if (response.deltaPower[ind] == 0)
            {
                response.isUpgrade[ind] = MathToEngineEqualsStruct.none;
                continue;
            }

            if (response.deltaPower[ind] > 0)
            {
                if (Attribute.isSpinUp)
                {
                    response.isUpgrade[ind] = MathToEngineEqualsStruct.upgrade;
                }
                else
                {
                    response.isUpgrade[ind] = MathToEngineEqualsStruct.downgrade;
                }

            }
            else
            {
                if (!Attribute.isSpinUp)
                {
                    response.isUpgrade[ind] = MathToEngineEqualsStruct.upgrade;
                }
                else
                {
                    response.isUpgrade[ind] = MathToEngineEqualsStruct.downgrade;
                }
            }

        }


        // print("Clone test");
        //  print(powerOriginal[BaseAttribute.maxSpeed]);
        //  print(powerUpgrade[BaseAttribute.maxSpeed]);

        return response;
    }


    //словарь левелов это тупо Turbo=3 lev
    //вычисляем хр машины после апгрейда по словарю левелов
    public MathToEngineResponseStruct MathToEngineLevelList_Old(Dictionary<string, int> levels)
    {
        MathToEngineResponseStruct response = new MathToEngineResponseStruct();
        response.levels = levels;

        //записываем оригинальные и будущие характеристики
        response.powerOriginal = Read_EngineBaseAttibuteDic(carConfigBase.engineAttribute);
        response.powerUpgrade = Read_EngineBaseAttibuteDic(carConfigBase.engineAttribute);


        //прходимся по списку всех возможных тюнингов
        for (int i = 0; i < projectDatabaseClass.indEngineTune.list.Count; i++)
        {
            ItemTunePowerStruct Parametr = projectDatabaseClass.indEngineTune.list[i];

            //проверяем есть ли в списке левелов этот  тюнинг
            if (!levels.ContainsKey(Parametr.ind)) continue;

            int _theLevel = levels[Parametr.ind];


            if (_theLevel == 0) continue;
            if (_theLevel > Parametr.maxLevel)
            {
                print("ошибка! У тачки левел " + Parametr.ind + " больше максимума!");
                _theLevel = Parametr.maxLevel;
            }

          //  print("LEVEL TEST " + Parametr.ind + " = " + _theLevel.ToString());
            for (int j = 0; j < Parametr.boostForLevel.Count; j++)
            {
                BoosterTunePowerStruct _U = Parametr.boostForLevel[j];
                if (!response.powerUpgrade.ContainsKey(_U.ind)) continue;

              //  print("xr set " + _U.ind.ToString());
               // print("xr set " + _U.ind.ToString() + " = " + _theLevel.ToString());
              //  print(_U.boostCofficient);
               // print(response.powerOriginal[_U.ind]);
                response.powerUpgrade[_U.ind] += response.powerOriginal[_U.ind] * _U.boostCofficient * _theLevel;
            }

        }


        response.deltaPower = new Dictionary<BaseAttribute, float>();
        response.isUpgrade = new Dictionary<BaseAttribute, MathToEngineEqualsStruct>();

        //генерим делту и что стало лучше
        for (int i = 0; i < projectDatabaseClass.indEngineTune.engineAttribute.Count; i++)
        {
            BaseEngineAttributeStruct Attribute = projectDatabaseClass.indEngineTune.engineAttribute[i];
            BaseAttribute ind = Attribute.ind;

            response.deltaPower[ind] = response.powerUpgrade[ind] - response.powerOriginal[ind];
            if (response.deltaPower[ind] == 0)
            {
                response.isUpgrade[ind] = MathToEngineEqualsStruct.none;
                continue;
            }

            if (response.deltaPower[ind] > 0)
            {
                if (Attribute.isSpinUp)
                {
                    response.isUpgrade[ind] = MathToEngineEqualsStruct.upgrade;
                }
                else
                {
                    response.isUpgrade[ind] = MathToEngineEqualsStruct.downgrade;
                }

            }
            else
            {
                if (!Attribute.isSpinUp)
                {
                    response.isUpgrade[ind] = MathToEngineEqualsStruct.upgrade;
                }
                else
                {
                    response.isUpgrade[ind] = MathToEngineEqualsStruct.downgrade;
                }
            }

        }


       // print("Clone test");
        //  print(powerOriginal[BaseAttribute.maxSpeed]);
        //  print(powerUpgrade[BaseAttribute.maxSpeed]);

        return response;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
 