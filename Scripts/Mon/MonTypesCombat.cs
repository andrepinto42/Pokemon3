/*
Attack and Defense:
    Perfuration -> Ataques como mordidas, usar as garras
    Impact -> Quando usa a sua força bruta, como um murro ou uma cabecada
    Warcry -> Habilidade quando o pokemon invoca algum tipo de objeto
    Aura -> Habilidade quando o pokemon usa sua habilidade Interior para atacar
*/
public enum TypeOfDamage{
    PIERCE = 0,
    IMPACT = 1,
    WARCRY = 2,
    AURA = 3
}
[System.Serializable]
public struct MonTypesCombat{
    public float[] arrayAtributtes;
    public MonTypesCombat(float i1,float i2,float i3,float i4){
        arrayAtributtes = new float[4];

        arrayAtributtes[0]  = i1;
        arrayAtributtes[1]  = i2;
        arrayAtributtes[2]  = i3;
        arrayAtributtes[3]  = i4; 
    }
}